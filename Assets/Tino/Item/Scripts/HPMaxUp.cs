using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HPMaxUp : MonoBehaviour
    {
        public int MaxUpAmount = 10;

        void Start() { }

        void Update() { }

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
            Destroy(this.gameObject);
        }
    }
}