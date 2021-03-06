//
// Original Code by Keith
// Modified by Jose
//
// Purpose: Tweaking enemy flight path and trying to fix
//			issues when colliding with the player.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat_modified : MonoBehaviour {
	/*
	public float speed = 0.0f;
	public float yRange = 3.0f;
	public float xRange = 30.0f;
	private Vector2 startPosition;
	private CapsuleCollider2D rob_col;
	private BoxCollider2D bat_col;
	private GameObject robot;
	public GameObject batPrefab;
	private GameObject angryBat = null;
	public int hitCount = 2;
	private float startRocketHitTime = 0.0f;
	public float hitRocketTime = 1.0f;
	public bool isFlipped = false;
	*/
	public int hitCount = 2;
	private float startRocketHitTime = 0.0f;
	// Collision
	public LayerMask whatIsWall;
	public Transform wallCheckL;
	public Transform wallCheckR;
	public Transform floorCheck;
	public Transform ceilingCheck;
	private Rigidbody2D rb;
	private CapsuleCollider2D collider;
	public bool isLeftWall = false;
	public bool isRightWall = false;
	public bool isFloor = false;
	public bool isCeiling = false;
	public float groundRadius = 0.0001f;
	// Sprite Control
	public bool turning = false;
	public bool forward = false;
	private Animator anim;
	// Enemy Control
	public Vector3 A;
	public Vector3 B;
	public float detectRange = 2.5f;
	public LayerMask playerLayer;
	public bool playerInRange;
	public bool batMoving;
	public float amplitude = 0.1f;
	public float dropAmount = 10.0f;
	public float minY;
	public float maxY;
	public float curY;
	public bool goingDown = true;
	private float newHeight;
	public Transform flyTo;
	public float dist = 3f;
	// Calculations
	GameObject player;
	public float playerDir;
	public float speed = 1.0f;
	public bool batCD = false;
	public float batWaitTime = 0.0f;
	public Vector3 MoveDir = Vector3.forward;
	public Vector3 VertDir = Vector3.down;

	// Use this for initialization
	void Start () {
		/*
		startPosition = transform.position;
		robot = GameObject.Find ("Robot");
		rob_col = robot.GetComponent<CapsuleCollider2D> ();
		bat_col = GetComponent<BoxCollider2D> ();

		if (isFlipped) {
			transform.localRotation = Quaternion.Euler (0, 180, 0);  
		}
		*/
		A = transform.position;
		B = flyTo.position;
		player = GameObject.Find ("Robot");
		minY = transform.position.y - dropAmount;
		maxY = transform.position.y;
		anim = GetComponentInChildren<Animator> ();
		anim.SetBool ("Ceiling", true);
		anim.SetBool ("Moving", false);
	}

	// 
	void FixedUpdate() {
		curY = transform.position.y;
		isLeftWall = Physics2D.OverlapCircle(wallCheckL.position, groundRadius, whatIsWall);
		isRightWall = Physics2D.OverlapCircle(wallCheckR.position, groundRadius, whatIsWall);
		isFloor = Physics2D.OverlapCircle (floorCheck.position, groundRadius, whatIsWall);
		isCeiling = Physics2D.OverlapCircle (ceilingCheck.position, groundRadius, whatIsWall);
		playerInRange = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInRange && player && !batMoving && Time.time > batWaitTime) {
			playerDir = player.transform.position.x;
			if (playerDir < transform.position.x && !isLeftWall)
				MoveDir = new Vector3 (-1, 0, 0); // 0 means left
			else if (playerDir > transform.position.x && !isRightWall)
				MoveDir = new Vector3 (1, 0, 0);  // 1 means right
			else
				MoveDir = new Vector3 (0, 0, 0);
			batMoving = true;
		}
		if (batMoving && Time.time > batWaitTime) {
			transform.Translate (MoveDir * Time.deltaTime * speed*.5f);
			transform.Translate (VertDir * Time.deltaTime * speed*2.5f);
			anim.SetBool ("Moving", true);
            //Bat Sound
            FindObjectOfType<AudioManager_2>().Play("Bat");
            if (isFloor || curY <= minY)
				VertDir = Vector3.up;
            
        if (isCeiling && VertDir == Vector3.up) {
                VertDir = Vector3.down;
				batMoving = false;
				anim.SetBool ("Moving", false);
				anim.SetBool ("Ceiling", true);
				batWaitTime = Time.time + 1.0f;
			}
            
        }
		// Simple move from Point A to Point B with SmoothStep
		/*
		if (playerInRange || batMoving) {
			transform.position = Vector3.Lerp (A, B, Mathf.SmoothStep (
				0.0f, 1.0f, Mathf.PingPong (Time.time / dist, 1.0f))
			);
			//while (curY >= minY) {
			if (curY > minY && goingDown)
				newHeight = curY - amplitude;
			else if (curY <= minY)
				goingDown = false;
			if (curY < maxY && !goingDown)
				newHeight = curY + amplitude;
			else if (curY >= maxY)
				goingDown = true;
			transform.position = new Vector2 (transform.position.x, newHeight);
		}
		*/
	}

	void OnDrawGizmosSelected () {
		Gizmos.DrawSphere (transform.position, detectRange);
		//Gizmos.DrawCube(transform.position, new Vector3(.5f,10,1));
	}

	void Flip()
	{
		turning = true;
		transform.Rotate (Vector3.up, 180.0f, 0);
		forward = !forward;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("BAT COLLIDED WITH SOMETHING!");
		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED BAT");
			//Destroy (coll.gameObject);
			hitCount--;
			startRocketHitTime = Time.time;
			if (hitCount == 0) {
				Destroy (this.gameObject);
           
            }
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("Bullet hit bat");
			hitCount--;
			if (hitCount == 0) {
				Destroy (this.gameObject);
                FindObjectOfType<AudioManager_2>().Play("Batdies");
            }
		}
	}

}
