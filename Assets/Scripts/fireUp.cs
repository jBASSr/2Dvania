using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireUp : MonoBehaviour {

	public float fireRate = 0.5F;
	public float rocket_speed = 5.0f;
	private float lastFire = 0.0f;
	private GameObject bullet;

	private float bulletSpeed = 100;
	private SpriteRenderer robot_sr;

	//FOR FIRING:
	public GameObject rocketPrefab;
	public GameObject cannon_up;


	// Use this for initialization
	void Start () {
		robot_sr = GameObject.Find ("Robot").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			if (Time.time > lastFire + fireRate) {
				lastFire = Time.time;
				Fire ();
			}
		}
	}

	void Fire()
	{
		var my_cannon_up = (GameObject)Instantiate (
			cannon_up,
			new Vector2((float)(transform.position.x), 
				(float)(this.GetComponent<SpriteRenderer>().bounds.max.y + 0.1)),transform.rotation);		
		
		var rocket = (GameObject)Instantiate (
			rocketPrefab,
			new Vector2( (float)my_cannon_up.transform.position.x, (float)(my_cannon_up.GetComponent<SpriteRenderer>().bounds.max.y + 0.5)), my_cannon_up.transform.rotation);
		//Debug.Log ("rocket velocity=" + speedX + forward_mult * rocket_speed);
		rocket.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, rocket_speed);
		if (rocket != null) {			
			Destroy (rocket, 10.0f);
		}
		if (my_cannon_up != null) {
			Destroy (my_cannon_up, 0.05f);
		}

	}
}
