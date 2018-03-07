using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tino
{
    public class LevelControl : MonoBehaviour
    {

        public Image Black;
        public Animator Animator;

        void Start() { }
        
        void Update() { }

        public void NewScene(string sceneName)
        {
            StartCoroutine(this.Fading(sceneName));
        }

        public IEnumerator Fading(string sceneName)
        {
            this.Animator.SetBool("Fade", true);
            yield return new WaitUntil(() => this.Black.color.a == 1);
            SceneManager.LoadScene(sceneName);
        }
    }
}
