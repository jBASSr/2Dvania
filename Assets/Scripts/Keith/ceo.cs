using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ceo : MonoBehaviour {
    private int velx=1;
    public float speed = 1.0f;
    private Vector2 vec;    
    private SpriteRenderer sr;
    private bool is_flipX;

    // Use this for initialization
    void Start () {
        vec = new Vector2(1, 0);        
        //sr = new SpriteRenderer();
        is_flipX = false;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("velx=" + velx);
        this.transform.Translate(vec * speed * Time.deltaTime);        
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("COLLISION!!!!");
        if (coll.gameObject.tag == "Column")
        {
            //speed *= -1;
            //sr.flipX = !sr.flipX;
            is_flipX = !is_flipX;
            if (is_flipX == true)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                //this.transform.Translate(new Vector2(2, 0));
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                //this.transform.Translate(new Vector2(-20, 0));
            }

        }

    }

}
