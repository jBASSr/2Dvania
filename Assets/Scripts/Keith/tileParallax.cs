using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class tileParallax : MonoBehaviour {

	public int offsetX = 2;

	public bool hasRightTile = false;
	public bool hasLeftTile = false;
	public bool reverseScale = false;

	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;

	public float smoothing = 1f;   // Parallax smooth effect
	private float parallaxScale = 0.0f;        // Proportion to move backgrounds based on movement

	private Vector3 prevCamPos;    // Position of camera in previous frame
	SpriteRenderer sr;


	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		spriteWidth = sr.sprite.bounds.size.x;

		prevCamPos = cam.transform.position;
		// Iterate through backgrounds to assign parallaxScales
		parallaxScale = myTransform.position.z*-1; // Must have *-1 for effect to work
	}

	// Update is called once per frame
	void Update () {
		float parallax = (prevCamPos.x - cam.transform.position.x) * parallaxScale;
	     // Apply parallax
		float myTransformTargetPosX = myTransform.position.x + parallax;
	     // Target Position
		Vector3 myTransformTargetPos = new Vector3 (myTransformTargetPosX, myTransform.position.y, myTransform.position.z);
	     // Fade effect (between current and target position) using lerp
		myTransform.position = Vector3.Lerp (myTransform.position, myTransformTargetPos, smoothing * Time.deltaTime);
		prevCamPos = cam.transform.position;
		if (hasLeftTile == false || hasRightTile == false) {
			// Calculate what camera can see 
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
			// Calculate where camera can see edge of a sprite
			float edgeVisiblePosRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;
			// Test if edge of sprite is visble
			if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasRightTile == false) {
				CreateTile(1);
				hasRightTile = true;
			} else if (cam.transform.position.x <= edgeVisiblePosLeft + offsetX && hasLeftTile == false) {
				CreateTile(-1);
				hasLeftTile = true;
			}
		}
	}

	void CreateTile (int side) {
		Debug.Log ("Create Tile Called." + this.tag);
		// Calculate position
		Vector3 newPos;
		if (side > 0) {
			newPos = new Vector3 (sr.bounds.max.x + spriteWidth * side, myTransform.position.y, myTransform.position.z);
		} else {
			newPos = new Vector3 (sr.bounds.min.x + spriteWidth * side, myTransform.position.y, myTransform.position.z);
		}
		// Instantiate and assign new tile
		Transform newTile = Instantiate (myTransform, newPos, myTransform.rotation) as Transform;

		// If sprite is not tilable simply scale to -1
		if (reverseScale == true) {
			newTile.localScale = new Vector3 (newTile.localScale.x * -1, newTile.localScale.y, newTile.localScale.z);
		}
		// Assign to parent
		if (side > 0) {
			newTile.GetComponent<tileParallax> ().hasLeftTile = true;
		} else {
			newTile.GetComponent<tileParallax> ().hasRightTile = true;
		}
	}
}
