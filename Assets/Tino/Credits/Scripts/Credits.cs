﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tino
{
    public class Credits : MonoBehaviour
    {
        private float TextHeight;
        private Text Text;
        public float ScrollAmount = 0.5f;

        void Start()
        {
			FindObjectOfType<AudioManager_2>().Play("Win");
            this.Text = this.gameObject.GetComponent<Text>();
            this.TextHeight = -25;
        }

        void Update()
        {
			this.TextHeight += ScrollAmount;
            this.Text.transform.position = new Vector3(this.Text.transform.position.x, this.TextHeight, 0);
            if (this.TextHeight > 1000 || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("xboxStart"))
            {
				FindObjectOfType<AudioSource> ().Stop ();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }
}