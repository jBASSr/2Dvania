using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour {

	// Camera Follow Target
	public Transform cameraTarget;
	//public GameObject player;
	// Camera Settings
	public float trackSpeed = 1.0f;
	//public Vector3 offset;
	// Tracking
	private Vector3 cameraPosition;
	private Vector3 playerPosition;
	private Vector3 prevPosition;
	private Rect windowRect;

	public float smoothX = 2;
	public float smoothY = 5;
	public float y_offset = 6.0f;
	public float marginX;
	public float marginY;
	//public Vector2 maxXY = new Vector2 (30, 10);
	//public Vector2 minXY = new Vector2 (-30, -10);

	// Use this for initialization
	void Start () {
		//cameraPosition = transform.position;
		if (cameraTarget == null)
			Debug.Log ("No camera target set?");
	}	
	// Update is called once per frame
	void FixedUpdate () {
		Follow ();
		//transform.position = new Vector3 (cameraTarget.position.x + offset.x,
		//	cameraTarget.position.y + offset.y, offset.z);
	}

	void Follow() {
		transform.position = new Vector3 (cameraTarget.position.x, cameraTarget.position.y+y_offset, transform.position.z);
	}
}
