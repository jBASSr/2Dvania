﻿using System.Collections;
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
	bool landing = false;
	float landTime = 0f;
	public int extraJumps = 2;
	public int jumpCount = 0;
	public float jumpSpeed = 80;
	public float boostSpeed = 150;
	RaycastHit2D floor;
	Vector2 gravity;
	Vector2 movement;
	Vector2 vel;

	// Controller Support
	float hAxis, vAxis; // Left-Stick 
	float lTrigger, rTrigger; // Triggers
	float dhAxis, dvAxis; // D-Pad (someone is bound to prefer it, i'll add it soon)

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
	public float transWait = 0.5f;
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
	Rigidbody2D baseWeapon;
	Rigidbody2D laserWeapon;
	Rigidbody2D missileWeapon;
	Rigidbody2D bullet;
	GameObject FrontCenter, TopRight, BottomRight, Top, Bottom;
	public int aimDir = 0;
    public int equippedWeapon; //0: Base Weapon, 1: Missile Weapon
	bool swapping;
	bool isShooting;
	bool shot;
	bool aiming; // Actively pressing LB
	float swapTime = 0f;
	float aimTime = 0f;
	float aimWait = 1f;
	public float bulletSpeed = 400;
	// old
	public GameObject rocketPrefab;
	public float fireRate = 0.5F;
	public float rocket_speed = 5.0f;
	private float lastFire = 0.0f;

    private bool HasFreezeWeapon { get; set; }
    public int MissileAmmo { get; private set; }

	// External Stuff
	HealthSystem hp;

	void Awake ()
	{
		collider = GetComponent<CapsuleCollider2D> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		pGraphics = transform.Find ("Graphics");
		hp = GetComponent<HealthSystem> ();
		FrontCenter = transform.Find ("Weapon/FrontCenter").gameObject;
		TopRight 	= transform.Find ("Weapon/TopRight").gameObject;
		BottomRight = transform.Find ("Weapon/BottomRight").gameObject;
		Top 		= transform.Find ("Weapon/Top").gameObject;
		Bottom 		= transform.Find ("Weapon/Bottom").gameObject;
		missileWeapon = Resources.Load ("Bullet_Missile_Prefab", typeof(Rigidbody2D)) as Rigidbody2D;
		baseWeapon = Resources.Load ("Bullet_Base_Prefab", typeof(Rigidbody2D)) as Rigidbody2D;
		laserWeapon = Resources.Load ("Bullet_Laser_Prefab", typeof(Rigidbody2D)) as Rigidbody2D;
		equippedWeapon = 0;
		swapping = false;
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
		if (isGrounded && !isMovingUp && !jumping) {
			jumpCount = 0;
		}
		// Sprite Orientation
		if ((speedX > 0.0f && !forward) || (speedX < 0.0f && forward)) {
			Flip();
		}
		// Implement Wall Slide/Hold (aka prevent played being stuck to wall)
		isWall = Physics2D.OverlapCircle(wallCheck.position, groundRadius, whatIsWall);
		// Fix "ramping" issue when walking up a postive ramp
		if (!turning && !hp.StunnedState) {
			if (isGrounded && !jumping && !transforming && !landing) {
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
			} else if (isWall && !isGrounded) {
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
		//speedX = Input.GetKey (KeyCode.A) ? -1 : Input.GetKey (KeyCode.D) ? 1 : 0;
		hAxis = Input.GetAxis("Horizontal");
		vAxis = Input.GetAxis("Vertical");
		if (hAxis >= 0.5f) { speedX = 1; } else if (hAxis <= -0.5f) { speedX = -1; } else { speedX = 0; }
		if (vAxis >= 0.5f) { speedY = 1; } else if (vAxis <= -0.5f) { speedY = -1; } else { speedY = 0; }
	    //speedY = Input.GetKey (KeyCode.S) ? -1 : Input.GetKey (KeyCode.W) ? 1 : 0;
		lTrigger = Input.GetAxis("xboxLT");
		rTrigger = Input.GetAxis("xboxRT");
		PlayerState();
		Jump();
		// Move to function eventually

		if (Mathf.Abs (speedX) > 0.1f && isGrounded && stance == 0 && hp.health > 0.0) {
			GetComponent<AudioSource> ().UnPause ();
		} else {
			GetComponent<AudioSource> ().Pause ();
		}

		//
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
		if (swapping) {
			swapTime += Time.deltaTime;
			if (swapTime >= 0.5f) {
				swapping = false;
				swapTime = 0;
			}
		}
		if (landing) {
			landTime += Time.deltaTime;
			if (landTime >= 0.35f) {
				landing = false;
				landTime = 0;
			}
		}
		if (shot) {
			aimTime += Time.deltaTime;
			if (aimTime >= aimWait) {
				shot = false;
				anim.SetBool ("isShooting", false);
				aimTime = 0;
			}
		}

        if ((Input.GetKey(KeyCode.R) || Input.GetButtonDown("xboxRB")) && !swapping)
        {
			swapping = true;
			SwapWeapons (equippedWeapon);
		}
		if ((Input.GetKey (KeyCode.F) || Input.GetMouseButton (0) || Input.GetButtonDown("xboxY")) && stance < 1) {//rTrigger >= 0.01 && stance < 1) {
			anim.SetBool ("isShooting", true);
			//isShooting = true;
			shot = true;
			aimTime = 0;
			// Regular Bullets
			if (equippedWeapon == 0) {
				if (Time.time > lastFire + fireRate) {
					lastFire = Time.time;
					if (Fire ()) {
						//Shoot Sound
						FindObjectOfType<AudioManager_2> ().Play ("Shoot");
					}
				}
				// Missiles
			} else if (equippedWeapon == 1) {
				if (Time.time > lastFire + (fireRate * 4)) {
					lastFire = Time.time;
					if (Fire ()) {
						//Shoot Sound
						FindObjectOfType<AudioManager_2> ().Play ("Shoot");
					}
				}
				// Laser
			} else if (equippedWeapon == 2) {
				if (Time.time > lastFire + (fireRate * 4)) {
					lastFire = Time.time;
					if (Fire ()) {
						//Shoot Sound
						FindObjectOfType<AudioManager_2> ().Play ("Shoot");
					}
				}
			}
		} else {
			//anim.SetBool ("isShooting", false);
			//isShooting = false;
		}
	}

	void Jump()
	{		
		if (rb.velocity.y >= 0.5f) {
			isMovingUp = true;
		} else if (rb.velocity.y >= -1f && rb.velocity.y < 0f) {
			isMovingUp = false;
			anim.SetBool ("Jumped", false);
			anim.SetBool ("Boosted", false);
			jumping = false;
			/*
			if (isGrounded && !landing) {
				Debug.Log ("Landed!!!!!");
				landing = true;
				collider.size = new Vector2(collider.size.x, crouchHeight);
			}
			*/
		}

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("xboxB")) {
			if ((isGrounded || jumpCount < extraJumps) && !hp.StunnedState) {
				anim.SetBool ("Jumped", true);
				jumping = true;
                //Jumping Sound
				// UNCOMMENT ONCE SOUND IS ADDED TO "JR_LEVEL_01"
                FindObjectOfType<AudioManager_2>().Play("jump");
                rb.AddForce (new Vector2 (0.0f, jumpSpeed));
				jumpCount++;
			} else if (jumpCount == 1 && extraJumps == 1 && !isGrounded && !hp.StunnedState) {
				anim.SetBool ("Boosted", true);
                jumpCount++;
                FindObjectOfType<AudioManager_2>().Play("Boost");
				rb.velocity = new Vector2 (0f, 0f);
				rb.AddForce (new Vector2 (0.0f, boostSpeed*2));
			}
		}
		if (Input.GetKeyUp (KeyCode.Space) || Input.GetButtonDown("xboxB") && isMovingUp) {
			rb.velocity = new Vector2 (rb.velocity.x, 0f);
		}
	}

	void AimingState()
	{
		if (Input.GetButton("xboxLB")) {
			aiming = true;
			//Debug.Log ("Left Bumper Hold");
			if (speedY < 0) {
				aimDir = aimDownRight;
				bulletSpawn = BottomRight;
				anim.SetInteger ("AimDir", 3);
			} else {
				aimDir = aimTopRight;
				bulletSpawn = TopRight;
				anim.SetInteger ("AimDir", 1);
			}
		} else {
			aiming = false;
			if (speedX == 0 && speedY == 0) {
				aimDir = aimIdle;
				bulletSpawn = FrontCenter;
				anim.SetInteger ("AimDir", 2);
			} else if (speedX == 0 && speedY > 0) {
				aimDir = aimUp;
				bulletSpawn = Top;
				anim.SetInteger ("AimDir", 0);
			} else if (speedX > 0 && speedY > 0) {
				aimDir = aimTopRight;
				bulletSpawn = TopRight;
				anim.SetInteger ("AimDir", 1);
			} else if (speedX > 0 && speedY == 0) {
				aimDir = aimForward;
				bulletSpawn = FrontCenter;
				anim.SetInteger ("AimDir", 2);
			} else if (speedX > 0 && speedY < 0) {
				aimDir = aimDownRight;
				bulletSpawn = BottomRight;
				anim.SetInteger ("AimDir", 3);
			} else if (speedX == 0 && speedY < 0) {
				aimDir = aimDown;
				bulletSpawn = Bottom;
				anim.SetInteger ("AimDir", 4);
			} else if (speedX < 0 && speedY == 0) {
				aimDir = aimIdle;
				bulletSpawn = FrontCenter;
				anim.SetInteger ("AimDir", 2);
			} else if (speedX < 0 && speedY < 0) {
				aimDir = aimDownRight;
				bulletSpawn = BottomRight;
				anim.SetInteger ("AimDir", 3);
			} else if (speedX < 0 && speedY > 0) {
				aimDir = aimTopRight;
				bulletSpawn = TopRight;
				anim.SetInteger ("AimDir", 1);
			}
		}
		// Bumpers
		if (Input.GetButtonDown("xboxLB") && speedY > 0) {
			aimDir = aimTopRight;
			bulletSpawn = TopRight;
		} else if (Input.GetButtonDown("xboxLB") && speedY < 0) {
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
			if (Input.GetKeyDown (KeyCode.S) || vAxis == -1 && speed == 0 && stance < 2 && !transforming && !isShooting && !aiming) {
				stance = stance < 2 ? stance + 1 : 2;
				transforming = true;
                //Crouch Sound
                FindObjectOfType<AudioManager_2>().Play("Crouch");

                // Standing Up
			} else if (Input.GetKeyDown (KeyCode.W) || vAxis == 1 && !isCeiling && stance > 0 && !transforming) {
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
		if (stance == playerStanding && collider.size.y != standHeight && !landing) {
			collider.size = new Vector2(collider.size.x, standHeight);
			groundCheck.position = new Vector3 (rb.position.x, rb.position.y-1.15f, 0f);
		} else if (stance == playerCrouched && collider.size.y != crouchHeight) {
			collider.size = new Vector2(collider.size.x, crouchHeight);
			groundCheck.position = new Vector3 (rb.position.x, rb.position.y-0.75f, 0f);
		} else if (stance == playerRolling && collider.size.y != rollHeight) {
			collider.size = new Vector2(collider.size.x, rollHeight);
			groundCheck.position = new Vector3 (rb.position.x, rb.position.y-0.35f, 0f);
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
    bool Fire()
	{
		// now with ANGLES (hopefully)! :D
		// TODO: Assign aim direction based on player inputs
		//	
		// Prepare position and rotation of the bullet based on aim state
		Vector3 bulletPos = bulletSpawn.transform.position;
		Quaternion bulletRot = bulletSpawn.transform.rotation;
		// Create bullet object

        if(this.equippedWeapon == 1)
        {
            if(this.MissileAmmo <= 0) { return false; } //Out of ammo. No missile to fire.
            this.MissileAmmo--;
        }
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
        return true; //Bullet was fired
	}
    void SwapWeapons(int n)
    {
        switch (n)
        {
            case 0:
                if(this.MissileAmmo > 0)
                {
                    equippedWeapon = 1;
					currentWeapon = missileWeapon;
                }
                else if(this.HasFreezeWeapon)
                {
                    equippedWeapon = 2;
                    currentWeapon = laserWeapon; //No freeze weapon yet. When we have one we can change this.
                }
                break;
            case 1:
                if(this.HasFreezeWeapon)
                {
                    equippedWeapon = 2;
                    currentWeapon = laserWeapon; //No freeze weapon yet. When we have one we can change this.
                }
                else
                {
			equippedWeapon = 0;
			currentWeapon = baseWeapon;
		}
                break;
            case 2:
                equippedWeapon = 0;
                currentWeapon = baseWeapon;
                break;
        }
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
		Tino.PlayerState.MissileAmmo = this.MissileAmmo;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (Tino.Save.SaveLoadGame.SaveExists && !Tino.Save.SaveLoadGame.PlayerMovementLoaded)
        {
            this.extraJumps = Tino.Save.SaveLoadGame.SavedGame.PlayerExtraJumps;
			this.MissileAmmo = Tino.Save.SaveLoadGame.SavedGame.PlayerMissileAmmo;
            Tino.Save.SaveLoadGame.PlayerMovementLoaded = true;
        }
        else
        {
            this.extraJumps = Tino.PlayerState.ExtraJumps;
			this.MissileAmmo = Tino.PlayerState.MissileAmmo;
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    public bool AddMissileAmmo(int amount)
    {
        this.MissileAmmo += 5;
        return true;
    }

    public bool AcquireFreezeWeapon()
    {
        if(!this.HasFreezeWeapon)
        {
            this.HasFreezeWeapon = true;
            return true;
        }
        return false;
    }
}