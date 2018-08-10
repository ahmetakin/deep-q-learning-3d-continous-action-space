using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_dynamic : MonoBehaviour {

	[SerializeField] Vector3 position1;
	[SerializeField] Vector3 position2;
	[SerializeField] float speed;
	// Use this for initialization
	void Start () {
		StartCoroutine (Move(position1));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Move(Vector3 target){
		while (Mathf.Abs ((target - transform.localPosition).x) > 0.20f) {
			Vector3 direction = target.x == position1.x ? Vector3.left : Vector3.right;
			transform.localPosition += direction * (speed * Time.deltaTime);

				yield return null;
		}
		print ("reacted the target");

		yield return new WaitForSeconds(0.2f);

		Vector3 newTarget = target.x==position1.x ? position2 : position1;

		StartCoroutine(Move(newTarget));
	}
}
