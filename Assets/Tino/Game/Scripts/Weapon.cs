using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Transform BulletPrefab;
    public float Rate = 0.25f;
    public Vector2 Direction = new Vector2(1, 0);
    private float Cooldown = 0f;

	// Use this for initialization
	void Start () {
        this.Cooldown = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.Cooldown > 0)
        {
            this.Cooldown -= Time.deltaTime;
        }
	}

    public void Attack(bool isEnemy)
    {
        if(!this.CanAttack) { return; }
        this.Cooldown = this.Rate;
        Transform bullet = Instantiate(BulletPrefab) as Transform;
        bullet.position = transform.position;
        Bullet bulletScript = bullet.gameObject.GetComponent<Bullet>();
        if(bulletScript != null)
        {
            bulletScript.IsEnemyHit = isEnemy;
            bulletScript.Direction = this.Direction;
        }
    }

    public bool CanAttack { get { return this.Cooldown <= 0f; } }
}
