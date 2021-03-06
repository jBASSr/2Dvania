using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class PlayerHUD : MonoBehaviour
	{
	    private int maxHealthBarLength;
	    private int healthBarLength;
	    public float maxHealth = 100.0f;
	    public float curHealth;
	    private GameObject robot;
   	    private GUIStyle style1, style2;

		void Start()
		{
		curHealth = maxHealth;
		robot = GameObject.Find ("Robot");
		maxHealthBarLength = Screen.width / 6;
		healthBarLength = maxHealthBarLength;
	    }

		void Update()
		{
   		    updateHealth ();
		}

	public void adjustHealth(float adj){
		curHealth += adj;
	}

	void OnGUI(){			  
		style1 = new GUIStyle (GUI.skin.box);
		style2 = new GUIStyle (GUI.skin.box);
		style1.normal.background = makeTexure (healthBarLength, 20, new Color (0.0f, 0.8f, 0.0f, 1.0f));
		style2.normal.background = makeTexure (maxHealthBarLength-healthBarLength, 20, new Color (1.0f, 0.0f, 0.0f, 1.0f));
	    GUI.Box(new Rect(10, 10, healthBarLength, 20),""+curHealth, style1);
		GUI.Box(new Rect(10+healthBarLength, 10, maxHealthBarLength-healthBarLength, 20), "", style2);
    }

   public void updateHealth(){		
		if (curHealth < 0) {
			curHealth = 0;
			UnityEngine.SceneManagement.SceneManager.LoadScene("YouLose");
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