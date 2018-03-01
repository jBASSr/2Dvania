using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAhead : MonoBehaviour {
	private SimpleMovement charSpeed;
	private Transform parentTrans;
	public float LookAheadValue = 1f;
	// Use this for initialization
	void Awake () {
		// Refs
		charSpeed = GameObject.Find("Robot").GetComponent<SimpleMovement>();
		parentTrans = transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
		// Set "Look Ahead"
		transform.position = new Vector3 (parentTrans.position.x + charSpeed.speedX * LookAheadValue,
			transform.position.y, transform.position.z);
	}
}
