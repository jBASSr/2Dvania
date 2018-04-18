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

        private GameObject Player;

        private bool IsColliding = false;
        private bool SceneTransitioning = false;
        private bool StartDoor = false;

	    void Start()
        {
            if(WorldState.GetDoorName() == this.gameObject.name)
            {
                this.StartDoor = true;
            }
        }
	
	    void Update() {
		    if(!this.IsColliding) { return; }
            if(!this.SceneTransitioning && !this.StartDoor)
            {
                this.SceneTransitioning = true;
                StartCoroutine(this.FadeToNewScene());
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
                this.StartDoor = false;
            }
        }

        public IEnumerator FadeToNewScene()
        {
            this.Animator.SetBool("Fade", true);
            yield return new WaitUntil(() => this.Black.color.a == 1);
            Tino.WorldState.ComingFromDoor = this.name;
            Tino.WorldState.ComingFromScene = SceneManager.GetActiveScene().name;
            string nextScene = WorldState.GetSceneName();
            if(nextScene != null)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }
    }
}