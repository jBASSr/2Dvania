using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class DoubleJump : MonoBehaviour
    {

        void OnEnable()
        {
            if (!WorldState.IsItemOn(this.gameObject.scene.name, this.name))
            {
                
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            {
                if (c.gameObject.tag != "Player")
                {
                    FindObjectOfType<AudioManager_2>().Play("Grab");
                    return;
                }
                SimpleMovement playerMovement = c.gameObject.GetComponent<SimpleMovement>();
                if (playerMovement == null)
                {
                    return;
                }
                playerMovement.extraJumps++;
                WorldState.TurnOffItem(this.gameObject.scene.name, this.name);
                Destroy(this.gameObject);
            }
        }
    }
}