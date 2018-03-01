using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class HPRefill : MonoBehaviour
    {
        public int RefillAmount = 0;

        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag != "Player")
            {
                return;
            }

            //TODO: Try to increase player's health. Need to merge with Jose first.

            Destroy(this.gameObject);
        }
    }
}