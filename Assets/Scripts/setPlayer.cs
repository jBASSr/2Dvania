using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPlayer : MonoBehaviour {

	private GameObject robot;
	private GameObject toDoor = null;
	public float fallLimit = 0.0f;
	private Vector2 lastPosition;
	// Use this for initialization
	void Start () {		
		robot = GameObject.Find ("Robot");
		Debug.LogError("door_start=" + GameManager.door_start);
		if (GameManager.door_start != "") {
			toDoor = GameObject.Find (GameManager.door_start);
			if (GameObject.Find (GameManager.door_start) !=null){
			robot.transform.position = toDoor.transform.position;
			//Camera cam = Camera.main;	
			//cam.gameObject.transform.position = toDoor.transform.position;
			}
		}
	}

}
