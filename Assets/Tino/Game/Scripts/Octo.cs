using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octo : MonoBehaviour {

    public int HP = 1;
    public bool IsEnemy = true;
    
    public Vector2 Speed = new Vector2(1, 1);
    public Vector2 Direction = new Vector2(-1, 0);
    private Vector2 Movement;
    private Rigidbody2D RigidBodyComponent;

    private Weapon Weapon;

	// Use this for initialization
	void Start () {
		
	}
	
    void Awake()
    {
        this.Weapon = this.GetComponent<Weapon>();
    }

	// Update is called once per frame
	void Update () {
        this.Movement = new Vector2(this.Speed.x * this.Direction.x, this.Speed.y * this.Direction.y);
        if(this.Weapon != null && this.Weapon.CanAttack)
        {
            this.Weapon.Attack(true);
        }
	}

    void FixedUpdate()
    {
        if(this.RigidBodyComponent == null)
        {
            this.RigidBodyComponent = GetComponent<Rigidbody2D>();
        }
        this.RigidBodyComponent.velocity = this.Movement;
        this.RigidBodyComponent.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void TakeDamage(int d)
    {
        this.HP -= d;
        if(this.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D o)
    {
        Bullet b = o.gameObject.GetComponent<Bullet>();
        if(b == null)
        {
            return;
        }
        if(b.IsEnemyHit) { return; }
        this.TakeDamage(b.Damage);
        Destroy(b);
    }
}
