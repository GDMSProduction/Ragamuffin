using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck
//               Date: 10/13/2019
//            Purpose: Encapsulate scene loading to a single static class 
// Associated Scripts: GameManager 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog 
// 10/13/2019 Colby Peck: Created script 

public class LevelLoadingManager : MonoBehaviour
{
	public bool printLogs = false;
	public static Scene currentScene;

	public static LevelLoadingManager levelManager = null;

	private void Awake()
	{
		if (levelManager == null)
		{
			levelManager = this;
		}
		if (levelManager != this)
		{
			Destroy(this);
		}
	}

	/// <summary>
	/// Reloads the currently active scene.
	/// </summary>
	public static void ReloadCurrentScene()
	{
		if (levelManager.printLogs)
			Debug.Log("LevelManager.ReloadCurrentScene(): Called!");

		if (currentScene.Equals(null))
		{
			Debug.LogError("LevelManager.ReloadCurrentScene(): Current scene found to be null!");
			return;
		}
	}

	/// <summary>
	/// Loads a scene by its index number. 
	/// </summary>
	/// <param name="index">What's the index of the scene you want to load?</param>
	// This prevents other scripts from needing to use UnityEngine.SceneManagement.
	public static void LoadSceneByIndex(int index)
	{

		if (levelManager.printLogs)
			Debug.Log("LevelManager.LoadSceneByIndex(): Attempting to load scene at index: " + index.ToString());

		try
		{
			SceneManager.LoadScene(index); //attempt to load the scene at the specified index
		}
		catch (System.Exception e) //upon exception,
		{
			if (e is System.IndexOutOfRangeException) //upon IOORE,
			{
				Debug.LogError("LevelManager.LoadSceneByIndex(): No scene found at specified index!"); //tell the console what's happened
			}
			else //upon any other kind of exception,
			{
				Debug.LogError("LevelManager.LoadSceneByIndex(): Unforeseen exception generated: " + e.Message); //tell the console what's happened
			}
		}
	}

	/// <summary>
	/// Loads the next scene in the scene manager. If the current scene is the last scene, it will load the first scene.
	/// </summary>
	public static void LoadNextScene()
	{
		if (levelManager.printLogs)
			Debug.Log("LevelManager.LoadNextScene: Called!");

		if (currentScene.Equals(null))
		{
			Debug.LogError("LevelManager.LoadNextScene(): Current scene found to be null!");
			return;
		}

		if ((currentScene.buildIndex + 2) > SceneManager.sceneCountInBuildSettings) //if our index +2 (index starts at 0, count starts at 1) is greater than the count of total scenes, 
		{
			LoadSceneByIndex(0); //Default to loading the first scene (wraparound) 
			return;
		}
		else
		{
			LoadSceneByIndex(currentScene.buildIndex + 1);
		}

	}

	/// <summary>
	/// Loads the previous scene in the scene manager. If the current scene is the first scene, it will load the last scene.
	/// </summary>
	public static void LoadPreviousScene()
	{
		if (levelManager.printLogs)
			Debug.Log("LevelManager.LoadPreviousScene(): Called!");

		if (currentScene.Equals(null))
		{
			Debug.LogError("LevelManager.LoadPreviousScene(): Current scene found to be null!");
			return;
		}

		if ((currentScene.buildIndex - 1 < 0)) //if we're the first scene, 
		{
			LoadSceneByIndex(SceneManager.sceneCountInBuildSettings - 1); //Load the last scene (wraparound) 
			return;
		}
		else //otherwise, 
		{
			LoadSceneByIndex(currentScene.buildIndex - 1); //Load the previous scene 
		}

	}

	/// <summary>
	/// Loads the next scene by a string containing it's exact name
	/// </summary>
	/// <param name="name">The string of the scene's name</param>
	public static void LoadSceneByName(string name)
	{
		if (levelManager.printLogs)
			Debug.Log("LevelManager.LoadsceneByName(): Attempting to load scene: \'" + name + "\"");

		try
		{
			SceneManager.LoadScene(name);
		}
		catch (System.Exception e)//upon exception,
		{
			if (e is System.ArgumentOutOfRangeException)
			{
				Debug.LogError("LevelManager.LoadSceneByName(): No scene found in build settings with name: \"" + name + "\"!"); //Tell the console what's happened
			}
			else
			{
				Debug.LogError("LevelManager.LoadSceneByName(): Unforeseen exception generated: " + e.Message); //tell the console what's happened
			}
			return;
		}
	}


}
