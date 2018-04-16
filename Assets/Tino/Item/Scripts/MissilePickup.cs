using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class MissilePickup : MonoBehaviour
    {
        public int Amount = 5;

        void OnEnable()
        {
            if (!WorldState.IsItemOn(this.gameObject.scene.name, this.name))
            {

                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag != "Player")
            {
                return;
            }
            SimpleMovement playerMovement = c.gameObject.GetComponent<SimpleMovement>();
            if (playerMovement == null) { return; }

            if(playerMovement.AddMissileAmmo(this.Amount))
            {
                WorldState.TurnOffItem(this.gameObject.scene.name, this.name);
                FindObjectOfType<AudioManager_2>().Play("Grab");
                Destroy(this.gameObject);
            }
        }
    }
}