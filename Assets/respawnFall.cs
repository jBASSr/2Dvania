using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnFall : MonoBehaviour {

	private SimpleMovement sm;

	public float fallY = 0.0f;
	private Vector2 lastPosition;
	// Use this for initialization
	void Start () {
		//Debug.Log ("RESPAWN FALL SCRIPT STARTED!!!!!");
		lastPosition = new Vector2(transform.position.x, transform.position.y);
		sm = this.GetComponent<SimpleMovement> ();
	}
	
	// Update is called once per frame
	void Update () {		
		if (sm.isGrounded) {
			//Debug.Log ("PLAYER IS GROUNDED, SAVING POSITION!!!!!");
			lastPosition = transform.position;
		}
		if (transform.position.y < fallY) {
			if (lastPosition.x < transform.position.x) {
				lastPosition.x -= 0.1f;
			} else {
				lastPosition.x += 0.1f;
			}
			Debug.Log ("PLAYER FELL DOWN!!!!!");
			transform.position = lastPosition;
		}
	}
}
