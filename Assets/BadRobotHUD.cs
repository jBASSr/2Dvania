﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class BadRobotHUD : MonoBehaviour
	{
	    public int maxHealthBarLength;
	    public int healthBarLength;
	    int maxHealth;
	    int curHealth;
	    private bad_robot myBadRobot;
   	    public GUIStyle style1, style2;

		void Start()
		{
		myBadRobot = GameObject.Find ("BAD_ROBOT").GetComponent<bad_robot> ();
		maxHealthBarLength = Screen.width / 6;
		healthBarLength = maxHealthBarLength;
		maxHealth = Mathf.FloorToInt(myBadRobot.maxHealth);
		curHealth = maxHealth;	}

		void Update()
		{
   		    adjustHealth ();
		}

	void OnGUI(){			  
		style1 = new GUIStyle (GUI.skin.box);
		style2 = new GUIStyle (GUI.skin.box);
		style1.normal.background = makeTexure (healthBarLength, 20, new Color (0.0f, 1.0f, 0.0f, 1.0f));
		style2.normal.background = makeTexure (maxHealthBarLength-healthBarLength, 20, new Color (1.0f, 0.0f, 0.0f, 1.0f));
	    GUI.Box(new Rect(700, 10, healthBarLength, 20),""+curHealth, style1);
		GUI.Box(new Rect(700+healthBarLength, 10, maxHealthBarLength-healthBarLength, 20), "", style2);
    }

   public void adjustHealth(){		
		curHealth = Mathf.FloorToInt(myBadRobot.hitCount);
		if (curHealth < 0) {
			curHealth = 0;
		}
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
		}
		if (maxHealth < 1) {
			maxHealth = 1;
		}
		healthBarLength = Mathf.FloorToInt(maxHealthBarLength * ((float)curHealth / (float)maxHealth));
  }

	public Texture2D makeTexure(int width, int height, Color col){
		Texture2D result = null;
		if (width > 0 && height > 0) {
			Color[] pix = new Color[width * height];

			for (int i = 0; i < pix.Length; ++i) {
				pix [i] = col;
			}	
			result = new Texture2D (width, height);
			result.SetPixels (pix);
			result.Apply ();
		}
		return result;
	}


}