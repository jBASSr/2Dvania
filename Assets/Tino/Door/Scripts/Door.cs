using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tino
{
    public class Door : MonoBehaviour {

        public Image Black;
        public Animator Animator;
        public string TargetScene;
        public GameObject TargetDoor;

        private GameObject Player;

        private bool IsColliding = false;

	    void Start() { }
	
	    void Update() {
		    if(!this.IsColliding) { return; }

            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (this.TargetScene.Length > 0)
                {
                    StartCoroutine(this.FadeToNewScene());
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

        public IEnumerator FadeToNewScene()
        {
            this.Animator.SetBool("Fade", true);
            yield return new WaitUntil(() => this.Black.color.a == 1);
            Tino.WorldState.ComingFromDoor = this.name;
            Tino.WorldState.ComingFromScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(this.TargetScene, LoadSceneMode.Single);
        }
    }
}