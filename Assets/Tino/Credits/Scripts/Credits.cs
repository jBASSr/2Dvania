using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tino
{
    public class Credits : MonoBehaviour
    {
        private float TextHeight;
		private Text myText;
        public float ScrollAmount = 0.5f;

        void Start()
        {
			//FindObjectOfType<AudioManager_2>().Play("Win");
			myText = GetComponent<Text>();
			Debug.LogError("myText=" + myText);
            TextHeight = -25;
        }

        void Update()
        {
			TextHeight += ScrollAmount;
			myText.transform.position = new Vector3(myText.transform.position.x, TextHeight, 0);
            if (TextHeight > 1000 || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("xboxStart"))
            {
				FindObjectOfType<AudioSource> ().Stop ();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }
}