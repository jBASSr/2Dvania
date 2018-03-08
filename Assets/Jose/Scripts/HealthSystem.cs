using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {
	// Health/Armor
	public Text healthText;
	public int health = 100;
	//public int armor = 250;
	// Refs
	SimpleMovement charSpeed;
	Rigidbody2D rb;
	CapsuleCollider2D collider;
	//Text healthText;
	// Animations
	SpriteRenderer spriteRenderer;
	Animator anim;
	// Variables
	public float kbForceX = 100;
	public float kbForceY = 150;
	public bool StunnedState = false;
	public float stunTime;
	public float recoverTime = 1.25f;

	void Awake() {
		// Required to prevent movement while stunned
		charSpeed = GetComponent<SimpleMovement> ();
		anim = GetComponentInChildren<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		collider = GetComponent<CapsuleCollider2D> ();
		//healthText = GetComponentInChildren<Text>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}
	// Use this for initialization
	void Start () {
		healthText = GetComponent<Text> ();
		// Init HUD
	}
	
	// Update is called once per frame
	void Update () {
		//healthText.text = health.ToString ();
		// Implement
		// 1. Invulnerability Timer
		if (StunnedState && stunTime <= recoverTime) {
			stunTime += Time.deltaTime;
		} else {
			StunnedState = false;
			stunTime = 0.0f;
		}
		// 2. Sprite / Animations
	}

	// Collision with Enemy Hitbox
	void OnCollisionEnter2D (Collision2D c)
	{
		//Debug.Log("Test");
		if (c.gameObject.tag == "Enemy" && health > 0 && !StunnedState) {
			// Deal damage to player
			health = Mathf.Clamp(health - 15, 0, 100);
			Debug.Log ("-15 HP!");
			// Implement
			// 1. Update HUD
			// 2. Death function
			if (health <= 0) {
				Debug.Log("Player Died");
				Death ();
				return;
			}
			// 3. Animations
			// 4. Invul or Knockback?
			StunnedState = true;
			Vector2 knockback = new Vector2 (charSpeed.forward ? 
				-kbForceX : kbForceX, kbForceY);
			//Debug.Log (knockback);
			rb.AddForce (knockback);
			//rb.AddForce (Vector3.left * 10, ForceMode2D.Impulse);
			//rb.AddForce (transform.up * (10));
			//rb.AddForce (transform.right * -(100));
			//rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y);
			// 5. Stun / Recovery Time
			//charSpeed.bodyState = 0;
		}
	}
	void Death() {
		// Disable Object
		this.enabled = false;
		// Disable Controller
		charSpeed.enabled = false;
		// Death Animation
		// Game Over Screen
	}
}
