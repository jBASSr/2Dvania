using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil_mess_fall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{		
        if(coll.gameObject.tag == "Player"){
			Destroy (this.gameObject, 1.0f);
		}
		else{
			Destroy (this.gameObject, 0.0f);
		}
	}
}
