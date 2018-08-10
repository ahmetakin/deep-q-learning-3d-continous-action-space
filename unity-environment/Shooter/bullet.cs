using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnCollisionEnter(Collision col){
		shooteragent agent = GetComponent<shooteragent>();
		if (col.gameObject.tag == "enemy") {
			//Destroy (col.gameObject);
			col.gameObject.SetActive(false);
			agent.AddReward(15f);
		}
	}
}
