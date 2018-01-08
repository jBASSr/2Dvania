using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
	// Movement
	public float maxSpeed = 2.0f;
	public bool forward = true;
	public float speedX;
	public float speedY;
	public float thrust;
	private float speed;

	// Jumping
	public int extraJumps = 5;
	public int jumpCount = 0;
	public float jumpSpeed = 70;

	// Objects & Physics
	private Rigidbody2D rb;
	private CapsuleCollider2D collider;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public bool isGrounded = false;
	public bool isMovingUp = false;
	public float groundRadius = 0.0001f;

	// Animations
	public float turnWait = 0.2f;
	public float turnTime = 0;
	public bool turning = false;

	private Animator anim;
	Transform pGraphics;

	void Awake ()
	{
		collider = GetComponent<CapsuleCollider2D> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		pGraphics = transform.Find ("Graphics");
		//rb.sleepThreshold = 0.0f;

		// Find groundCheck transform object
		// Implement Wall/Obstacle check?
		// Implement Animations
		// Implement Weapons System
	}

	void FixedUpdate ()
	{
		//BodyState ();
		speed = Mathf.Abs(speedX);
		speedY = rb.velocity.y;
		anim.SetFloat ("moveX", speedX);
		anim.SetFloat ("moveY", speedY);
		anim.SetFloat ("Speed", speed);

		isGrounded = Physics2D.OverlapCircle(groundCheck.position,
			groundRadius,
			whatIsGround);
		anim.SetBool ("isGrounded", isGrounded);
		if (isGrounded) {
			jumpCount = 0;
		}
		if ((speedX > 0.0f && !forward) || (speedX < 0.0f && forward)) {
			Flip ();
		}
		if (!turning) {
			rb.velocity = new Vector2 (speedX * maxSpeed, rb.velocity.y);
		}
	}

	void Update()
	{
		/*
		if ((Input.GetAxis ("Horizontal")) > 0) {
			speedX = 1;
			Debug.Log ("Right");
		} else if ( (Input.GetAxis("Horizontal")) < 0) {
			speedX = -1;
			Debug.Log("Left");
		}
		*/
		speedX = Input.GetKey (KeyCode.A) ? -1 : Input.GetKey (KeyCode.D) ? 1 : 0;
		// speedY = Input.GetKey (KeyCode.S) ? -1 : Input.GetKey (KeyCode.W) ? 1 : 0;
		Jump ();
		if (turning) {
			turnTime += Time.deltaTime;
			if (turnTime >= turnWait) {
				turning = false;
				turnTime = 0;
			}
		}
	}

	void Jump()
	{
		if (rb.velocity.y >= 0.5f)
			isMovingUp = true;
		else
			isMovingUp = false;

		if (Input.GetKeyDown (KeyCode.Space)) {
			if ((isGrounded || jumpCount < extraJumps)) {
				rb.AddForce (new Vector2 (0.0f, jumpSpeed));
				if (jumpCount < extraJumps && !isGrounded)
					jumpCount++;
			}
		}
		if (Input.GetKeyUp (KeyCode.Space) && isMovingUp) {
			rb.velocity = new Vector2 (rb.velocity.x, 0f);
		}
	}

	void Flip()
	{
		turning = true;
		transform.Rotate (Vector3.up, 180.0f, 0);
		forward = !forward;
		/*
		if (speedX < 0 && forward)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
		if (speedX > 0 && !forward)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
		*/
	}

	void OnCollisionEnter2D (Collision2D c)
	{
		//Debug.Log ("COLLIDED?");
		if (c.gameObject.tag == "Enemy") {
			// rigidBody.AddForce(Vector3.left * 100);
			//rb.AddForce(transform.right * -thrust);
			rb.AddForce (transform.up * (thrust));
			rb.AddForce (transform.right * -(thrust));
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y);
			// isGrounded = false;
			// velocity.y += knockbackY;
			// velocity.x += knockbackX;
		}
	}


}