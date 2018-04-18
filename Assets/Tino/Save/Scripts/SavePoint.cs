using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class SavePoint : MonoBehaviour
    {
        private Sprite[] lampSprite;
        private bool IsColliding = false;

        void Start()
        {
            lampSprite = Resources.LoadAll<Sprite>("save_lamp");
        }

        void OnEnable()
        {   
        }

        void Update()
        {
            if(!this.IsColliding) { return; }

            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Save.SaveLoadGame.SaveGame();
                SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                renderer.sprite = lampSprite[0];
            }
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag == "Player")
            {
                this.IsColliding = true;
            }
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if(c.gameObject.tag == "Player")
            {
                this.IsColliding = false;
            }
        }
    }
}
