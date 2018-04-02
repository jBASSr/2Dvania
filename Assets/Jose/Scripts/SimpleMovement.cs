using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleMovement : MonoBehaviour
{
	// Movement
	public float maxSpeed = 2.0f;
	public bool forward = true;
	private float forward_mult = 1.0f;
	public float speedX;
	public float speedY;
	public float cSpeedY;
	public float thrust;
	public float speed;
	public bool jumping;
	public int extraJumps = 2;
	public int jumpCount = 0;
	public float jumpSpeed = 80;
	RaycastHit2D floor;
	Vector2 gravity;
	Vector2 movement;
	Vector2 vel;

	// Player & Weapon State
	public int stance = 0;
	private const int playerStanding = 0;
	private const int playerCrouched = 1;
	private const int playerRolling = 2;
	private const int aimIdle = 0;
	private const int aimUp = 1;
	private const int aimTopRight = 2;
	private const int aimForward = 3;
	private const int aimDownRight = 4;
	private const int aimDown = 5;
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

	// Animations
	public float turnWait = 0.2f;
	public float turnTime = 0;
	public bool turning = false;
	public float transWait = 2f;
	public float transTime = 0;
	public bool transforming = false;

	private Animator anim;
	Transform pGraphics;

	// Weapons
	public float cooldown = 0.5f;
	public float cooldown_start = 0.0f;

	//FOR FIRING:
	// new
	public GameObject bulletSpawn;
	public Rigidbody2D currentWeapon;
	Rigidbody2D bullet;
	GameObject FrontCenter, TopRight, BottomRight, Top, Bottom;
	public int aimDir = 0;
	public float bulletSpeed = 400;
	// old
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
		//FrontCenter = transform.Find ("Weapon/FrontCenter").gameObject;
		//TopRight 	= transform.Find ("Weapon/TopRight").gameObject;
		//BottomRight = transform.Find ("Weapon/BottomRight").gameObject;
		//Top 		= transform.Find ("Weapon/Top").gameObject;
		//Bottom 		= transform.Find ("Weapon/Bottom").gameObject;
		// Find groundCheck transform object
		// Implement Wall/Obstacle check?
		// Implement Animations
		// Implement Weapons System
	}

	void FixedUpdate ()
	{
		//BodyState ();
		speed = Mathf.Abs(speedX);
		cSpeedY = rb.velocity.y;
		anim.SetFloat ("moveX", speedX);
		anim.SetFloat ("moveY", cSpeedY);
		anim.SetFloat ("Speed", speed);
		anim.SetInteger ("Stance", stance);
		// Animation States
		ColliderState();
		AimingState();
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
		// Fix "ramping" issue when walking up a postive ramp
		if (!turning && !hp.StunnedState) {
			if (isGrounded && !jumping && !transforming) {
				floor = Physics2D.Raycast (rb.position, -Vector2.up);
				gravity = floor.normal;
				movement = new Vector2 (gravity.y, -gravity.x);
				if (forward) {
					vel = (movement * speed) * 5.0f;
				} else {
					vel = (-movement * speed) * 5.0f;
				}
				//rb.velocity = new Vector2(vel.x, rb.velocity.y);
				rb.velocity = vel;
			} else if (isWallTrig && !isGrounded) {
				//rb.velocity = new Vector2 (speedX * maxSpeed, rb.velocity.y);
				rb.velocity = new Vector2 (speedX * 0, rb.velocity.y);
			} else {
				rb.velocity = new Vector2 (speedX * maxSpeed, rb.velocity.y);
			}
		}
		//
		Debug.DrawRay(rb.position, vel, Color.yellow);
		Debug.DrawRay(rb.position, movement, Color.green);
		Debug.DrawRay(rb.position, Physics2D.gravity * rb.mass, Color.red);
		//
		/*
		if (!turning && !hp.StunnedState) {
			if (!isWallTrig || isGrounded) {
				rb.velocity = new Vector2 (speedX * maxSpeed, rb.velocity.y);
			} else if (isWallTrig && !isGrounded) {
				rb.velocity = new Vector2 (speedX * 0, rb.velocity.y);
				//Debug.Log("Hit a wall");
			}
		}
		*/
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
		// User Inputs for Aim AND Movement
		speedX = Input.GetKey (KeyCode.A) ? -1 : Input.GetKey (KeyCode.D) ? 1 : 0;
	    speedY = Input.GetKey (KeyCode.S) ? -1 : Input.GetKey (KeyCode.W) ? 1 : 0;
		PlayerState();
		Jump ();
		if (turning) {
			turnTime += Time.deltaTime;
			if (turnTime >= turnWait) {
				turning = false;
				turnTime = 0;
			}
		}
		if (transforming) {
			transTime += Time.deltaTime;
			if (transTime >= transWait) {
				transforming = false;
				transTime = 0;
			}
		}
		if (Input.GetKey (KeyCode.F) || Input.GetMouseButton (0)) {
			anim.SetBool ("isShooting", true);
			if (Time.time > lastFire + fireRate) {
				lastFire = Time.time;
				Fire ();
                //Shoot Sound
                //FindObjectOfType<AudioManager_2>().Play("Shoot");
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
			jumping = false;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if ((isGrounded || jumpCount < extraJumps && !hp.StunnedState)) {
				anim.SetBool ("Jumped", true);
				jumping = true;
                //Jumping Sound
                FindObjectOfType<AudioManager_2>().Play("jump");
                rb.AddForce (new Vector2 (0.0f, jumpSpeed));
				if (jumpCount < extraJumps && !isGrounded)
					jumpCount++;
			}
		}
		if (Input.GetKeyUp (KeyCode.Space) && isMovingUp) {
			rb.velocity = new Vector2 (rb.velocity.x, 0f);
		}
	}

	void AimingState()
	{
		if (speedX == 0 && speedY == 0) {
			aimDir = aimIdle;
			bulletSpawn = FrontCenter;
		} else if (speedX == 0 && speedY > 0) {
			aimDir = aimUp;
			bulletSpawn = Top;
		} else if (speedX > 0 && speedY > 0) {
			aimDir = aimTopRight;
			bulletSpawn = TopRight;
		} else if (speedX > 0 && speedY == 0) {
			aimDir = aimForward;
			bulletSpawn = FrontCenter;
		} else if (speedX > 0 && speedY < 0) {
			aimDir = aimDownRight;
			bulletSpawn = BottomRight;
		} else if (speedX == 0 && speedY < 0) {
			aimDir = aimDown;
			bulletSpawn = Bottom;
		} else if (speedX < 0 && speedY == 0) {
			aimDir = aimIdle;
			bulletSpawn = FrontCenter;
		} else if (speedX < 0 && speedY < 0) {
			aimDir = aimDownRight;
			bulletSpawn = BottomRight;
		}
		// Bumpers
		if (Input.GetKey (KeyCode.Q)) {
			aimDir = aimTopRight;
			bulletSpawn = TopRight;
		} else if (Input.GetKey (KeyCode.E)) {
			aimDir = aimDownRight;
			bulletSpawn = BottomRight;
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
				transforming = true;
                //Crouch Sound
                FindObjectOfType<AudioManager_2>().Play("Crouch");

                // Standing Up
            } else if (Input.GetKeyDown (KeyCode.W) && !isCeiling) {
				stance = stance > 0 ? stance - 1 : 0;
                //Stand Sound
                FindObjectOfType<AudioManager_2>().Play("Crouch");
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
		// now with ANGLES (hopefully)! :D
		// TODO: Assign aim direction based on player inputs
		//	
		// Prepare position and rotation of the bullet based on aim state
		Vector3 bulletPos = bulletSpawn.transform.position;
		Quaternion bulletRot = bulletSpawn.transform.rotation;
		// Create bullet object
		bullet = Instantiate(currentWeapon, bulletPos, bulletRot);// as Rigidbody2D;
		// Apply players current x and y velocity
		bullet.velocity = rb.velocity;
		// TODO: 1. Aiming Direction vars
		// 		 2. Negate y velocity if aiming straight-forward
		if (aimDir == 0) {
			Vector2 newVel = bullet.velocity; //take current bullet vel
			newVel.y = 0;					  //zero y velocity
			bullet.velocity = newVel;		  //assign new velocity to bullet
		}
		//		 3. Animation bools
		// Apply actual bullet velocity to shot
		bullet.AddForce(bulletSpawn.transform.right * bulletSpeed);
		// Bullet destruction handled in 'bulletEvent.cs'
		// Check the 'MisslePrefab' if you wish to adjust these values
		/*
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
		*/
	}

	void OnTriggerEnter2D(Collider2D other) {
		isWallTrig = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		isWallTrig = false;
	}

    public void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnload;
    }

    public void OnDisable()
    {
        Tino.PlayerState.ExtraJumps = this.extraJumps;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (Tino.Save.SaveLoadGame.SaveExists && !Tino.Save.SaveLoadGame.PlayerMovementLoaded)
        {
            this.extraJumps = Tino.Save.SaveLoadGame.SavedGame.PlayerExtraJumps;
            Tino.Save.SaveLoadGame.PlayerMovementLoaded = true;
        }
        else
        {
            this.extraJumps = Tino.PlayerState.ExtraJumps;
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnload;
    }
}