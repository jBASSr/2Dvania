using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil : MonoBehaviour {

	public float speed = 3.0f;
	private string direction;
	private float wall_height=0, wall_width=0;
	private bool onGround=true;

	// Use this for initialization
	void Start () {
		direction = "right";
		onGround = true;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (direction == "up") {//To move up->right
			if (this.transform.position.y > wall_height) {
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
				direction = "right";
			}
		}
		if (direction == "right" && onGround==false) {//to move right->down
			if (this.transform.position.x > wall_width) {
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
				direction = "right";
			}
		}
		
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall")
		{
			if (direction == "right") {//to move right->up
				Debug.Log ("HIT WALL GOING RIGHT!");
				wall_height = coll.collider.bounds.size.y;
				wall_width = coll.collider.bounds.size.x;
				Debug.Log ("WALL HEIGHT=" + wall_height);
				direction = "up";
				onGround = false;
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
			}
		}
		if (coll.gameObject.tag == "Ground"){//to move down->right
			if (direction == "down") {
				direction = "right";
				onGround = true;
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
			}
	   }
   }
}