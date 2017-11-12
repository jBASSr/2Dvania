﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	private ceo myCeo;

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log("TRIGGER CALLED");
		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("ENEMY SHOT!!!");
			myCeo = coll.GetComponent<ceo> ();
			myCeo.Bleed();
		}
		Destroy(this.gameObject, 0.0f);
	}
}