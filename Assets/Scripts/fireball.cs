using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour {

	private Color old_color;
	private GameObject robot;
	private PlayerHUD ph;
	// Use this for initialization
	void Start () {
		robot = GameObject.Find ("Robot");
		ph = robot.GetComponent<PlayerHUD> ();
		old_color = robot.gameObject.GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag == "Player") {
			Destroy (this.gameObject, 0.2f);
			if (ph != null) {
				ph.adjustHealth (-15.0f);
			}
			//this.gameObject.GetComponent<SpriteRenderer>().color=new Color(1, 1, 0, 1);
			this.gameObject.GetComponent<SpriteRenderer>().color=new Color(0, 0, 0, 1);
			//StartCoroutine(hitPlayer());
		}
	}

	IEnumerator hitPlayer (){		
		Debug.Log ("HIT PLAYER CALLED!");
		this.gameObject.GetComponent<SpriteRenderer>().color=new Color(0, 0, 0, 1);
		yield return new WaitForSeconds(0.2f);
		robot.gameObject.GetComponent<SpriteRenderer>().color=old_color;
	}
}
