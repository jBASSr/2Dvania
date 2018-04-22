using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBadRobot : MonoBehaviour {

	// Use this for initialization
	private GameObject br;
	void Start () {
		br = GameObject.Find ("BAD_ROBOT");
		br.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
