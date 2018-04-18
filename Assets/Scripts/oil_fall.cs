using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil_fall : MonoBehaviour {

	public GameObject oil_mess;
	public float speed = 3.0f;
	private string direction;
	private bool is_right;
	private float wall_height=0, wall_width=0;
	private bool onGround=true;
	private GameObject wall;

	float is_right_mul = -1.0f;
	private SimpleMovement robot;
	private GameObject oilMess;
	private Vector2 velocityNow;
	private SpriteRenderer sr, wsr;
	private CapsuleCollider2D capcol;
	private BoxCollider2D boxcol;
	public int hitCount = 3;
	private float startRocketHitTime = 0.0f;
	private float startBulletHitTime = 0.0f;
	public float hitRocketTime = 1.0f;
	public float hitBulletTime = 1.0f;
	private PlayerHUD ph;

	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot").GetComponent<SimpleMovement>();
		ph = GameObject.Find ("Robot").GetComponent<PlayerHUD>();
		direction = "horizontal";
   	     sr = GetComponent<SpriteRenderer> ();
		capcol = robot.GetComponent<CapsuleCollider2D> ();
		boxcol = this.GetComponent<BoxCollider2D> ();

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("OIL IS UPDATED?!!!!!");
		if (oilMess != null) {
			Destroy (oilMess,0.0f);
		}
		bool inx = (capcol.bounds.min.x < boxcol.bounds.max.x) && (capcol.bounds.max.x > boxcol.bounds.min.x);
		bool iny = (capcol.bounds.min.y < boxcol.bounds.max.y) && (capcol.bounds.max.y > boxcol.bounds.min.y);
		if (inx && iny){
			Debug.Log ("OIL ON PLAYER!!!!");
			if (oilMess == null) {
				oilMess = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
				if (ph != null) {
					ph.adjustHealth (-2.0f);
				}
			}
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
			Debug.Log ("ROCKET COLLIDED WITH OIL");
			Destroy (coll.gameObject);
			startRocketHitTime = Time.time;
			Destroy (this.gameObject);
			if (oilMess != null) {
				Destroy (oilMess);
			}
		}else if(coll.gameObject.tag == "Player"){
			Destroy (this.gameObject, 1.0f);
			if (oilMess != null) {
				Destroy (oilMess);
			}
		}
		else{
			Destroy (this.gameObject, 0.0f);
			if (oilMess != null) {
				Destroy (oilMess);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("BULLET COLLIDED WITH OIL FALL!");
			Destroy (c.gameObject);
			startBulletHitTime = Time.time;
			Destroy (this.gameObject);
			if (oilMess != null) {
				Destroy (oilMess,0.0f);
			}		
		}
	}
}