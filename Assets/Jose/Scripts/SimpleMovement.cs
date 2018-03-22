using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
	// Movement
	public float maxSpeed = 2.0f;
	public bool forward = true;
	private float forward_mult = 1.0f;
	public float speedX;
	public float speedY;
	public float thrust;
	public float speed;
	public int extraJumps = 2;
	public int jumpCount = 0;
	public float jumpSpeed = 80;

	// Player State
	public int stance = 0;
	private const int playerStanding = 0;
	private const int playerCrouched = 1;
	private const int playerRolling = 2;
	private const float standHeight = 2f;
	private const float crouchHeight = 1.5f;
	private const float rollHeight = 0.7f;

	// Objects, Masks & Physics
	private Rigidbody2D rb;
	private CapsuleCollider2D collider;
	public Transform groundCheck;
	public Transform wallCheck;
	public Transform ceilingCheck;
	public LayerMask whatIsGround;
	public LayerMask whatIsWall;
	public LayerMask whatIsDoor;
	public bool isGrounded = false;
	public bool isMovingUp = false;
	public bool isWall = false;
	public bool isWallTrig = false;
	public bool isCeiling = false;
	public float groundRadius = 0.0001f;

	// GameObjects & Prefabs
	private GameObject bullet;
	Rigidbody clone;
	public int aim = 0;

	// Animations
	public float turnWait = 0.2f;
	public float turnTime = 0;
	public bool turning = false;

	private Animator anim;
	Transform pGraphics;

	// Weapons
	private float bulletSpeed = 100;
	public float cooldown = 0.5f;
	public float cooldown_start = 0.0f;

	//FOR FIRING:
	public GameObject rocketPrefab;
	public float fireRate = 0.5F;
	public float rocket_speed = 5.0f;
	private float lastFire = 0.0f;

	// External Stuff
	HealthSystem hp;

	void Awake ()
	{
		collider = GetComponent<CapsuleCollider2D> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		pGraphics = transform.Find ("Graphics");
		hp = GetComponent<HealthSystem> ();
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
		anim.SetInteger ("Stance", stance);
		// Animation States
		ColliderState();
		isGrounded = Physics2D.OverlapCircle(groundCheck.position,
			groundRadius,
			whatIsGround);
		isCeiling = Physics2D.OverlapCircle (ceilingCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("isGrounded", isGrounded);
		// Reset Jump Count
		if (isGrounded) {
			jumpCount = 0;
		}
		// Sprite Orientation
		if ((speedX > 0.0f && !forward) || (speedX < 0.0f && forward)) {
			Flip ();
		}
		// Implement Wall Slide/Hold (aka prevent played being stuck to wall)
		isWall = Physics2D.OverlapCircle(wallCheck.position, groundRadius, whatIsWall);
		if (!turning && !hp.StunnedState) {
			if (!isWallTrig || isGrounded) {
				rb.velocity = new Vector2 (speedX * maxSpeed, rb.velocity.y);
			} else if (isWallTrig && !isGrounded) {
				rb.velocity = new Vector2 (speedX * 0, rb.velocity.y);
				//Debug.Log("Hit a wall");
			}
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
		PlayerState();
		Jump ();
		if (turning) {
			turnTime += Time.deltaTime;
			if (turnTime >= turnWait) {
				turning = false;
				turnTime = 0;
			}
		}
		if (Input.GetKey (KeyCode.F)) {
			anim.SetBool ("isShooting", true);
			if (Time.time > lastFire + fireRate) {
				lastFire = Time.time;
				Fire ();
			}
		} else {
			anim.SetBool ("isShooting", false);
		}
	}

	void Jump()
	{
		if (rb.velocity.y >= 0.5f) {
			isMovingUp = true;
		} else {
			isMovingUp = false;
			anim.SetBool ("Jumped", false);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if ((isGrounded || jumpCount < extraJumps && !hp.StunnedState)) {
				anim.SetBool ("Jumped", true);
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

	void PlayerState()
	{
		if (isGrounded) {
			// Crouch
			if (Input.GetKeyDown (KeyCode.S) && speed == 0) {
				stance = stance < 2 ? stance + 1 : 2;
			// Standing Up
			} else if (Input.GetKeyDown (KeyCode.W) && !isCeiling) {
				stance = stance > 0 ? stance - 1 : 0;
			}
			if (speed != 0 && stance == playerCrouched) {
				stance = playerStanding;
			}
		} else {
			
		}
	}

	void ColliderState()
	{
		if (stance == playerStanding && collider.size.y != standHeight) {
			collider.size = new Vector2(collider.size.x, standHeight);
		} else if (stance == playerCrouched && collider.size.y != crouchHeight) {
			collider.size = new Vector2(collider.size.x, crouchHeight);
		} else if (stance == playerRolling && collider.size.y != rollHeight) {
			collider.size = new Vector2(collider.size.x, rollHeight);
		}
	}

  	/*
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
	*/ 
	void Fire()
	{
		if (forward) {
			forward_mult = 1.0f;
		} else {
			forward_mult = -1.0f;
		}
		var rocket = (GameObject)Instantiate (
			rocketPrefab,
			new Vector2((float)(transform.position.x + forward_mult*1.02), 
				(float)(transform.position.y + 0.29)),
				transform.rotation);
		//Debug.Log ("rocket velocity=" + speedX + forward_mult * rocket_speed);
		rocket.GetComponent<Rigidbody2D> ().velocity = new Vector2 (forward_mult * rocket_speed, 0);
		if (rocket != null) {
			Destroy (rocket, 10.0f);
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		isWallTrig = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		isWallTrig = false;
	}
}