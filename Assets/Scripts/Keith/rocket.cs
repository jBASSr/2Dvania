using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		Debug.Log ("ROCKET COLLIDED!!!!!");
		//if (coll.gameObject.tag != "Enemey" || coll.gameObject.tag != "oil") {
			Destroy (this.gameObject);
		//}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.tag == "Enemy") {
			Destroy (this.gameObject);
		}
	}
}
