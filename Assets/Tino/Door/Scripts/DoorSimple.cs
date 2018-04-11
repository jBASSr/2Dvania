using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    public class DoorSimple : MonoBehaviour {

        public Image Black;
        public Animator Animator;

        private GameObject Player;

        private bool IsColliding = false;
        private bool SceneTransitioning = false;

	public string fromScene =null;
	   public string toScene;

	  private string toDoor = null;

	    void Start()
	{
		if (fromScene != null && fromScene == "DesertLevel2") {
			GameManager.door_start = "Door3";
		}
	}
	
	    void Update() {
		    if(!this.IsColliding) { return; }
				if (toScene!=null && toScene != "boss" || GameManager.hasKey == true) {
					this.SceneTransitioning = true;
					StartCoroutine (this.FadeToNewScene (toScene));
				} else {
					Debug.Log ("YOU DONT HAVE THE KEY!!!!!");
				}
	    }

        void OnTriggerEnter2D(Collider2D c)
		{
            if(c.gameObject.tag == "Player")
            {  
			Debug.Log ("Player COllided with door.!!!!!!@!@!");
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

		public IEnumerator FadeToNewScene(string nextScene)
        {
            this.Animator.SetBool("Fade", true);
            yield return new WaitUntil(() => this.Black.color.a == 1);
			SceneManager.LoadScene (toScene, LoadSceneMode.Single);
        }    
}