//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: ???
//            Purpose: To act as the main Game Manager for Ragamuffin. I just laid out some basic functions like Quit, and set up scene management.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	////######## VARIABLES ########////

	public static GameManager gameManager = null;
	public Scene sceneCurrent;

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
