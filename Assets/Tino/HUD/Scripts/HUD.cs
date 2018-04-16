using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tino
{
    public class HUD : MonoBehaviour
    {
        private const int HpPerBar = 10;
        private Sprite[] HealthBarSprite;
        private List<GameObject> HealthBars;
        private string AmmoText;
        private GameObject[] WeaponObjects = new GameObject[3];
        private GUIStyle AmmoStyle = new GUIStyle();
        private SimpleMovement PlayerSimpleMovement;

        void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                this.PlayerSimpleMovement = player.GetComponent<SimpleMovement>();
            }
            this.HealthBarSprite = Resources.LoadAll<Sprite>("health_sprites");
            this.HealthBars = new List<GameObject>();

            Sprite[] weaponSprites = new Sprite[3];
            weaponSprites[0] = Resources.Load<Sprite>("StandardWeapon");
            weaponSprites[1] = Resources.Load<Sprite>("Missile");
            weaponSprites[2] = Resources.Load<Sprite>("FreezeWeapon");
            for(int i = 0; i < 3; i++)
            {
                this.WeaponObjects[i] = new GameObject();
                this.WeaponObjects[i].AddComponent<SpriteRenderer>();
                SpriteRenderer sr = this.WeaponObjects[i].GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "Player";
                
                sr.sprite = weaponSprites[i];
                this.WeaponObjects[i].transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
                this.WeaponObjects[i].SetActive(false);
            }

            this.AmmoStyle.fontSize = 20;
            this.AmmoStyle.normal.textColor = Color.white;
        }

        void Update()
        {
            this.SetHealth();
            this.DisplayHealth();
            this.DisplayWeapon();
        }

        void OnGUI()
        {
            this.DisplayAmmo();
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
            GameObject player = GameObject.FindWithTag("Player");
            if(player == null) { return; }
            HealthSystem healthSystem = player.GetComponent<HealthSystem>();
            if(healthSystem == null) { return; }
            int playerHealth = healthSystem.health;
            int playerMaxHealth = healthSystem.maxHealth;
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
                    sr.sortingLayerName = "Player";
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

        void DisplayWeapon()
        {   
            Vector3 position = new Vector3(0.02f, 0.93f, 1.0f);

            for(int i = 0; i < 3; i++)
            {
                this.WeaponObjects[i].SetActive(i == this.PlayerSimpleMovement.equippedWeapon ? true : false);
            }
            switch (this.PlayerSimpleMovement.equippedWeapon)
            {
                case 0:
                    this.WeaponObjects[0].transform.position = Camera.main.ViewportToWorldPoint(position);
                    break;
                case 1:
                    this.WeaponObjects[1].transform.position = Camera.main.ViewportToWorldPoint(position);
                    this.AmmoText = this.PlayerSimpleMovement.MissileAmmo.ToString();
                    break;
                case 2:
                    this.WeaponObjects[2].transform.position = Camera.main.ViewportToWorldPoint(position);
                    break;
            }

            Vector3 textPosition = new Vector3(0.05f, 0.93f, 1.0f);
            //this.AmmoText.transform.position = Camera.main.ViewportToWorldPoint(position);
        }

        void DisplayAmmo()
        {
            if(this.PlayerSimpleMovement.equippedWeapon == 1)
            {
                GUI.Label(new Rect(35, 43, 150, 100), this.AmmoText, this.AmmoStyle);
            }
        }
    }
}