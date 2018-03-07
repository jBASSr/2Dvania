using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class DoubleJump : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.tag != "Player")
            {
                return;
            }
            SimpleMovement playerMovement = c.gameObject.GetComponent<SimpleMovement>();
            if (playerMovement == null)
            {
                return;
            }
            playerMovement.extraJumps++;
            Destroy(this.gameObject);
        }
    }
}