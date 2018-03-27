using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource BGM;
    // Use for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
        }   
    }

    // Updatecalled once per frame
    void Update()
    {

    }

    public void changeBGM(AudioClip music)
    {
        if (BGM.clip.name == music.name)
            return;

        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
}
