using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour {
	// Health/Armor
	public Text healthText;
    public int health = 50;
    public int maxHealth = 100;
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
	public GameObject Explode;
	float timer;
	bool exploded;
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
	bool isAlive;
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
		isAlive = true;
		exploded = false;
		timer = 0.0f;
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
		if (!isAlive) {
			timer += Time.deltaTime;
			if (timer > 2.0f && !exploded) {
				// After 2 seconds, blow up.
				anim.SetBool("Death", true);
				Debug.Log("Blew up");
				exploded = true;
				Instantiate (Explode, rb.transform.position, rb.transform.rotation);
			}
		}
	}
	/*
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
	*/
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
	// Collision with Enemy Projectiles
	void OnTriggerEnter2D(Collider2D other) {
		if ((other.tag == "Enemy_Bullet") && health > 0 && !StunnedState) {
			health = Mathf.Clamp(health - 15, 0, 100);
			Debug.Log ("-15 HP!");
			if (health <= 0) {
				Debug.Log("Player Died");
				Death ();
				return;
			}
			StunnedState = true;
			Vector2 knockback = new Vector2 (charSpeed.forward ? 
				-kbForceX : kbForceX, kbForceY);
			//Debug.Log (knockback);
			rb.AddForce (knockback);
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
		Debug.Log ("We ded af");
		// Disable Object
		//this.enabled = false;
		// Disable Controller
		charSpeed.enabled = false;
		// Center Camera on Player "Robot"
		camera.cameraTarget = GameObject.Find("Robot").transform;
		GetComponent<AudioSource> ().Pause ();// Hard coding cause I'm bad.
		// Death Animation
		anim.SetFloat ("Speed", 0);
		//anim.SetBool("Death", true);
		// Game Over Screen
		fadeanim.SetBool("Fade", true);
		isAlive = false;
        //Invoke("Reload", 2);
	}
    
    /// <summary>
    /// Reloads the game from the last save point.
    /// </summary>
    private void Reload()
    {
        Tino.Save.SaveLoadGame.LoadGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(Tino.Save.SaveLoadGame.SavedGame.CurrentScene);
    }

    public bool RefillHealth(int health)
    {
        if(this.health >= this.maxHealth) { return false; }
        if(health == 0)
        {
            this.health = this.maxHealth;
            return true;
        }
        this.health = this.health + health > this.maxHealth ? this.maxHealth : this.health + health;
        return true;
    }

    public bool MaxHealthIncrease(int amount)
    {
        this.maxHealth += amount;
        //Health PickUp Sound
        FindObjectOfType<AudioManager_2>().Play("Health");
        return true;
    }

    public void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnload;
    }

    public void OnDisable()
    {
        Tino.PlayerState.Health = this.health;
        Tino.PlayerState.MaxHealth = this.maxHealth;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        this.health = Tino.PlayerState.Health;
        this.maxHealth = Tino.PlayerState.MaxHealth;

        //note(tino): why is this here in the health script instead of its own script?
        //I put it here to avoid any meta file merge conflicts. It can be freely moved into its own script after we merge all the code together.
        string doorName = Tino.WorldState.GetDoorName();
        if(!Tino.Save.SaveLoadGame.SaveExists)
        {
            this.maxHealth = Tino.PlayerState.StartingHealth;
            this.health = Tino.PlayerState.StartingHealth;
        }
        else if (Tino.Save.SaveLoadGame.SaveExists && !Tino.Save.SaveLoadGame.PlayerHealthLoaded)
        {
            Vector3 newPos;
            newPos.x = Tino.Save.SaveLoadGame.SavedGame.PlayerPosition.X;
            newPos.y = Tino.Save.SaveLoadGame.SavedGame.PlayerPosition.Y;
            newPos.z = Tino.Save.SaveLoadGame.SavedGame.PlayerPosition.Z;
            this.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
            this.health = Tino.Save.SaveLoadGame.SavedGame.PlayerCurrentHealth;
            this.maxHealth = Tino.Save.SaveLoadGame.SavedGame.PlayerMaxHealth;
            Tino.Save.SaveLoadGame.PlayerHealthLoaded = true;
        }
        if (!string.IsNullOrEmpty(doorName))
        {
            GameObject doorObject = GameObject.Find(doorName);
            Vector3 doorPos = doorObject.transform.position;
            this.transform.position = new Vector3(doorPos.x, doorPos.y + 0.5f, this.transform.position.z);
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnload;
    }

}
