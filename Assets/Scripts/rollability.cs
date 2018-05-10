using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollability : MonoBehaviour {

	private GameObject exitDoorPrefab;
	// Use this for initialization
	void Start () {
		//exitDoorPrefab = GameObject.Find ("DoorExit");
		//exitDoorPrefab.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{		
		if (coll.gameObject.tag == "Player") {
			Debug.LogError ("PLAYER NOW HAS ROLLABILITY!");
			GameManager.canRoll = true;
			FindObjectOfType<AudioManager_2> ().Play ("Grab");
			Destroy (this.gameObject);
			//exitDoorPrefab.SetActive (true);
		}
	}
}
