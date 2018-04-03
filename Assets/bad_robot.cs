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
	public float jumpXFactor = 4.0f;
	private Animator animator;
	public float attackRange = 5.0f;
	private GameObject robot;
	private Rigidbody2D rb;
	public float jumpForce = 20.0f;
	private bool isGrounded;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		robot = GameObject.Find ("Robot");
		isGrounded = true;
		xStart = this.transform.position.x;
		xLast = xStart;
		if (is_right) {
			velocityNow = new Vector2 (speed, 0);
			animator.SetBool ("is_right", true);
		} else {
			velocityNow = new Vector2 (-speed, 0);
			animator.SetBool ("is_right", false);
		}
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
			this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		if (Mathf.Abs (this.transform.position.x - xStart) > xRange && Mathf.Abs(this.transform.position.x - xLast)>xRange) {
			xLast = this.transform.position.x;
			is_right = !is_right;
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);
				animator.SetBool ("is_right", true);
			} else {
				velocityNow = new Vector2 (-speed, 0);
				animator.SetBool ("is_right", false);
			}
		}
		// TO DO IN RANGE JUMP TO ROBOT POSTION AND FIRE AI:
		float diff = this.transform.position.x - robot.transform.position.x;
		if ((Mathf.Abs (diff) < attackRange) && isGrounded == true) {
			animator.SetBool ("is_jump", true);
			if (diff > 0) {//JUMP LEFT
				velocityNow = new Vector2 (jumpXFactor*-diff,0);
			} else {//JUMP RIGHT:
				velocityNow = new Vector2 (jumpXFactor*-diff,0);
			}
			rb.AddForce (new Vector2 (0.0f, jumpForce));
			isGrounded = false;
		} else {
			animator.SetBool ("is_jump", false);
		}
		if (isGrounded) {
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);
				animator.SetBool ("is_right", true);
			} else {
				velocityNow = new Vector2 (-speed, 0);
				animator.SetBool ("is_right", false);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground") {
			Debug.Log("BAD ROBOT FELL TO GROUND!");
			isGrounded = true;
		}
	}
}
