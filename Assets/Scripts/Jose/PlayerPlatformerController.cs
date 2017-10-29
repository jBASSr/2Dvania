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
	private ceo myCeo;
	private bool is_move_right=false;
	private bool is_collide=false;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

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
		//if (is_collide == false) {
			//Vector3 move = Vector3.zero;
			Vector2 move = Vector2.zero;
			move = rigidBody.velocity;


			if (Input.GetKeyUp (KeyCode.X)) {
				Debug.Log ("X PRESSED!");
				Fire ();
			}

			if (Input.GetButtonDown ("Jump") && grounded) {
				velocity.y = jumpForce;
			} else if (Input.GetButtonUp ("Jump")) {
				if (velocity.y > 0) {
					velocity.y = velocity.y * 0.5f;
				}
			}

			move.x = Input.GetAxis ("Horizontal");
			// Flip gameObject based on moving direction and state of Scale X
			if (move.x < 0) {
				transform.localRotation = Quaternion.Euler (0, 180, 0);
				/*transform.localScale = new Vector2 (-direction, transform.localScale.y);
			direction = -direction;
             */
				is_move_right = true;
			}
			else if (move.x > 0){
				transform.localRotation = Quaternion.Euler (0, 0, 0);
				//transform.localScale = new Vector2 (direction, transform.localScale.y);
				is_move_right = false;

			}
			// animator.SetBool ("grounded", grounded);
			// animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

			targetVelocity = move * maxSpeed;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("ENEMY COLLISION!!!!");
			//float force = 3;
			myCeo = coll.collider.gameObject.GetComponent<ceo> ();
			if (myCeo.getIsRight () != is_move_right) {
				//myCeo.setCollided (true);
				GameObject col_l = GameObject.FindGameObjectWithTag ("ColumnLeft");
				GameObject col_r = GameObject.FindGameObjectWithTag ("ColumnLeft");
				float coll_x = col_l.transform.position.x;
				float colr_x = col_r.transform.position.x;
				Debug.Log ("coll_x=" + coll_x);
				if (transform.position.x > (coll_x + 10)) {
					Vector2 force = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
					rigidBody = GetComponent<Rigidbody2D> ();
					transform.Translate (new Vector2 (rigidBody.velocity.x - 3, 0));
					is_collide = true;
				}
				if (transform.position.x < (colr_x - 10)) {
					Vector2 force = coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
					rigidBody = GetComponent<Rigidbody2D> ();
					transform.Translate (new Vector2 (rigidBody.velocity.x + 3, 0));
					is_collide = true;
				}
				/*Vector2 dir = coll.transform.position - transform.position;
				dir = dir.normalized;

				//force.Normalize();
				var magnitude = 5000;

				rigidBody.AddForce(-force * magnitude);
				*/

			}
		} 
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab

		GameObject tempBullet;
		tempBullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		float speed = 50.0f;
		if (is_move_right) {
			speed *= -1;
		}
		tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(speed,0);//tempBullet.transform.forward * 6;

		// Destroy the bullet after 2 seconds
		Destroy(tempBullet, 2.0f);

	}
}