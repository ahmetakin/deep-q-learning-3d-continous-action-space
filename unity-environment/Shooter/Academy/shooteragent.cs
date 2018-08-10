using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class shooteragent : Agent {
	
	RayPerception rayPer;

	public GameObject bulletExit;
	public GameObject bullet;
	public float bulletForwardForce;

	public float agentRunSpeed;

	public GameObject enemyOne;
	public GameObject enemyTwo;
	public GameObject enemyThree;
	public GameObject enemyFour;
	public GameObject enemyFive;
	public GameObject enemySix;

	public Vector3 enemy3From;
	public Vector3 enemy3To;
	public float enemy3Speed;

	public Text Text;
	public Text Text1;

	Rigidbody rBody;

	void Start () {
		rBody = GetComponent<Rigidbody>();
		StartCoroutine (MoveEnemy3(enemy3From));
	}
	IEnumerator MoveEnemy3(Vector3 target){
		while (Mathf.Abs ((target - enemyThree.transform.localPosition).x) > 0.20f) {
			Vector3 direction = target.x == enemy3From.x ? Vector3.left : Vector3.right;
			enemyThree.transform.localPosition += direction * (enemy3Speed * Time.deltaTime);
			yield return null;
		}

		yield return new WaitForSeconds(0.2f);

		Vector3 newTarget = target.x==enemy3From.x ? enemy3To : enemy3From;

		StartCoroutine(MoveEnemy3(newTarget));
	}
	public void MoveAgent(float[] act)
	{		
		Vector3 dirToGo = Vector3.zero;
		Vector3 rotateDir = Vector3.zero;

		int action = Mathf.FloorToInt(act[0]);

		switch (action)
		{
		case 0:
			dirToGo = transform.forward * 1f;
			break;
		case 1:
			dirToGo = transform.forward * -1f;
			break;
		/*case 2:
			rotateDir = transform.up * 1f;
			break;
		case 3:
			rotateDir = transform.up * -1f;
			break;*/
		case 2:
			dirToGo = transform.right * -0.75f;
			break;
		case 3:
			dirToGo = transform.right * 0.75f;
			break;
		case 4:
			fire ();
			break;
		}
		transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
		rBody.AddForce(dirToGo * agentRunSpeed,ForceMode.VelocityChange);

	}

	public float detectLenwall;
	public float rewardd;
	public override void AgentAction(float[] vectorAction, string textAction)
	{
		Text.text = string.Format ("Reward at Step {0}",GetReward());
		Text1.text = string.Format ("Step {0}",GetStepCount());
		RaycastHit hit;
		Ray wallDetectorforward = new Ray (transform.position, transform.TransformDirection(Vector3.forward));
		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.forward), Color.red);

		var cross = (transform.forward + transform.right).normalized;
		Ray wallDetectorforwardcross = new Ray (transform.position, transform.TransformDirection(cross).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross), Color.red);

		var cross2 = (transform.forward - transform.right).normalized;
		Ray wallDetectorforwardcross2 = new Ray (transform.position, transform.TransformDirection(cross2).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross2), Color.red);

		var cross3 = (-transform.forward - transform.right).normalized;
		Ray wallDetectorforwardcross3 = new Ray (transform.position, transform.TransformDirection(cross3).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross3), Color.red);

		var cross4 = (-transform.forward + transform.right).normalized;
		Ray wallDetectorforwardcross4 = new Ray (transform.position, transform.TransformDirection(cross4).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross4), Color.red);

		var cross5 = Quaternion.AngleAxis(22.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross5 = new Ray (transform.position, transform.TransformDirection(cross5).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross5), Color.green);

		var cross6 = Quaternion.AngleAxis(67.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross6 = new Ray (transform.position, transform.TransformDirection(cross6).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross6), Color.green);

		var cross7 = Quaternion.AngleAxis(112.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross7 = new Ray (transform.position, transform.TransformDirection(cross7).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross7), Color.green);

		var cross8 = Quaternion.AngleAxis(157.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross8 = new Ray (transform.position, transform.TransformDirection(cross8).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross8), Color.green);

		var cross9 = Quaternion.AngleAxis(202.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross9 = new Ray (transform.position, transform.TransformDirection(cross9).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross9), Color.green);

		var cross10 = Quaternion.AngleAxis(247.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross10 = new Ray (transform.position, transform.TransformDirection(cross10).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross10), Color.green);

		var cross11 = Quaternion.AngleAxis(292.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross11 = new Ray (transform.position, transform.TransformDirection(cross11).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross11), Color.green);

		var cross12 = Quaternion.AngleAxis(337.5f, transform.up) * transform.forward;
		Ray wallDetectorforwardcross12 = new Ray (transform.position, transform.TransformDirection(cross12).normalized);
		Debug.DrawRay (transform.position, transform.TransformDirection(cross12), Color.green);


		Ray wallDetectorback = new Ray (transform.position, transform.TransformDirection(Vector3.back));
		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.back), Color.red);

		Ray wallDetectorleft = new Ray (transform.position, transform.TransformDirection(Vector3.left));
		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.left), Color.red);

		Ray wallDetectorright = new Ray (transform.position, transform.TransformDirection(Vector3.right));
		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.right), Color.red);


		if ((Physics.Raycast (wallDetectorforwardcross8, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross7, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross6, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross12, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross11, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross10, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross9, out hit, detectLenwall)  ) || (Physics.Raycast (wallDetectorforwardcross5, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross4, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross3, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross2, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforward, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorback, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorleft, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorright, out hit, detectLenwall)) ) {
			if (hit.collider.tag == "wall") {
				print ("wall");
				AddReward (-0.5f);//50
			} else if (hit.collider.tag == "enemy") {
				//Destroy (col.gameObject);
				hit.collider.gameObject.SetActive (false);
				AddReward (5f);//500
			} 
//			/*else if (hit.collider.name == "enemy_static") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(40f);
//			}
//			else if (hit.collider.name == "enemy_static1 (1)") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(5f);
//			}
//			else if (hit.collider.name == "enemy_static1 (2)") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(25f);
//			}
//			else if (hit.collider.name == "enemy_static1 (3)") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(15f);
//			}
//			else if (hit.collider.name == "enemy_static1") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(10f);
//			}
//			else if (hit.collider.name == "enemy_dynamic") {
//				//Destroy (col.gameObject);
//				hit.collider.gameObject.SetActive(false);
//				AddReward(50f);
//			}

		}
		MoveAgent(vectorAction);
		AddReward(-0.00005f);//1
		if (!enemyOne.activeInHierarchy && !enemyTwo.activeInHierarchy && !enemyThree.activeInHierarchy && !enemyFour.activeInHierarchy && !enemyFive.activeInHierarchy && !enemySix.activeInHierarchy) {
			//print ("cabbiş");
			Done ();
		} else {
			//print ("solomon");
		}
	}
	public void spawnEnemies(){
		
		enemyOne.SetActive(true);
		enemyTwo.SetActive(true);
		enemyThree.SetActive(true);
		enemyFour.SetActive(true);
		enemyFive.SetActive(true);
		enemySix.SetActive(true);
		enemyOne.SetActive(true);
		enemyTwo.SetActive(true);
		enemyThree.SetActive(true);
		enemyFour.SetActive(true);
		enemyFive.SetActive(true);
		enemySix.SetActive(true);
		enemyOne.SetActive(true);
		enemyTwo.SetActive(true);
		enemyThree.SetActive(true);
		enemyFour.SetActive(true);
		enemyFive.SetActive(true);
		enemySix.SetActive(true);
		enemyOne.SetActive(true);
		enemyTwo.SetActive(true);
		enemyThree.SetActive(true);
		enemyFour.SetActive(true);
		enemyFive.SetActive(true);
		enemySix.SetActive(true);
	}
	public override void AgentReset(){	
		spawnEnemies();
		transform.position = new Vector3(3.994858f,0.275f,4.061983f);
	}
	public float detectLen;
	public override void CollectObservations()
	{		
		/*RaycastHit hit;
		Ray detectEnemy = new Ray (bulletExit.transform.position, bulletExit.transform.TransformDirection(Vector3.back));
		Debug.DrawRay (bulletExit.transform.position, bulletExit.transform.TransformDirection(Vector3.back), Color.green);

		if (Physics.Raycast (detectEnemy, out hit, detectLen)) {
			//AddVectorObs (hit.transform.position);
			if (hit.collider.tag == "enemy") {
				AddReward (10f);
				fire ();
			}
		} else {
			//AddVectorObs(new Vector3(0f,0f,0f));
		}*/

//		RaycastHit hit;
//		Ray wallDetectorforward = new Ray (transform.position, transform.TransformDirection(Vector3.forward));
//		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.forward), Color.red);
//
//		var cross = (transform.forward + transform.right).normalized;
//		Ray wallDetectorforwardcross = new Ray (transform.position, transform.TransformDirection(cross).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross), Color.red);
//
//		var cross2 = (transform.forward - transform.right).normalized;
//		Ray wallDetectorforwardcross2 = new Ray (transform.position, transform.TransformDirection(cross2).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross2), Color.red);
//
//		var cross3 = (-transform.forward - transform.right).normalized;
//		Ray wallDetectorforwardcross3 = new Ray (transform.position, transform.TransformDirection(cross3).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross3), Color.red);
//
//		var cross4 = (-transform.forward + transform.right).normalized;
//		Ray wallDetectorforwardcross4 = new Ray (transform.position, transform.TransformDirection(cross4).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross4), Color.red);
//
//		var cross5 = Quaternion.AngleAxis(22.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross5 = new Ray (transform.position, transform.TransformDirection(cross5).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross5), Color.green);
//
//		var cross6 = Quaternion.AngleAxis(67.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross6 = new Ray (transform.position, transform.TransformDirection(cross6).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross6), Color.green);
//
//		var cross7 = Quaternion.AngleAxis(112.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross7 = new Ray (transform.position, transform.TransformDirection(cross7).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross7), Color.green);
//
//		var cross8 = Quaternion.AngleAxis(157.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross8 = new Ray (transform.position, transform.TransformDirection(cross8).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross8), Color.green);
//
//		var cross9 = Quaternion.AngleAxis(202.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross9 = new Ray (transform.position, transform.TransformDirection(cross9).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross9), Color.green);
//
//		var cross10 = Quaternion.AngleAxis(247.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross10 = new Ray (transform.position, transform.TransformDirection(cross10).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross10), Color.green);
//
//		var cross11 = Quaternion.AngleAxis(292.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross11 = new Ray (transform.position, transform.TransformDirection(cross11).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross11), Color.green);
//
//		var cross12 = Quaternion.AngleAxis(337.5f, transform.up) * transform.forward;
//		Ray wallDetectorforwardcross12 = new Ray (transform.position, transform.TransformDirection(cross12).normalized);
//		Debug.DrawRay (transform.position, transform.TransformDirection(cross12), Color.green);
//
//
//		Ray wallDetectorback = new Ray (transform.position, transform.TransformDirection(Vector3.back));
//		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.back), Color.red);
//
//		Ray wallDetectorleft = new Ray (transform.position, transform.TransformDirection(Vector3.left));
//		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.left), Color.red);
//
//		Ray wallDetectorright = new Ray (transform.position, transform.TransformDirection(Vector3.right));
//		Debug.DrawRay (transform.position, transform.TransformDirection(Vector3.right), Color.red);
//
//
//		if ((Physics.Raycast (wallDetectorforwardcross8, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross7, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross6, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross12, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross11, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross10, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross9, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross5, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross4, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross3, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross2, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforwardcross, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorforward, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorback, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorleft, out hit, detectLenwall)) || (Physics.Raycast (wallDetectorright, out hit, detectLenwall))) {
//			if (hit.collider.tag == "wall") {
//				AddVectorObs (hit.transform.position);
//			} else if (hit.collider.tag == "enemy") {
//				AddVectorObs (hit.transform.position);
//			} 
//		}else {
//			AddVectorObs (new Vector3 (0f, 0f, 0f));
//		}

		AddVectorObs (transform.position);
		//AddVectorObs (transform.eulerAngles);
	}

	public void fire(){
		GameObject temp_bullet;
		temp_bullet = Instantiate (bullet, bulletExit.transform.position, bulletExit.transform.rotation) as GameObject;
		temp_bullet.transform.Rotate (Vector3.left * 90);

		Rigidbody temp_rigidbody;
		temp_rigidbody = temp_bullet.GetComponent<Rigidbody> ();
		temp_rigidbody.AddForce (transform.forward * bulletForwardForce);
		Destroy (temp_bullet, 2.0f);

	}
	public void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "wall") {
			AddReward (-0.005f);
		}

	}

}
