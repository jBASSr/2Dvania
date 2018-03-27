using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HPRefill : MonoBehaviour
    {
        public int RefillAmount = 0;

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
            HealthSystem playerHealth = c.gameObject.GetComponent<HealthSystem>();
            if(playerHealth == null)
            {
                return;
            }
            if(playerHealth.RefillHealth(RefillAmount))
            {
                WorldState.TurnOffItem(this.gameObject.scene.name, this.name);
                Destroy(this.gameObject);
            }
        }
    }
}