﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour {
	private SimpleMovement robot;

	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot").GetComponent<SimpleMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D coll)
	{		
		if (coll.gameObject.tag == "Player"){
			Debug.Log("GOT THE KEY!");
			GameManager.hasKey = true;
			Destroy (this.gameObject);
		}
    }
}
