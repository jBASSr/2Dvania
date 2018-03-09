using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		robot = GameObject.Find ("Robot");
		rob_col = robot.GetComponent<CapsuleCollider2D> ();
		bat_col = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (angryBat != null) {
			Destroy (angryBat,0.0f);
		}
		transform.position = new Vector2(transform.position.x + speed, startPosition.y - yRange * (Mathf.Sin(Time.time)));
		if (Mathf.Abs(transform.position.x - startPosition.x) > xRange) {
			speed *= -1.0f;
			if (speed > 0) {
				transform.localRotation = Quaternion.Euler (0, 180, 0);  
			} else {
				transform.localRotation = Quaternion.Euler (0, 0, 0);
			}
		}

		bool inx = (rob_col.bounds.min.x < bat_col.bounds.max.x) && (rob_col.bounds.max.x > bat_col.bounds.min.x);
		bool iny = (rob_col.bounds.min.y < bat_col.bounds.max.y) && (rob_col.bounds.max.y > bat_col.bounds.min.y);
		if (inx && iny){
			Debug.Log ("BAT ON PLAYER!!!!");
			if (angryBat == null) {
				Debug.Log ("INSTATIATING batPrefab!!!");
				angryBat = (GameObject)Instantiate (
					batPrefab,
					transform.position,
					transform.rotation);
			}
		}
		if (Time.time < startRocketHitTime + hitRocketTime) {
			if (angryBat == null) {
				Debug.Log ("Instantiating oil mess...");
				angryBat = (GameObject)Instantiate (
					batPrefab,
					transform.position,
					transform.rotation);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("BAT COLLIDED WITH SOMETHING!");
		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED BAT");
			Destroy (coll.gameObject);
			hitCount--;
			startRocketHitTime = Time.time;
			if (hitCount == 0) {
				Destroy (this.gameObject);
			}
		}
	}
}
