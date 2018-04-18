using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tino
{
    [System.Serializable]
    public class Dialogue : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        private GameObject Canvas;
        private string[] Lines;
        private List<string> CurrentLines;
        public Text Text;
        private bool Queued;

        void Start()
        {
            this.Queued = false;
            this.IsActive = false;
            this.Canvas = GameObject.Find("DialogueCanvas");
            this.Canvas.SetActive(false);
        }

        void Update()
        {
            if(this.Queued)
            {
                Time.timeScale = 0;
                this.CurrentLines = new List<string>(this.Lines);
                this.IsActive = true;
                this.Canvas.SetActive(true);
            }

            if(this.IsActive)
            { 
                if (Input.GetKeyDown(KeyCode.Space) || this.Queued || Input.GetButtonDown("xboxA"))
                {
                    this.Queued = false;
                    if (this.CurrentLines.Count <= 0)
                    {
                        Time.timeScale = 1;
                        this.Canvas.SetActive(false);
                        this.IsActive = false;
                        return;
                    }
                    this.Text.text = this.CurrentLines[0];
                    this.CurrentLines.RemoveAt(0);
                }
            }
        }

        private void QueueLines(string[] lines)
        {
            if(lines.Length > 0)
            {
                this.Queued = true;
                this.Lines = lines;
            }
        }

        static public Dialogue GetDialogue()
        {
            GameObject d = GameObject.Find("Dialogue");
            if(d == null) { return null; }
            return d.GetComponent<Dialogue>();
        }

        static public void Queue(string[] lines)
        {
            Dialogue d = Dialogue.GetDialogue();
            if(d != null)
            {
                d.QueueLines(lines);
            }
        }
    }
}
