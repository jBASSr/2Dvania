using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHP : MonoBehaviour {

	// GUI
	float hpBar = 0.0f;
	Vector2 pos = new Vector2 (20, 40);
	Vector2 size = new Vector2 (60, 20);
	Texture2D emptyBar;
	Texture2D fullBar;
	HealthSystem hp;

	void Awake() {
		hp = GetComponent<HealthSystem> ();
	}

	void OnGUI() {
		// HP Bar Base
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			GUI.Box (new Rect (0, 0, size.x, size.y), emptyBar);
			// HP Bar 'Full' State
			GUI.BeginGroup(new Rect(0, 0, size.x * hpBar, size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), fullBar);
			GUI.EndGroup();
		GUI.EndGroup();
	}

	// Update is called once per frame
	void Update () {
		hpBar = hp.health;
		//hpBar = Time.time * 0.05f;
	}
}
