﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	private ceo myCeo;
	private SimpleMovement player;

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log("TRIGGER CALLED game tag=" + coll.gameObject.tag);
		/*if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("ENEMY SHOT!!!");
			myCeo = coll.GetComponent<ceo> ();
			myCeo.Bleed();
		}
		*/
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("PLAYER SHOT!!!");
			player = coll.GetComponent<SimpleMovement> ();
			//player.Bleed();
		}
		if (coll.gameObject.tag != "Enemy") {
			Destroy (this.gameObject, 0.0f);
		}
	}
}