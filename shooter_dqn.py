# -*- coding: utf-8 -*-
"""
Created on Tue May  8 13:12:57 2018

@author: ahmetakin
"""

import os
import sys
import numpy as np
import tensorflow as tf
import matplotlib.pyplot as plt
from datetime import datetime
import csv

from unityagents import UnityEnvironment

#print("Python version:")
#print(sys.version)


class hidden_layer:
    def __init__(self,L1,L2,f=tf.nn.tanh,use_bias=True): #l1,l2 ye göre weight ler belirledik
        self.W=tf.Variable(tf.random_normal(shape=(L1,L2)))#rasgele sayı verdik
        self.params=[self.W] 
        self.use_bias=use_bias
        if use_bias: #bias varsa bias ekledik
            self.bias=tf.Variable(np.zeros(L2).astype(np.float32))
            self.params.append(self.bias)
        self.f=f
    def forward(self,X): #girilen değere göre yattıgımız weightlerle layer hesaplanıyor z değerleri vector olarak
        if self.use_bias:
            a=tf.matmul(X,self.W)+self.bias
        else:
            a=tf.matmul(X,self.W)
        return self.f(a) #bulunan değer de aktivasyon fonk ile elde ediliyor yani matmul çarpımlarını akt ile 

class DeepQNetwork:
    #def __init__(self,D,K,hiddenlayersizes,gamma,max_exp=1350,min_exp=850,batch_sz=170):
    def __init__(self,D,K,hiddenlayersizes,gamma,max_exp=50000,min_exp=5000,batch_sz=170):
        self.K=K #action kleri 6 adet

        self.layers=[] #katmanlar
        L1=D # d burda obs değeri 9 adet
        for L2 in hiddenlayersizes:#verilen layer adetine gör burda 2 şuan
            layer=hidden_layer(L1,L2)#ilk tur 1. katman 9x200 ikincide 200x200
            self.layers.append(layer)#elde edilen layer değerleri ekleniyor
            L1=L2

        #son layer
        layer=hidden_layer(L1,K,lambda x:x) #200x6
        self.layers.append(layer) #layerlere ekledi

        #ana network ten hedef network a kopyalama için kolaylık olması acısından
        self.params=[]
        for layer in self.layers:
            self.params+=layer.params

        #input returns targets
        self.X=tf.placeholder(tf.float32,shape=(None,D),name='X')#matrix ,9
        self.G=tf.placeholder(tf.float32,shape=(None,),name='G')#vector ,
        self.actions=tf.placeholder(tf.int32,shape=(None,),name='actions')#vecctor ,

        #calc output and cost
        Z=self.X
        for layer in self.layers:
            Z=layer.forward(Z) #z ler hesaplanıyor yani katmanlar layer lar
        Y_hat=Z #yani olasılıgıhı bulunuyor
        self.predict_op=Y_hat

        selected_act_values=tf.reduce_sum(
        	Y_hat*tf.one_hot(self.actions,K),#indices,depth
        	reduction_indices=[1]
        	)

        cost = tf.reduce_sum(tf.square(self.G-selected_act_values))
        #self.train_o=tf.train.GradientDescentOptimizer(10e-5).minimize(cost) #maliyeti düşürmek için momentum opstimizer yani bi bakıma backgrog grad
        self.train_o=tf.train.AdagradOptimizer(10e-3).minimize(cost)
        #self.train_o=tf.train.MomentumOptimizer(10e-5,momentum=0.9).minimize(cost)
        #self.train_o=tf.train.AdamOptimizer(10e-5).minimize(cost)
        self.exp = {'s': [], 'a': [], 'r': [], 's2': [], 'done': []}
        self.max_exp=max_exp
        self.min_exp=min_exp
        self.batch_sz=batch_sz
        self.gamma=gamma

    def set_session(self,session):
        self.session=session

    def copy_from(self,other):#dual network staffs
        ops=[]
        my_params=self.params
        other_params=other.params
        for p,q in zip(my_params,other_params):
            actual=self.session.run(q)
            op=p.assign(actual)
            ops.append(op)
        self.session.run(ops)

    def predict(self,X):
        X= np.atleast_2d(X)        
        return self.session.run(self.predict_op,feed_dict={self.X: X}) #predict_o you verilen x e gmre buulacak

    def train(self,target_n):
        if len(self.exp['s']) < self.min_exp:
            #print("test")
            return 
        #print("test1")
        idx = np.random.choice(len(self.exp['s']),size=self.batch_sz,replace=False) #batch uzunlugunda len s sayısına kadar array örneğin 5 e kadar olan sayılardan rasgele 3 sayı
        states = [self.exp['s'][i] for i in idx]
        actions = [self.exp['a'][i] for i in idx]
        rewards = [self.exp['r'][i] for i in idx]
        next_states= [self.exp['s2'][i] for i in idx]
        dones=[self.exp['done'][i] for i in idx]
        next_Q = np.max(target_n.predict(next_states),axis=1)#max q yu predict ediyoruz
        targets = [r + self.gamma * next_q if not done else r for r,next_q,done in zip(rewards,next_Q,dones)]

        self.session.run(self.train_o,feed_dict={self.X:states,self.G:targets,self.actions:actions})

    def add_experience(self,s,a,r,s2,done):
        #print("length",len(self.exp['s']))
        if len(self.exp['s']) >= self.max_exp: #eğer uzunluk aşarsa sil
          self.exp['s'].pop(0)
          self.exp['a'].pop(0)
          self.exp['r'].pop(0)
          self.exp['s2'].pop(0)
          self.exp['done'].pop(0)
        self.exp['s'].append(s)
        self.exp['a'].append(a)
        self.exp['r'].append(r)
        self.exp['s2'].append(s2)
        self.exp['done'].append(done)

    def sample_action(self,x,epsilon):
        if np.random.random() < epsilon:
            return np.random.choice(self.K)
        else:
            X = np.atleast_2d(x)
            return np.argmax(self.predict(X)[0])

def play_one(env,model,tmodel,epsilon,gamma,copy_period):    
    train_mode=True
    default_brain = env.brain_names[0]
    env_info = env.reset(train_mode=train_mode)[default_brain]
    done=False
    totalreward=0
    iters=0
    kill=0
    obsv=env_info.vector_observations[0]
    while not done: #tüm düşmanlar yok olunca yada adım hakkı bitince
        action=model.sample_action(obsv,epsilon)
        prev_observation = obsv #observation.vector_observations[0]
        action1=int(action)
        env_info = env.step(action1)[default_brain]
        obsv= env_info.vector_observations[0]
        reward = env_info.rewards[0]
        done = env_info.local_done[0]
        totalreward +=reward
        if reward > 4:
            kill+=1
        if done:
            reward = -200

        model.add_experience(prev_observation,action1,reward,obsv,done)
        model.train(tmodel)

        iters +=1

        if iters % copy_period ==0:
            tmodel.copy_from(model)         
    return totalreward ,kill

def plot_running_avg(totalrewards): #plot avg
  N = len(totalrewards)
  running_avg = np.empty(N)
  for t in range(N):
    running_avg[t] = totalrewards[max(0, t-100):(t+1)].mean()
  plt.plot(running_avg)
  plt.title("Running Average")
  plt.show()
  
def main():
    env_name="shooter"    
    env = UnityEnvironment(file_name=env_name)
    gamma=0.99
    copy_period=100

    D = 3 #observation space a göre
    K = 4 #action space e göre
    sizes =[200,200]
    model = DeepQNetwork(D,K,sizes,gamma)
    tmodel =DeepQNetwork(D,K,sizes,gamma)
    init=tf.global_variables_initializer()
    #session=tf.InteractiveSession(config=tf.ConfigProto(log_device_placement=True))
    session=tf.InteractiveSession()
    session.run(init)
    model.set_session(session)
    tmodel.set_session(session)

    N=200
    totalrewards=np.empty(N)
    epsilons=[]
    costs=np.empty(N)
    totalrewards2=[]
    kills=[]
    timetaken=[]
    startTime=datetime.now()    
    for n in range(N):
        epsilon=1.0/np.sqrt(n+1)
        epsilons.append(epsilon)
        totalreward , kill=play_one(env,model,tmodel,epsilon,gamma,copy_period)
        kills.append(kill)
        totalrewards[n]=totalreward
        totalrewards2.append(totalreward)
        timedif=datetime.now() - startTime
        timetaken.append(str(timedif))
        print("episode:", n, "total reward:", totalreward, "eps:", epsilon, "avg reward:", np.mean(totalrewards2),"kill count",kill, "Time taken:", datetime.now() - startTime,"\n")

    print("Avg reward for last 100 episodes:", totalrewards[-100:].mean())

    rewardsfile = open('shooterlogs/rewards.csv','w',newline='')
    rewardsfilewrite = csv.writer(rewardsfile)
    rewardsfilewrite.writerows(map(lambda x: [x], totalrewards2))
    rewardsfile.close()

    epsilonfile = open('shooterlogs/epsilon.csv','w',newline='')
    epsilonfilewrite = csv.writer(epsilonfile)
    epsilonfilewrite.writerows(map(lambda x: [x], epsilons))
    epsilonfile.close()

    killsfile = open('shooterlogs/kills.csv','w',newline='')
    killsfilewrite = csv.writer(killsfile)
    killsfilewrite.writerows(map(lambda x: [x], kills))
    killsfile.close()

    timetakenfile = open('shooterlogs/timetaken.txt','w')
    for i in range(len(timetaken)):
        timetakenfile.write(timetaken[i]+"\n")
    timetakenfile.close()

    plot_running_avg(totalrewards)


if __name__ == '__main__':
    main()