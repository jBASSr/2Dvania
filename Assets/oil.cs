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
	float height=0.1462913f;
	float width = 0.1658955f;

	float is_right_mul = -1.0f;
	public SimpleMovement robot;
	private GameObject oilPrefab;

	// Use this for initialization
	void Start () {
		direction = "horizontal";
		is_right = true;
		onGround = true;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		//height = this.GetComponent<Collider>().bounds.size.y;
		//width = this.GetComponent<Collider>().bounds.size.x;
	}

	// Update is called once per frame
	void Update () {
		if (oilPrefab != null) {
			Destroy (oilPrefab,0.0f);
		}
		if (direction == "vertical") {//To move up->right			
			//Debug.Log("WALL HEIGHT=" + wall_height + "y=" + this.transform.position.y) ;


			if (this.transform.position.y + 0.5*height - 0.5> wall.GetComponent<Transform>().position.y + 0.5*wall_height) {				
				if (is_right) {
					is_right_mul = 1.0f;
				} else {
					is_right_mul = -1.0f;
				}
				//Debug.Log ("TOP OF WALL");
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (is_right_mul*speed, 0);
				direction = "horizontal";
			}
		}
		else if (direction == "horizontal" && onGround==false) {//to move right->down
			//RectTransform rt =(RectTransform)this.transform;
			if (is_right) {
				if (this.transform.position.x + (0.5 * width) - 0.7 > wall.GetComponent<Transform> ().position.x + (0.5 * wall_width)) {
					this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
					direction = "vertical";
					//Debug.Log ("MOVING RIGHT & DOWN");
				}
			} else {
				if (this.transform.position.x - (0.5 * width) + 0.7 < wall.GetComponent<Transform> ().position.x - (0.5 * wall_width)) {
					this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
					direction = "vertical";
					//Debug.Log ("MOVING LEFT & DOWN");
				}			
			}
		}

		if (robot.GetComponent<CapsuleCollider2D>().bounds.Intersects(this.GetComponent<BoxCollider2D>().bounds)) {
			Debug.Log ("OIL ON PLAYER!!!!");
			if (oilPrefab == null) {
				oilPrefab = (GameObject)Instantiate (
					oil_mess,
					transform.position,
					transform.rotation);
			}
		}
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//if (coll.gameObject.tag == "Player") {
		//	Debug.Log ("OIL ON PLAYER!!!!");
		//}

		if (coll.gameObject.tag == "Wall")
		{
			if (direction == "horizontal") {//to move right->up
				wall = coll.gameObject;
				wall_height = coll.collider.bounds.size.y;
				wall_width = coll.collider.bounds.size.x;
				direction = "vertical";
				onGround = false;
				//Debug.Log("MOVING UP");
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
			}
		}
		if (coll.gameObject.tag == "Ground" && direction == "vertical"){//to move down->right
			//Debug.Log("BOUNCED ON GROUND");
		    is_right = !is_right;
			this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);	
	    }
		if (coll.gameObject.tag == "MiddleGround" && direction == "vertical"){//to move down->right
			onGround = true;
			direction = "horizontal";
			if (is_right) {
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);	
			} else {
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, 0);	
			}
		}
   }
}