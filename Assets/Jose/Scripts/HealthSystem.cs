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
	Rigidbody2D rb;
	CapsuleCollider2D collider;
	//Text healthText;
	// Animations
	SpriteRenderer spriteRenderer;
	Animator anim;
	// Variables
	public float kbForceX = 100;
	public float kbForceY = 10;

	void Awake() {
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
		// 2. Sprite / Animations
	}

	// Collision with Enemy Hitbox
	void OnCollisionEnter2D (Collision2D c)
	{
		//Debug.Log("Test");
		if (c.gameObject.tag == "Enemy" && health > 0) {
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
			Vector2 knockback = new Vector2 (charSpeed.forward ? -kbForceX : kbForceX, kbForceY);
			//rb.AddForce (knockback);
			rb.AddForce (transform.up * (100));
			rb.AddForce (transform.right * -(100));
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y);
			// 5. Stun / Recovery Time
			//charSpeed.bodyState = 0;
		}
	}
	void Death() {

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
        if (Tino.Save.SaveLoadGame.SaveExists && !Tino.Save.SaveLoadGame.PlayerHealthLoaded)
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
        else if (!string.IsNullOrEmpty(doorName))
        {
            Debug.Log(doorName);
            GameObject doorObject = GameObject.Find(doorName);
            Vector3 doorPos = doorObject.transform.position;
            this.transform.position = new Vector3(doorPos.x, doorPos.y + 0.5f, doorPos.z);
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnload;
    }

}
