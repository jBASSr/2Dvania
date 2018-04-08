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
				Tino.WorldState.ComingFromDoor = this.name;
				Tino.WorldState.ComingFromScene = SceneManager.GetActiveScene().name;
				string nextScene = WorldState.GetSceneName();
				Debug.Log ("nextScene=" + nextScene);
				//LOAD boss scene if hasKey
				if (nextScene!=null && nextScene != "boss" || GameManager.hasKey == true) {
					this.SceneTransitioning = true;
					StartCoroutine (this.FadeToNewScene (nextScene));
				} else {
					Debug.Log ("YOU DONT HAVE THE KEY!!!!!");
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
                this.StartDoor = false;
            }
        }

		public IEnumerator FadeToNewScene(string nextScene)
        {
            this.Animator.SetBool("Fade", true);
            yield return new WaitUntil(() => this.Black.color.a == 1);
			SceneManager.LoadScene (nextScene, LoadSceneMode.Single);
        }
    }
}