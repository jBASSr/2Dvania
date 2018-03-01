using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HUD : MonoBehaviour
    {
        private const int HpPerBar = 10;
        private GameObject Player;
        private Sprite[] HealthBarSprite;
        private List<GameObject> HealthBars;

        void Start()
        {
            this.Player = GameObject.FindWithTag("Player");
            this.HealthBarSprite = Resources.LoadAll<Sprite>("health_sprites");
            Debug.Log(this.HealthBarSprite.GetLength(0));
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
                Vector3 position = new Vector3(0.02f + (i*0.015f), 0.97f, 1.0f);
                this.HealthBars[i].transform.position = 
                    Camera.main.ViewportToWorldPoint(position);
                this.HealthBars[i].transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            }
        }

        void SetHealth()
        {
            int playerHealth = 60;
            int playerMaxHealth = 100;
            int healthBars = Mathf.FloorToInt(playerMaxHealth / HUD.HpPerBar);
            int filledBars = Mathf.FloorToInt(playerHealth / HUD.HpPerBar);
            while (this.HealthBars.Count < healthBars)
            {
                GameObject hBar = new GameObject();
                hBar.AddComponent<SpriteRenderer>();
                SpriteRenderer sr = hBar.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = this.HealthBarSprite[3];
                }
                hBar.layer = 10;
                hBar.SetActive(true);

                this.HealthBars.Add(hBar);
            }
            while(this.HealthBars.Count > healthBars)
            {
                this.HealthBars.RemoveAt(this.HealthBars.Count - 1);
            }

            for(int i = 0; i < this.HealthBars.Count; i++)
            {
                int idx = 0;
                if(i < filledBars)
                {
                    idx = i == 0 ? 0 : 2;
                    
                }
                else
                {
                    idx = i == 0 ? 1 : 3;
                }
                SpriteRenderer sr = this.HealthBars[i].GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = this.HealthBarSprite[idx];
                }
            }
        }
    }

}