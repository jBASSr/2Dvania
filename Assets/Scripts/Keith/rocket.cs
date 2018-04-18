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
		//if (coll.gameObject.tag != "Enemy") {
			Destroy (this.gameObject);
		//}
	}
    
}
