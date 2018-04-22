using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBadRobot : MonoBehaviour {

	private GameObject br;
	private BadRobotHUD brh;
	private bad_robot brs;
	private GameObject robot;
	// Use this for initialization
	private Camera cam;
	private float camera_width;
	private float camera_height;
	public GameObject brPrefab;
	private bool isBrSet;

	void Start () {
		isBrSet = false;
		robot = GameObject.Find ("Robot");
		Debug.LogError ("SETTING BAD ROBOT TO FALSE!");
		cam = Camera.main;
		camera_height = 2f * cam.orthographicSize;
		camera_width = camera_height * cam.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		if (isBrSet == false) {
			if ((Mathf.Abs (robot.transform.position.x - transform.position.x) < camera_width) && (Mathf.Abs (robot.transform.position.y - transform.position.y) < camera_height)) {								
				Debug.LogError ("INSTANTIATE BAD ROBOT:");	
				br = (GameObject)Instantiate (
					brPrefab,
					new Vector2(transform.position.x+5.0f,
						transform.position.y),
					transform.rotation);
				br.gameObject.name = "BAD_ROBOT";
				isBrSet = true;
			}
		}
	}
}
