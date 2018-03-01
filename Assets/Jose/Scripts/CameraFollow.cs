using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

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
	public float marginX;
	public float marginY;
	public Vector2 maxXY = new Vector2 (30, 10);
	public Vector2 minXY = new Vector2 (-30, -10);

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
		float targetX = transform.position.x;
		float targetY = transform.position.y;
		if (Mathf.Abs (transform.position.x - cameraTarget.position.x) > marginX) {
			targetX = Mathf.Lerp (transform.position.x, cameraTarget.position.x, Time.deltaTime * smoothX);
		}
		if (Mathf.Abs (transform.position.y - cameraTarget.position.y) > marginY) {
			targetY = Mathf.Lerp (transform.position.y, cameraTarget.position.y, Time.deltaTime * smoothY);
		}
		targetX = Mathf.Clamp (targetX, minXY.x, maxXY.x);
		targetY = Mathf.Clamp (targetY, minXY.y, maxXY.y);
		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}
}
