using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scroll : MonoBehaviour {

    public Vector2 Speed = new Vector2(2, 2);
    public Vector2 Direction = new Vector2(-1, 0);

    public bool IsLinkedToCamera = false;
    public bool IsLooping = false;
    private List<SpriteRenderer> BackgroundPart;

	// Use this for initialization
	void Start () {
		if(this.IsLooping)
        {
            this.BackgroundPart = new List<SpriteRenderer>();
            for(int i = 0; i< this.transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();
                if(r != null)
                {
                    this.BackgroundPart.Add(r);
                }
            }

            this.BackgroundPart = this.BackgroundPart.OrderBy(t => t.transform.position.x).ToList();
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 movement = new Vector3(this.Speed.x * this.Direction.x, this.Speed.y * this.Direction.y, 0);
        movement *= Time.deltaTime;
        this.transform.Translate(movement);
        if(this.IsLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        if(this.IsLooping)
        {
            SpriteRenderer firstChild = this.BackgroundPart.FirstOrDefault();
            if(firstChild != null)
            {
                if(firstChild.transform.position.x < Camera.main.transform.position.x)
                {
                    if(firstChild.IsVisibleFrom(Camera.main) == false)
                    {
                        SpriteRenderer lastChild = this.BackgroundPart.LastOrDefault();
                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = lastChild.bounds.max - lastChild.bounds.min;
                        firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x, firstChild.transform.position.y, firstChild.transform.position.z);
                        this.BackgroundPart.Remove(firstChild);
                        this.BackgroundPart.Add(firstChild);
                    }
                }
            }
        }
	}
}
