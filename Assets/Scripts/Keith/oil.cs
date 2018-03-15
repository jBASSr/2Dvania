using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil : MonoBehaviour {

	public GameObject oil_mess;
	public float speed = 3.0f;
	private string direction;
	private bool is_right;
	private float wall_height=0, wall_width=0;
	private bool onGround=true;
	private GameObject wall;

	float is_right_mul = -1.0f;
	private SimpleMovement robot;
	private GameObject oilPrefab;
	private Vector2 velocityNow;
	private SpriteRenderer sr, wsr;
	private CapsuleCollider2D capcol;
	private BoxCollider2D boxcol;
	public int hitCount = 3;
	private float startRocketHitTime = 0.0f;
	public float hitRocketTime = 1.0f;

	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot").GetComponent<SimpleMovement>();
		direction = "horizontal";
		is_right = true;
		onGround = true;
		velocityNow = new Vector2 (speed, 0);
		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		sr = GetComponent<SpriteRenderer> ();
		capcol = robot.GetComponent<CapsuleCollider2D> ();
		boxcol = this.GetComponent<BoxCollider2D> ();

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("OIL IS UPDATED?!!!!!");
		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		if (oilPrefab != null) {
			Destroy (oilPrefab,0.0f);
		}
		if (direction == "vertical") {//To move up->right			
			if (sr.bounds.min.y> wsr.bounds.max.y+0.3) {				
				if (is_right) {
					is_right_mul = 1.0f;
				} else {
					is_right_mul = -1.0f;
				}
				velocityNow = new Vector2 (is_right_mul*speed, 0);
				this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
				direction = "horizontal";
			}
		}
		else if (direction == "horizontal" && onGround==false) {//to move right->down
			if (is_right) {
				if (sr.bounds.min.x > wsr.bounds.max.x + 0.3) {
					velocityNow = new Vector2 (0, -speed);
					this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
					direction = "vertical";
				}
			} else {
				if (sr.bounds.max.x + 0.3 < wsr.bounds.min.x) {
					velocityNow = new Vector2 (0, -speed);
					this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
					direction = "vertical";
				}			
			}
		}
		/*if (robot.scene.name) {
			Debug.Log ("THE ROBOT IS active?");
		}
		*/
		bool inx = (capcol.bounds.min.x < boxcol.bounds.max.x) && (capcol.bounds.max.x > boxcol.bounds.min.x);
		bool iny = (capcol.bounds.min.y < boxcol.bounds.max.y) && (capcol.bounds.max.y > boxcol.bounds.min.y);
		if (inx && iny){
			Debug.Log ("OIL ON PLAYER!!!!");
			if (oilPrefab == null) {
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		if (Time.time < startRocketHitTime + hitRocketTime) {
			if (oilPrefab == null) {
				Debug.Log ("Instantiating oil mess...");
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag == "Wall")
		{
			//Debug.Log ("OIL COLLIDED WITH WALL. this=" + this.gameObject.tag);
			if (direction == "horizontal") {//to move right->up							
				wall = coll.gameObject;
				wsr = wall.GetComponent<SpriteRenderer> ();
				wall_height = wall.GetComponent<BoxCollider2D>().size.y;
				wall_width = wall.GetComponent<BoxCollider2D>().size.x;
				direction = "vertical";
				onGround = false;
				//Debug.Log("MOVING UP wall_height=" + wall_height + ", wall_width=" + wall_width);
				velocityNow = new Vector2 (0, speed);
				this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
			}
		}
		if (coll.gameObject.tag == "Ground" && direction == "vertical"){//to move down->right
			//Debug.Log("BOUNCED ON GROUND");
		    is_right = !is_right;
			velocityNow = new Vector2 (0, speed);	
			this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
	    }
		if (coll.gameObject.tag == "MiddleGround" && direction == "vertical"){//to move down->right
			onGround = true;
			direction = "horizontal";
			if (is_right) {
				velocityNow = new Vector2 (speed, 0);	
			} else {
				velocityNow = new Vector2 (-speed, 0);	
			}
			this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		}
		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED CEO");
			Destroy (coll.gameObject);
			hitCount--;
			startRocketHitTime = Time.time;
			if (hitCount == 0) {
				Destroy (this.gameObject);
			}
		}
   }
}