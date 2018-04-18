using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch_light : MonoBehaviour {

	Light fireLight;
	float minInt = 3f, maxInt = 5f;
	float lightIntensity = 0f;

	// Use this for initialization
	void Start () {
		fireLight = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		lightIntensity = Random.Range(minInt, maxInt);
		fireLight.intensity = lightIntensity;
	}
}
