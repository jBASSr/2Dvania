using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	public float maxHealth = 10.0f;
	private GameObject myBadRobot;
	private bool isOutOfRange = false;

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
	/*
	void FixedUpdate(){
		//OUT OF RANGE:
		if (Mathf.Abs (this.transform.position.x - xStart) > xRange && Mathf.Abs (this.transform.position.x - xLast) > xRange) {
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

	}
*/
	void Update () {

		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		float diff = this.transform.position.x - robot.transform.position.x;
		//if (Mathf.Abs (this.transform.position.x - xStart) > xRange && Mathf.Abs (this.transform.position.x - xLast) > xRange) {
	//		xLast = this.transform.position.x;
	//		is_right = !is_right;
	//	}
		// TO DO IN RANGE JUMP TO ROBOT POSTION AND FIRE AI:
		if (isGrounded == true) {			
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);
				animator.SetBool ("is_right", true);
			} else {
				velocityNow = new Vector2 (-speed, 0);
				animator.SetBool ("is_right", false);
			}			
			if ((Mathf.Abs (diff) < attackRange)) {
				animator.SetBool ("is_jump", true);
				velocityNow = new Vector2 (jumpXFactor * -diff, 0);
				if (velocityNow.x < 0) {
					is_right = false;
				} else {
					is_right = true;
				}
				rb.AddForce (new Vector2 (0.0f, jumpForce));
				isGrounded = false;
			} else {
				animator.SetBool ("is_jump", false);
				if (is_right) {
					velocityNow = new Vector2 (speed, 0);
					animator.SetBool ("is_right", true);
				} else {
					velocityNow = new Vector2 (-speed, 0);
					animator.SetBool ("is_right", false);
				}
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
		if (coll.gameObject.tag == "Column") {
			Debug.Log("BAD ROBOT HIT COLUMN!");
			is_right = !is_right;
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);
				animator.SetBool ("is_right", true);
			} else {
				velocityNow = new Vector2 (-speed, 0);
				animator.SetBool ("is_right", false);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("Bullet COLLIDED WITH BAD ROBOT!");
			StartCoroutine(hitRobot());
			Destroy (c.gameObject);
		}
	}
	public IEnumerator loadCredits (){
		Debug.LogError ("loadCredets IEnumberator called!!");
		yield return new WaitForSeconds(2);
		Debug.LogError ("CALLING Application.LoadLevel Credits ????!!!!!!!!!!!!");
		//Application.LoadLevel("Credits");
		FindObjectOfType<AudioManager_2>().Play("Win");
		SceneManager.LoadScene("Credits", LoadSceneMode.Single);
	}

	IEnumerator hitRobot (){
		Debug.Log ("SETTTING ROBOT COLOR???");
		gameObject.GetComponent<SpriteRenderer> ().material.SetColor ("_Color", Color.blue);
		hitCount -= 0.5f;
		if (hitCount == 0) {
			animator.SetBool ("is_die", true);
			FindObjectOfType<AudioManager_2> ().Play ("explode");
			yield return new WaitForSeconds (1.2f);
			Destroy (this.gameObject);
			//yield return new WaitForSeconds (2.0f);
			//yield return new WaitForSeconds (0.2f);
			SceneManager.LoadScene("Credits", LoadSceneMode.Single);
		} else {
			yield return new WaitForSeconds (0.2f);
			gameObject.GetComponent<SpriteRenderer> ().material.SetColor ("_Color", Color.white);
		}
	}


	void Fire(){
		//lightIntensity = Random.Range(minInt, maxInt);
		FindObjectOfType<AudioManager_2> ().Play ("Missile");
		var fireballLeft = (GameObject)Instantiate (
			fireballLeftPrefab,
			fireballLeftEmitter.transform.position,
			fireballLeftEmitter.transform.rotation);
		fireballLeft.GetComponent<Rigidbody2D> ().velocity = new Vector2 (rb.velocity.x, -fireball_speed);
		var fireballRight = (GameObject)Instantiate (
			fireballRightPrefab,
			fireballRightEmitter.transform.position,
			fireballRightEmitter.transform.rotation);
		fireballRight.GetComponent<Rigidbody2D> ().velocity = new Vector2 (rb.velocity.x, -fireball_speed);

	}
}
