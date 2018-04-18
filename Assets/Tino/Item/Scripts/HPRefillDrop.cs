using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HPRefillDrop : MonoBehaviour
    {
        public int RefillAmount = 0;

        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag != "Player")
            {
                return;
            }
            HealthSystem playerHealth = c.gameObject.GetComponent<HealthSystem>();
            if (playerHealth == null) { return; }
            if (playerHealth.RefillHealth(RefillAmount))
            {
                Destroy(this.gameObject);
            }
        }
    }
}

