using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager_2 : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager_2 instance;
	public AudioSource BGM;
	private AudioClip myClip;
    // Use this for initialization
    void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //{
         //   Destroy(gameObject);
         //   return;
        //}
        //DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
		myClip = GetComponent<AudioSource> ().clip;
		playBGM (myClip);
		//changeBGM()
   }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "NOT FOUND!!!!");
            return;
        }
        s.source.Play();
    }

	public void changeBGM(AudioClip music)
	{

	}

	public void playBGM(AudioClip music)
	{
		if (BGM.clip.name == music.name)
			return;

		BGM.Stop();
		BGM.clip = music;
		BGM.Play();
	}
}
