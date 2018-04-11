using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPlayer : MonoBehaviour {

	private GameObject robot;
	private GameObject toDoor;
	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot");
		toDoor = GameObject.Find (GameManager.door_start);
		robot.transform.position = toDoor.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
