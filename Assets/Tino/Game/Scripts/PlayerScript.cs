using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public Vector2 Speed = new Vector2(50, 50);
    private Vector2 Movement;
    private Rigidbody2D RigidBodyComponent;
    public int HP = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        this.Movement = new Vector2(this.Speed.x * inputX, this.Speed.y * inputY);

        bool shoot = Input.GetButtonDown("Fire1");

        if(shoot)
        {
            Weapon w = this.GetComponent<Weapon>();
            if(w != null)
            {
                w.Attack(false);
            }
        }
	}

    void FixedUpdate()
    {
        if(this.RigidBodyComponent == null) { this.RigidBodyComponent = GetComponent<Rigidbody2D>(); }
        this.RigidBodyComponent.velocity = this.Movement;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Octo enemy = other.gameObject.GetComponent<Octo>();
        if(enemy == null) { return; }
        this.TakeDamage(1);
    }

    public void TakeDamage(int d)
    {
        this.HP -= d;
        if (this.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}