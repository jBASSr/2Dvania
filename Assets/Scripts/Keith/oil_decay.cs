using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class oil_decay : MonoBehaviour {

	public GameObject oil_mess;
	public GameObject oil_fall;
	public float speed = 3.0f;
	public float yRange = 3.0f;
	public float xRange = 3.0f;
	private Vector2 startPosition;
	private CapsuleCollider2D rob_col, oildec_col;

	private GameObject oilMess;
	private GameObject oilFall;
	private GameObject robot;
	private Rigidbody2D rb;

	public float fireRate = 0.5F;
	public float rocket_speed = 5.0f;
	private float lastFire = 0.0f;
	private int count_pass = 0;
	private Animator animator;
	private float startAttack = 0.0f;
	private float attackTime = 3.0f;
	private float savedSpeed = 0.0f;
	private bool isAttacking = false;
	private float yPos = 0.0f;
	private float offsetTime = 0.0f;

	public float hitCount = 10.0f;

	public float maxHealth = 10.0f;
	private float startRocketHitTime = 1.1f;
	public float hitRocketTime = 1.0f;
	private float startBulletHitTime = 1.1f;
	public float hitBulletTime = 0.5f;
	private GameObject rollability;
	public GameObject rollabilityPrefab;
	private GameObject exitDoorPrefab;

	// Use this for initialization
	void Start () {
		exitDoorPrefab = GameObject.Find ("DoorExit");
		exitDoorPrefab.SetActive (false);
		startPosition = transform.position;
		robot = GameObject.Find ("Robot");
		rob_col = robot.GetComponent<CapsuleCollider2D> ();
		oildec_col = GetComponent<CapsuleCollider2D> ();
		animator = GetComponent<Animator> ();
		savedSpeed = speed;
		animator.SetBool ("isAttack", false);
		animator.SetBool ("isDie", false);
	}
	
	// Update is called once per frame
	void Update () {
		if (oilMess != null) {
			Destroy (oilMess,0.0f);
		}
		//rb = this.GetComponent<Rigidbody2D> ();
		//rb.velocity = new Vector2 (xRange * (Mathf.Sin(Time.time)), yRange * (Mathf.Sin(Time.time)));
		if (isAttacking == false) {
			float myTime = Time.time - offsetTime;
			transform.position = new Vector2 (transform.position.x + speed, startPosition.y + yRange * (Mathf.Sin (myTime)));
		}

		if (Mathf.Abs(transform.position.x - startPosition.x) > xRange) {
			speed *= -1.0f;
			count_pass++;
		}

		if (count_pass >= 2 && (Mathf.Abs(transform.position.x - startPosition.x)<0.1)){
			animator.SetBool ("isAttack", true);
			offsetTime += attackTime;
			startAttack = Time.time;
			transform.position = new Vector2 (transform.position.x, startPosition.y + yRange);
			isAttacking = true;
			count_pass = 0;
		}
		if (Time.time > (startAttack + attackTime)) {			
			animator.SetBool ("isAttack", false);
			isAttacking = false;
		}
			

		bool inx = (rob_col.bounds.min.x < oildec_col.bounds.max.x) && (rob_col.bounds.max.x > oildec_col.bounds.min.x);
		bool iny = (rob_col.bounds.min.y < oildec_col.bounds.max.y) && (rob_col.bounds.max.y > oildec_col.bounds.min.y);
		if (inx && iny){
			Debug.Log ("OIL ON PLAYER!!!!");
			if (oilMess == null) {
				oilMess = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}

		//THIS FIRES DOWN OILS:
		if ((Time.time > lastFire + fireRate) && isAttacking == false) {
				lastFire = Time.time;
			  oilFall = (GameObject)Instantiate (
				oil_fall,
				new Vector2(transform.position.x,
					this.GetComponent<SpriteRenderer>().bounds.min.y),
				transform.rotation);
		}

		if (Time.time < startRocketHitTime + hitRocketTime) {
			if (oilMess == null) {
				Debug.Log ("Instantiating oil mess...");
				oilMess = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		if (Time.time < startBulletHitTime + hitBulletTime) {
			if (oilMess == null) {
				Debug.Log ("Instantiating oil mess...");
				oilMess = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED CEO");
			Destroy (coll.gameObject);
			hitCount -= 1.0f;
			startRocketHitTime = Time.time;
			if (hitCount <= 0.0f) {
				animator.SetBool ("isDie", true);
				Destroy (this.gameObject, 2.0f);
				if (oilMess != null) {
					Destroy (oilMess,0.0f);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("BULLET COLLIDED WITH OIL FALL!");
			Destroy (c.gameObject);
			hitCount -= 0.5f;
			startBulletHitTime = Time.time;
			if (hitCount<= 0.0f) {
				Destroy (this.gameObject, 2.0f);
				if (oilMess != null) {
					Destroy (oilMess,0.0f);
				}
				StartCoroutine (dropRollability ());
			}
		}
	}

	IEnumerator dropRollability (){
		Debug.Log ("SETTTING ROBOT COLOR???");
		yield return new WaitForSeconds(1.8f);
		exitDoorPrefab.SetActive (true);
		rollability = (GameObject)Instantiate (
			rollabilityPrefab,
			transform.position,
			transform.rotation);
		

	}
}
