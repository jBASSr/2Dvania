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
		GameObject.Find ("Robot").GetComponent<PlayerHUD> ().curHealth = GameManager.currentHealth;
		robot = GameObject.Find ("Robot");
		Debug.LogError("door_start=" + GameManager.door_start);
		if (GameManager.door_start != "") {
			toDoor = GameObject.Find (GameManager.door_start);
			robot.transform.position = toDoor.transform.position;
		}
	}

}
