using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bad_robot : MonoBehaviour {

	public float xRange = 20.0f;
	public bool is_right = true;

	private float xStart = 0.0f;
	private float xLast = 0.0f;

	private Vector2 velocityNow;

	public float speed = 3.0f;
	public float jumpXFactor = 4.0f;

	private Animator animator;

	public float attackRange = 5.0f;

	private GameObject robot;
	private Rigidbody2D rb;

	public float jumpForce = 20.0f;

	private bool isGrounded;
	private GameObject fireballLeft;
	private GameObject fireballRight;

	public GameObject fireballLeftEmitter;
	public GameObject fireballRightEmitter;
	public GameObject fireballLeftPrefab;
	public GameObject fireballRightPrefab;

	// Use this for initialization
	public float fireRate = 1.0F;
	public float fireball_speed = 5.0f;
	private float lastFire = 0.0f;

	private float startAttack = 0.0f;
	private float attackTime = 3.0f;

	private bool isAttacking = false;
	private float yPos = 0.0f;
	private float offsetTime = 0.0f;
	public float hitCount = 10.0f;
	private Color old_color;


	void Start () {
		old_color = gameObject.GetComponent<SpriteRenderer> ().color;
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
			//isAttacking = true;
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
			//NOW FIRE!!!!:
			if (Time.time > lastFire + fireRate) {
				lastFire = Time.time;
				Fire ();
			}
		}

	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("Bullet COLLIDED WITH BAD ROBOT!");
			Destroy (c.gameObject);
			hitCount -= 0.5f;
			if (hitCount <= 0) {
				Destroy (this.gameObject);
			}

			//animator.SetBool ("is_shocked", true);
			StartCoroutine(hitRobot());
			//animator.SetBool ("is_shocked", false);
		}
	}

	IEnumerator hitRobot (){
		gameObject.GetComponent<SpriteRenderer>().color=new Color(0, 0, 1, 1);
		yield return new WaitForSeconds(0.2f);
		gameObject.GetComponent<SpriteRenderer>().color=old_color;
	}


	void Fire(){
		var fireballLeft = (GameObject)Instantiate (
			fireballLeftPrefab,
			fireballLeftEmitter.transform.position,
			fireballLeftEmitter.transform.rotation);
		fireballLeft.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -fireball_speed);
		var fireballRight = (GameObject)Instantiate (
			fireballRightPrefab,
			fireballRightEmitter.transform.position,
			fireballRightEmitter.transform.rotation);
		fireballRight.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -fireball_speed);

	}
}
