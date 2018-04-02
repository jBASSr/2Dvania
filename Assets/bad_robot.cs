using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bad_robot : MonoBehaviour {

	public float xRange = 20.0f;
	public bool is_right = true;
	public float xStart = 0.0f;
	public float xLast = 0.0f;
	private Vector2 velocityNow;
	public float speed = 3.0f;
	// Use this for initialization
	void Start () {
		xStart = this.transform.position.x;
		xLast = xStart;
		if (is_right) {
			velocityNow = new Vector2 (speed, 0);
		} else {
			velocityNow = new Vector2 (-speed, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		if (Mathf.Abs (this.transform.position.x - xStart) > xRange && Mathf.Abs(this.transform.position.x - xLast)>xRange) {
			xLast = this.transform.position.x;
			is_right = !is_right;
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);
			} else {
				velocityNow = new Vector2 (-speed, 0);
			}
		}
	}
}
