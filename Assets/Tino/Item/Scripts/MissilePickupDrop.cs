using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class MissilePickupDrop : MonoBehaviour
    {
        public int Amount = 5;
        
        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag != "Player")
            {
                return;
            }
            SimpleMovement playerMovement = c.gameObject.GetComponent<SimpleMovement>();
            if (playerMovement == null) { return; }

            if (playerMovement.AddMissileAmmo(this.Amount))
            {
                FindObjectOfType<AudioManager_2>().Play("WeaponPU");
                Destroy(this.gameObject);
            }
        }
    }
}