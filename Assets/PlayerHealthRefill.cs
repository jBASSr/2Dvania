using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRefill : MonoBehaviour {

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c) {		
		if (c.tag == "Player") {
			Debug.LogError ("GOT HEALTH REFILL!");
			GameObject.Find ("Robot").GetComponent<PlayerHUD> ().adjustHealth (30.0f);
			Destroy (this.gameObject);
		}
	}
}
