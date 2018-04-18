﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletLaserEvent : MonoBehaviour {
	public float bulletDuration = 2f;
	public float bulletTime = 0f;
	// TODO: Animations
	public GameObject bulletHit_Prefab;
	GameObject bulletHit;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		// Increment time from creation
		bulletTime += Time.deltaTime;
		if (bulletTime > bulletDuration) {
			Destroy (this.gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Enemy" || c.tag == "Ground") {
			//Debug.Log ("Bullet hit something");
			// Laser should keep going until it expires
			// Talk about creative code, right?
			//Destroy (this.gameObject);
			bulletHit = GameObject.Instantiate (bulletHit_Prefab, 
				transform.position, transform.rotation) as GameObject;
			Destroy (bulletHit, 0.6f);
		}
	}
}
