using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HPMaxUp : MonoBehaviour
    {
        public int MaxUpAmount = 10;

        void OnEnable()
        {
            if(!WorldState.IsItemOn(this.gameObject.scene.name, this.name))
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
            HealthSystem playerHealth = c.gameObject.GetComponent<HealthSystem>();
            if (playerHealth == null)
            {
                return;
            }
            playerHealth.MaxHealthIncrease(MaxUpAmount);
            playerHealth.RefillHealth(playerHealth.maxHealth);

            Dialogue.Queue(new string[] { "Max. Health increased by 5" });
            
            WorldState.TurnOffItem(this.gameObject.scene.name, this.name);
            Destroy(this.gameObject);
        }
    }
}