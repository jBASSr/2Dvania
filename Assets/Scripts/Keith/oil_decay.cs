using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil_decay : MonoBehaviour {

	public GameObject oil_mess;
	public float speed = 3.0f;
	public float yRange = 3.0f;
	public float xRange = 30.0f;
	private Vector2 startPosition;
	private CapsuleCollider2D rob_col, oildec_col;
	private GameObject oilPrefab;
	private GameObject robot;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		robot = GameObject.Find ("Robot");
		rob_col = robot.GetComponent<CapsuleCollider2D> ();
		oildec_col = GetComponent<CapsuleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(transform.position.x + speed, startPosition.y + yRange * (Mathf.Sin(Time.time)));
		if (Mathf.Abs(transform.position.x - startPosition.x) > xRange) {
			speed *= -1.0f;
		}

		bool inx = (rob_col.bounds.min.x < oildec_col.bounds.max.x) && (rob_col.bounds.max.x > oildec_col.bounds.min.x);
		bool iny = (rob_col.bounds.min.y < oildec_col.bounds.max.y) && (rob_col.bounds.max.y > oildec_col.bounds.min.y);
		if (inx && iny){
			Debug.Log ("OIL ON PLAYER!!!!");
			if (oilPrefab == null) {
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
	}
}
