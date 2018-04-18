using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class Paused : MonoBehaviour
    {
        public bool IsPaused { get; private set; }
        private GameObject Canvas;

        void Start()
        {
            this.IsPaused = false;
            this.Canvas = GameObject.Find("PauseCanvas");
            this.Canvas.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (this.IsPaused)
                {
                    Time.timeScale = 1;
                    this.IsPaused = false;
                    this.Canvas.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    this.IsPaused = true;
                    this.Canvas.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (this.IsPaused)
                {
                    this.IsPaused = false;
                    Time.timeScale = 1;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                }
            }
        }
    }
}
