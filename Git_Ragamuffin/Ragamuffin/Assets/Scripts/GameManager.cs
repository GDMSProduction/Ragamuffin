//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: ???
//            Purpose: To act as the main Game Manager for Ragamuffin. I just laid out some basic functions like Quit, and set up scene management.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog 
// 08/22/2019 Colby Peck: Added static field and property of the Player GameObject to avoid using any GameObject.Find methods elsewhere 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	////######## VARIABLES ########////

	public static GameManager gameManager = null;
	public Scene sceneCurrent;

	private static GameObject player = null;
	public static GameObject Player
	{
		get
		{
			if (player == null)
			{
				player = GameObject.FindGameObjectWithTag("Player");
			}
			return player;
		}
	}
	////######## PREBUILT METHODS ########////

	void Awake()
	{
		if (!gameManager)
		{
			gameManager = this;
		}
		if (gameManager != this)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}

	////######## CUSTOM METHODS ########////

	public void QuitGame()
	{
		Application.Quit();
	}

	// SCENE MANAGMENT //
	public void LoadNextScene()
	{
		SceneManager.LoadScene(gameManager.sceneCurrent.buildIndex + 1);
	}

	public void LoadPreviousLevel() // FOR DEBUG ONLY
	{
		SceneManager.LoadScene(gameManager.sceneCurrent.buildIndex - 1);
	}

	public void LoadSceneByName(string _levelName)
	{
		SceneManager.LoadScene(_levelName);
	}

	public void LoadSceneByIndex(int _levelNumber)
	{
		SceneManager.LoadScene(_levelNumber);
	}

}
