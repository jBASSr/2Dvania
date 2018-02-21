using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
	// Health/Armor
	public int health = 100;
	//public int armor = 250;
	// Refs
	SimpleMovement charSpeed;
	// Animations
	Animator anim;
	// Use this for initialization
	void Start () {
		// health = Mathf.Clamp(health - 15, 0, 100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
