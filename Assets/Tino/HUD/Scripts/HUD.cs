using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HUD : MonoBehaviour
    {

        private GameObject Player;
        private Sprite HealthBarSprite;
        private List<GameObject> HealthBars;

        void Start()
        {
            this.Player = GameObject.FindWithTag("Player");
            this.HealthBarSprite = Resources.Load<Sprite>("HealthSprite");
            this.HealthBars = new List<GameObject>();
        }

        void Update()
        {
            this.SetHealth();
            this.DisplayHealth();
        }

        void DisplayHealth()
        {
            for(int i = 0; i < this.HealthBars.Count; i++)
            {
                Vector3 position = new Vector3(0.02f + (i*0.03f), 0.97f, 1.0f);
                this.HealthBars[i].transform.position = 
                    Camera.main.ViewportToWorldPoint(position);
            }
        }

        void SetHealth()
        {
            //Tino.Health playerHealth = this.Player.GetComponent<Tino.Health>();
            int playerHealth = 100;
            while (this.HealthBars.Count < (playerHealth / 10f))
            {
                GameObject hBar = new GameObject();
                hBar.AddComponent<SpriteRenderer>();
                SpriteRenderer sr = hBar.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = this.HealthBarSprite;
                }
                hBar.SetActive(true);

                this.HealthBars.Add(hBar);
            }
            while(this.HealthBars.Count > (playerHealth / 10f))
            {
                this.HealthBars.RemoveAt(this.HealthBars.Count - 1);
            }
        }
    }

}