using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

	public float maxSpeed = 7;
	public float jumpForce = 7;

	private float direction;
	private float _posX;

	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Awake () 
	{
		//spriteRenderer = GetComponent<SpriteRenderer> (); 
		//animator = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		direction = transform.localScale.x;
		_posX = transform.position.x;
	}

	protected override void ComputeVelocity()
	{
		//Vector3 move = Vector3.zero;
		Vector2 move = Vector2.zero;
		move = rigidBody.velocity;
		move.x = Input.GetAxis ("Horizontal");

		if (Input.GetButtonDown ("Jump") && grounded) {
			velocity.y = jumpForce;
		} else if (Input.GetButtonUp ("Jump")) 
		{
			if (velocity.y > 0) {
				velocity.y = velocity.y * 0.5f;
			}
		}
		// Flip gameObject based on moving direction and state of Scale X
		if (move.x < 0 && direction == 1) {
			transform.localScale = new Vector2 (-direction, transform.localScale.y);
		}
		if (move.x > 0 && direction == -1) {
			transform.localScale = new Vector2 (-direction, transform.localScale.y);
		}
		direction = transform.localScale.x;
		// animator.SetBool ("grounded", grounded);
		// animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

		targetVelocity = move * maxSpeed;
	}
}