using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tino
{
    public class Credits : MonoBehaviour
    {
        private float TextHeight;
        private Text Text;
        public float ScrollAmount;

        void Start()
        {
            this.Text = this.gameObject.GetComponent<Text>();
            this.TextHeight = -25;
        }

        void Update()
        {
            this.TextHeight += this.ScrollAmount;
            this.Text.transform.position = new Vector3(this.Text.transform.position.x, this.TextHeight, 0);
            if (this.TextHeight > 1000 || Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }
}