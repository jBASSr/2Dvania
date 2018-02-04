using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour {

	public Transform[] background; // Store elements in array to be parallaxed
	public float smoothing = 1f;   // Parallax smooth effect
	private float[] parallaxScales;        // Proportion to move backgrounds based on movement

	private Transform cam;		   // Reference to Main_Camera transform
	private Vector3 prevCamPos;    // Position of camera in previous frame

	// Use this for references
	void Awake () {
		// Camera Reference
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		// Store previous frame position
		prevCamPos = cam.position;

		parallaxScales = new float[background.Length];
		// Iterate through backgrounds to assign parallaxScales
		for (int i = 0; i < background.Length; i++) {
			parallaxScales [i] = background [i].position.z*-1; // Must have *-1 for effect to work
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < background.Length; i++) {
			float parallax = (prevCamPos.x - cam.position.x) * parallaxScales[i];
			// Apply parallax
			float backgroundTargetPosX = background [i].position.x + parallax;
			// Target Position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, background[i].position.y, background[i].position.z);
			// Fade effect (between current and target position) using lerp
			background[i].position = Vector3.Lerp (background[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		// Update prevCamPos to current position at end of frame
		prevCamPos = cam.position;
	}
}