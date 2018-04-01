using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ceo : MonoBehaviour {
	public float speed = 1.0f;
	public float fireRate = 0.5F;
	public float bullet_speed = 3.0f;

	public GameObject bloodPrefab;
	public GameObject bulletPrefab;

    private int velx=1;    
    private Vector2 vec;    
    private SpriteRenderer sr;
    private bool is_right;
	private bool is_collided;
	private GameObject tempBlood;
	private SimpleMovement robot;
	private Animator animator;

	private float nextFire = 0.0F;
	private float notNextFire = 0.0f;
	private float firingTime = 0.0f;
	private float fireTime = 0.0f;
	private float notFireTime = 0.5f;
	private bool isFire = false;
	private bool isStart = true;
	public float hitCount = 5.0f;
	AnimatorStateInfo stateInfo;
	int currentFrame;


    // Use this for initialization
    void Start () {
		robot = GameObject.Find ("Robot").GetComponent<SimpleMovement>();
        vec = new Vector2(1, 0);        
        //sr = new SpriteRenderer();
		is_right = true;
		is_collided = false;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		animator = GetComponent<Animator> ();
		RuntimeAnimatorController ac = animator.runtimeAnimatorController;    //Get Animator controller
		for(int i = 0; i<ac.animationClips.Length; i++)                 //For all animations
		{
			if(ac.animationClips[i].name == "ceo_shooting")        //If it has the same name as your clip
			{
				fireTime = ac.animationClips[i].length;
			}
		}
		//Debug.Log ("ceo_shooting length=" +fireTime);
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
					//Debug.Log ("FACING EACHOTHER. SHOOT YOU!!!");
					//INITIALIZE:
					if (isStart == true) {
						animator.SetBool ("is_shooting", true);
						nextFire = Time.time + fireTime;
						isFire = true;
						isStart = false;
						//Debug.Log("is_shooting=" + animator.GetBool ("is_shooting"));
					}
					//--------------------------------------

					if (isFire && Time.time > nextFire) {																		
						if (bulletPrefab != null) {
							notNextFire = Time.time + notFireTime;
							//WaitForSeconds(0.3);
							//Debug.Log("SHOOTING NOW!!!");
							Fire ();
							isFire = false;
							animator.SetBool ("is_shooting", false);
							//Debug.Log("is_shooting=" + animator.GetBool ("is_shooting"));
							//animator.SetBool ("is_shooting",false);
						}
					}
					else if (!isFire && Time.time > notNextFire) {												
						nextFire = Time.time + fireTime;
						isFire = true;
						animator.SetBool ("is_shooting", true);
						//Debug.Log("is_shooting=" + animator.GetBool ("is_shooting"));
							//animator.SetBool ("is_shoosting",false);
					}
					//STOP THE ENEMY SO HE CAN SHOOT:
					if (animator.GetBool ("is_shooting") == true) {
						this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
					} else {
						this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
					}


				}					
			    else {//IF NOT FACING EACH OTHER...
				   animator.SetBool ("is_shooting", false);
					nextFire = Time.time + fireTime;
			    }
			} else {//IF PLAYER NOT GROUNDED
				animator.SetBool ("is_shooting", false);
				nextFire = Time.time + fireTime;
			}
		}


	}
		

    void OnCollisionEnter2D(Collision2D coll)
    {
		Debug.Log("COLLISION WITH GAME OBJECT=" + coll.gameObject.tag);
		if (coll.gameObject.tag == "Wall" || coll.gameObject.tag == "Player")
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
				transform.Translate (new Vector2 (-0.1f,0));
                //this.transform.Translate(new Vector2(2, 0));
            }
            else
            {
				transform.localRotation = Quaternion.Euler(0, 180, 0);                
				transform.Translate (new Vector2 (0.1f,0));
                //this.transform.Translate(new Vector2(-20, 0));
            }
			speed *= -1;

        }

		if (coll.gameObject.tag == "Rocket") {
			Debug.Log ("ROCKET COLLIDED WITH ENEMY CEO");
			Destroy (coll.gameObject);
			hitCount -= 1.0f;
			if (hitCount <= 0.0f) {
				Destroy (this.gameObject);
			}
			Bleed ();
		}

    }
	
	public void setCollided(bool _is_collided){
		is_collided = _is_collided;
	}

	public bool getIsRight(){
		return is_right;
	}

	public void Bleed(){		
		Debug.Log ("CEO BLEEDING!");
		if (tempBlood == null) {
			tempBlood = (GameObject)Instantiate (
				bloodPrefab,
				transform.position,
				transform.rotation
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
			             transform.position,
			             transform.rotation);

		bullet.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed*bullet_speed, 0);
		if (bullet != null) {
			//Destroy (bullet, 10.0f);
		}

	}

	private IEnumerator WaitForAnimation ( Animation animation )
	{
		do
		{
			yield return null;
		} while ( animation.isPlaying );
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Bullet") {
			Debug.Log ("Bullet COLLIDED WITH ENEMY CEO");
			Destroy (c.gameObject);
			hitCount -= 0.5f;
			if (hitCount <= 0) {
				Destroy (this.gameObject);
			}
			Bleed ();
		}
	}


}
