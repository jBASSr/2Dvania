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

    // Use this for initialization
    void Start () {
        vec = new Vector2(1, 0);        
        //sr = new SpriteRenderer();
		is_right = true;
		is_collided = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (is_collided==false){
        this.transform.Translate(vec * speed * Time.deltaTime);        
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

        }

    }
	
	public void setCollided(bool _is_collided){
		is_collided = _is_collided;
	}

	public bool getIsRight(){
		return is_right;
	}

}
