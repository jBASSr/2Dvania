using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
	// Health/Armor
	public int health = 100;
	//public int armor = 250;
	// Refs
	SimpleMovement charSpeed;
	Rigidbody rb;
	// Animations
	SpriteRenderer spriteRenderer;
	Animator anim;
	// Variables
	public float kbForceX = 100;
	public float kbForceY = 10;

	void Awake() {
		charSpeed = GetComponent<SimpleMovement> ();
		anim = GetComponentInChildren<Animator> ();
		rb = GetComponent<Rigidbody> ();
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}
	// Use this for initialization
	void Start () {
		// Init HUD
	}
	
	// Update is called once per frame
	void Update () {
		// Implement
		// 1. Invulnerability Timer
		// 2. Sprite / Animations
	}

	// Collision with Enemy Hitbox
	void OnTriggerStay(Collider2D enemy) {
		if (enemy.tag == "Enemy" && health > 0) {
			// Deal damage to player
			health = Mathf.Clamp(health - 15, 0, 100);
			Debug.Log ("-15 HP!");
			// Implement
			// 1. Update HUD
			// 2. Death function
			if (health <= 0) {
				Death ();
				return;
			}
			// 3. Animations
			// 4. Invul or Knockback?
			Vector2 knockback = new Vector2 (charSpeed.forward ? -kbForceX : kbForceX,
				                    kbForceY);
			rb.AddForce (knockback);
			// 5. Stun / Recovery Time
			//charSpeed.bodyState = 0;
		}
	}
	void Death() {

	}
}
