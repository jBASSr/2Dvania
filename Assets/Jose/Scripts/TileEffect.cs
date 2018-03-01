using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class TileEffect : MonoBehaviour {

	public int offsetX = 2;

	public bool hasRightTile = false;
	public bool hasLeftTile = false;
	public bool reverseScale = false;

	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;

	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
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
		Debug.Log("Create Tile Called.");
		// Calculate position
		Vector3 newPos = new Vector3 (myTransform.position.x + spriteWidth * side, myTransform.position.y, myTransform.position.z);
		// Instantiate and assign new tile
		Transform newTile = Instantiate (myTransform, newPos, myTransform.rotation) as Transform;

		// If sprite is not tilable simply scale to -1
		if (reverseScale == true) {
			newTile.localScale = new Vector3 (newTile.localScale.x * -1, newTile.localScale.y, newTile.localScale.z);
		}
		// Assign to parent
		if (side > 0) {
			newTile.GetComponent<TileEffect> ().hasLeftTile = true;
		} else {
			newTile.GetComponent<TileEffect> ().hasRightTile = true;
		}
	}
}
