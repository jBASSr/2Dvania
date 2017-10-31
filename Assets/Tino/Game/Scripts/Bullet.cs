using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Rigidbody2D Body;
    public Vector2 Direction = new Vector2(1, 0);
    public Vector2 Speed = new Vector2(1, 1);
    public int Damage = 1;
    public bool IsEnemyHit = false;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 20);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void FixedUpdate()
    {
        if(this.Body == null) { this.Body = this.GetComponent<Rigidbody2D>(); }
        this.Body.velocity = new Vector2(this.Direction.x * this.Speed.x, this.Direction.y * this.Speed.y);
    }
}
