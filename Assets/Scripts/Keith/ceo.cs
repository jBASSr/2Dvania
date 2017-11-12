using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ceo : MonoBehaviour {
    private int velx=1;
    public float speed = 1.0f;
    private Vector2 vec;    
    private SpriteRenderer sr;
    private bool is_right;
	private bool is_collided;

	public GameObject bloodPrefab;
	public Transform bloodSpawn;
	private GameObject tempBlood;
	public SimpleMovement robot;
	private Animator animator;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;


    // Use this for initialization
    void Start () {
        vec = new Vector2(1, 0);        
        //sr = new SpriteRenderer();
		is_right = true;
		is_collided = false;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		animator = GetComponent<Animator> ();
    }
	
	// Update is called once per frame
	void Update () {
		if (is_collided==false){
        //this.transform.Translate(vec * speed * Time.deltaTime);        
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
			if (tempBlood != null) {
				//tempBlood.GetComponent<Rigidbody2D> ().velocity = this.GetComponent<Rigidbody2D> ().velocity;
				Vector2 pos = tempBlood.transform.position;
				Vector2 pos2 = transform.position;
				pos.x = pos2.x;
				pos.y = pos2.y;
				tempBlood.transform.position = pos;
			}
		}
		if (robot != null) {
			if (robot.isGrounded) {
				if ((robot.forward && !is_right && robot.transform.position.x < transform.position.x) || (!robot.forward && is_right && robot.transform.position.x > transform.position.x)) {
					Debug.Log ("FACING EACHOTHER. SHOOT YOU!!!");
					if (animator.GetBool ("is_shooting") == false) {
						animator.SetBool ("is_shooting", true);
						Fire ();
					} else {
						animator.SetBool ("is_shooting", false);
					}

				}					
			} else {//IF PLAYER NOT GROUNDED
				animator.SetBool ("is_shooting", false);
			}
		}

	}
		

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("COLLISION!!!!");
		if (coll.gameObject.tag == "ColumnLeft" || coll.gameObject.tag == "ColumnRight" || coll.gameObject.tag == "Player")
        {
			if (coll.gameObject.tag == "Player") {
				Debug.Log ("PLAYER COLLISION!!");
			}
            //speed *= -1;
            //sr.flipX = !sr.flipX;
			is_right = !is_right;
			if (is_right == true)
            {
				transform.localRotation = Quaternion.Euler(0, 0, 0);
                //this.transform.Translate(new Vector2(2, 0));
            }
            else
            {
				transform.localRotation = Quaternion.Euler(0, 180, 0);                
                //this.transform.Translate(new Vector2(-20, 0));
            }
			speed *= -1;

        }

    }
	
	public void setCollided(bool _is_collided){
		is_collided = _is_collided;
	}

	public bool getIsRight(){
		return is_right;
	}

	public void Bleed(){		
		if (tempBlood == null) {
			tempBlood = (GameObject)Instantiate (
				bloodPrefab,
				bloodSpawn.position,
				bloodSpawn.rotation
			);
			Vector2 pos = tempBlood.transform.position;
			Vector2 pos2 = transform.position;
			pos.x = pos2.x;
			pos.y = pos2.y;
			tempBlood.transform.position = pos;
			Destroy (tempBlood, 2.0f);
		}
	}

	void Fire(){
		var bullet = (GameObject)Instantiate (
			             bulletPrefab,
			             bulletSpawn.position,
			             bulletSpawn.rotation);

		bullet.GetComponent<Rigidbody2D> ().velocity = bullet.transform.forward * 6;
		Destroy (bullet, 2.0f);

	}



}
