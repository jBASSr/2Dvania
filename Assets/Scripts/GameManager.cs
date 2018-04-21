using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	public static string from_scene = "level1";
	public static string to_scene = "cave";
	public static bool hasKey = false;
	public static string door_start = "Door1";
	private GameObject robot;
	private GameObject targetDoor;
	public static bool canRoll = false;
	public static float currentHealth = 100.0f;
	//Awake is always called before any Start functions
	void Awake()
	{		
		
		//if (instance == null) {
			instance = this;
		//} else if (instance != this){
		//	Destroy (gameObject);    
	    //}
		DontDestroyOnLoad(gameObject);
    }

	void Start(){
		robot = GameObject.Find ("Robot");
		targetDoor = GameObject.Find (door_start);
		robot.transform.position = targetDoor.transform.position;
	}
}


