using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class col_left_col : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnControllerColliderHit(Collision collisionInfo)
    {

        Debug.Log("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
        Debug.Log("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
        Debug.Log("Their relative velocity is " + collisionInfo.relativeVelocity);

    }
}
