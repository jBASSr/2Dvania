using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil : MonoBehaviour {

	public GameObject oil_mess;
	public float speed = 3.0f;
	private string direction;
	private bool is_right;
	private bool onGround=true;
	private GameObject wall;

	float is_right_mul = -1.0f;
	private SimpleMovement robot;
	private GameObject oilPrefab;
	private Vector2 velocityNow;
	private SpriteRenderer sr, wsr;
	private CapsuleCollider2D capcol;
	private BoxCollider2D boxcol;
	public float hitCount = 3.0f;
	private float startRocketHitTime = 0.0f;
	private float startBulletHitTime = 0.0f;
	public float hitRocketTime = 1.0f;
	public float hitBulletTime = 0.5f;
	private float xStart = 0.0f;
	private float xLast = 0.0f;
	public float xRange = 3.0f;
	//private int NWALL = 11; 
	//private int NGROUND = 13;
	private PlayerHUD ph;

	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot").GetComponent<SimpleMovement>();
		ph = GameObject.Find ("Robot").GetComponent<PlayerHUD>();
		Debug.Log ("OIL STARTED!!!!!");
		direction = "right";
		is_right = true;
		onGround = true;
		velocityNow = new Vector2 (speed, 0);
		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;
		sr = GetComponent<SpriteRenderer> ();
		capcol = robot.GetComponent<CapsuleCollider2D> ();
		boxcol = this.GetComponent<BoxCollider2D> ();
		xStart = this.transform.position.x;
		xLast = xStart;
		startRocketHitTime = Time.time - hitRocketTime;
		startBulletHitTime = Time.time - hitBulletTime;

	}

	// Update is called once per frame
	void Update () {
		
		this.GetComponent<Rigidbody2D> ().velocity = velocityNow;

		if (oilPrefab != null) {
			Destroy (oilPrefab,0.0f);
		}
		if (sr != null && wsr != null) {
			if (direction == "up") {//To move up->right			
				if (sr.bounds.min.y > wsr.bounds.max.y + 0.3) {				
					if (is_right) {
						is_right_mul = 1.0f;
						direction = "right";
					} else {
						is_right_mul = -1.0f;
						direction = "left";
					}
					velocityNow = new Vector2 (is_right_mul * speed, 0);
				}
			} else if ((direction == "left" || direction == "right") && onGround == false) {//to move right->down
				if (is_right) {
					if (sr.bounds.min.x > wsr.bounds.max.x + 0.3) {
						velocityNow = new Vector2 (0, -speed);
						direction = "down";
					}
				} else {
					//Debug.Log("Direction=" + direction);
					if (sr.bounds.max.x + 0.3 < wsr.bounds.min.x) {
						//Debug.Log("Direction=" + direction + "LEFT OF WALL!!!!");
						velocityNow = new Vector2 (0, -speed);
						direction = "down";
					}			
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
			//Debug.Log ("OIL ON PLAYER!!!!");
			if (oilPrefab == null) {
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
				
			}
			if (ph != null) {
				ph.adjustHealth (-0.5f);
			}
		}
		if (Time.time < startRocketHitTime + hitRocketTime) {
			if (oilPrefab == null) {
				//Debug.Log ("Instantiating oil mess...");
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		if (Time.time < startBulletHitTime + hitBulletTime) {
			if (oilPrefab == null) {
				//Debug.Log ("Instantiating oil mess...");
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		if (Mathf.Abs(this.transform.position.x - xStart) > xRange && Mathf.Abs(this.transform.position.x - xLast)>(xRange/2)) {
			//Debug.Log ("SWITCHING DIRECTION!!!!!! direction=" + direction);
			xLast = this.transform.position.x;
			is_right = !is_right;
			if (direction == "right") {
				velocityNow = new Vector2 (-speed, 0);
				//Debug.Log ("SWITCHING LEFT!!!!!!");
				direction = "left";
				is_right = false;
			}
			else if (direction == "left"){
				velocityNow = new Vector2 (speed, 0);
				//Debug.Log ("SWITCHING RIGHT!!!!!!");
			    direction = "right";
				is_right = true;
			}
			else if(direction == "up"){
				//Debug.Log ("SWITCHING RIGHT AND GOING UP???!!!!!!");
			}
			else if(direction == "down"){
				//Debug.Log ("SWITCHING RIGHT AND GOING DOWN???!!!!!!");
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("OIL COLLIDED WITH SOMETHING!!!!!=" + coll.gameObject.tag);
		if (direction == "right") {
			    Debug.Log("RIGHT HIT WALL GOING UP?");
				wall = coll.gameObject;
				wsr = wall.GetComponent<SpriteRenderer> ();
				direction = "up";
				onGround = false;
				velocityNow = new Vector2 (0, speed);
	    }

		if (direction == "left") {
			Debug.Log("LEFT HIT WALL GOING UP? coll.gameObject.layer=" + coll.gameObject.layer );
			wall = coll.gameObject;
			wsr = wall.GetComponent<SpriteRenderer> ();
		   direction = "up";
			onGround = false;
			velocityNow = new Vector2 (0, speed);
		}
		if (direction == "down") {				
			onGround = true;
			if (is_right == true) {
				Debug.Log("DOWN HIT GROUND GOING RIGHT?");
				direction = "right";
				velocityNow = new Vector2 (speed, 0);	
			} else {
				direction = "left";
				velocityNow = new Vector2 (-speed, 0);	
				Debug.Log("DOWN HIT GROUND GOING LEFT?");
			}
		}
		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED WITH OIL");
			Destroy (coll.gameObject);
			hitCount -= 1.0f;
			startRocketHitTime = Time.time;
			if (hitCount <= 0.0f) {
				Destroy (this.gameObject);
				if (oilPrefab != null) {
					Destroy (oilPrefab,0.0f);
				}
			}
		}
		
   }

	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.tag == "Bullet") {
			Debug.Log ("BULLET COLLIDED WITH OIL!");
			Destroy (c.gameObject);
			hitCount -= 0.5f;
			startBulletHitTime = Time.time;
			if (hitCount<= 0.0f) {
				Destroy (this.gameObject);
				if (oilPrefab != null) {
					Destroy (oilPrefab,0.0f);
				}
			}
		}
	}
}