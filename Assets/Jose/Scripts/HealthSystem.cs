using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {
	// Health/Armor
	public Text healthText;
	public float health = 100;
	//public int armor = 250;
	// Refs
	SimpleMovement charSpeed;
	CameraFollow camera;
	Rigidbody2D rb;
	CapsuleCollider2D collider;
	//Text healthText;
	// GUI
	public float hpBar = 0.0f;
	Vector2 pos = new Vector2 (20, 20);
	Vector2 size = new Vector2 (100, 15);
	public Texture2D emptyBar;
	public Texture2D fullBar;
	// Animations
	SpriteRenderer spriteRenderer;
	Animator anim;
	public Image black;
	public Animator fadeanim;
	Color clearColor = new Color(1, 1, 1, 0);
	Color normalColor = new Color(1, 1, 1, 1);
	// Variables
	public float kbForceX = 100;
	public float kbForceY = 150;
	public bool StunnedState = false;
	public float stunTime;
	public float recoverTime = 1.25f;
	public float AnimationWaitTime;
	public float recoverAnimationTime = 0.05f;
	// Camera Control
	private LookAhead look_ahead;

	void Awake() {
		// Required to prevent movement while stunned
		charSpeed = GetComponent<SimpleMovement> ();
		camera = GameObject.Find ("Main Camera").GetComponent<CameraFollow> ();
		anim = GetComponentInChildren<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		collider = GetComponent<CapsuleCollider2D> ();
		healthText = GetComponentInChildren<Text>();
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
			// Animation stuff
			Stunned();
		} else {
			StunnedState = false;
			stunTime = 0.0f;
			spriteRenderer.color = normalColor;
		}
		hpBar = health / 100;
		// 2. Sprite / Animations
	}

	void OnGUI() {
		// HP Bar Base
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			GUI.Box (new Rect (0, 0, size.x, size.y), emptyBar);
			// HP Bar 'Full' State
			GUI.BeginGroup(new Rect(0, 0, size.x * hpBar, size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), fullBar);
			GUI.EndGroup();
			GUI.color = Color.white;
		GUI.Label (new Rect (3, -3f, 100, 20), health.ToString());
		GUI.EndGroup();
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

	void Stunned() {
		if (recoverAnimationTime <= AnimationWaitTime) { recoverAnimationTime += Time.deltaTime; 
		} else {
			if (spriteRenderer.color.a == 0) { 
				spriteRenderer.color = normalColor; 
			} else { 
				spriteRenderer.color = clearColor; 
			}
			recoverAnimationTime = 0;
		}
	}

	void Death() {
		// Disable Object
		this.enabled = false;
		// Disable Controller
		charSpeed.enabled = false;
		// Center Camera on Player "Robot"
		camera.cameraTarget = GameObject.Find("Robot").transform;
		// Death Animation
		anim.SetFloat ("Speed", 0);
		//anim.SetBool("Death", true);
		// Game Over Screen
		fadeanim.SetBool("Fade", true);
	}
}
