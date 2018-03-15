using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		robot = GameObject.Find ("Robot");
		rob_col = robot.GetComponent<CapsuleCollider2D> ();
		oildec_col = GetComponent<CapsuleCollider2D> ();
		animator = GetComponent<Animator> ();
		savedSpeed = speed;
		animator.SetBool ("isAttack", false);
	}
	
	// Update is called once per frame
	void Update () {
		if (oilMess != null) {
			Destroy (oilMess,0.0f);
		}
		//rb = this.GetComponent<Rigidbody2D> ();
		//rb.velocity = new Vector2 (xRange * (Mathf.Sin(Time.time)), yRange * (Mathf.Sin(Time.time)));
		if (isAttacking == false) {
			transform.position = new Vector2 (transform.position.x + speed, startPosition.y + yRange * (Mathf.Sin (Time.time)));
		}

		if (Mathf.Abs(transform.position.x - startPosition.x) > xRange) {
			speed *= -1.0f;
			count_pass++;
		}

		if (count_pass >= 3 && (Mathf.Abs(transform.position.x - startPosition.x)<0.2)){
			animator.SetBool ("isAttack", true);
			startAttack = Time.time;
			startPosition = transform.position;
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
			if (Time.time > lastFire + fireRate) {
				lastFire = Time.time;
			  oilFall = (GameObject)Instantiate (
				oil_fall,
				new Vector2(transform.position.x,
					this.GetComponent<SpriteRenderer>().bounds.min.y),
				transform.rotation);
			}
	}
}
