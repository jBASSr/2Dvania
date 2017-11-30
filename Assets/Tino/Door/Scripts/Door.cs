using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class Door : MonoBehaviour {

        public string TargetScene;
        public GameObject TargetDoor;

        private GameObject Player;

        private bool IsColliding = false;

	    void Start () { }
	
	    void Update () {
		    if(!this.IsColliding) { return; }

            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(this.TargetScene.Length > 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(this.TargetScene);
                }
                else if(this.TargetDoor != null)
                {
                    this.Player.transform.position = this.TargetDoor.transform.position;
                }
            }
	    }

        void OnTriggerEnter2D(Collider2D c)
        {
            if(c.gameObject.tag == "Player")
            {
                this.Player = c.gameObject;
                this.IsColliding = true;
            }
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.tag == "Player")
            {
                this.IsColliding = false;
            }
        }
    }
}