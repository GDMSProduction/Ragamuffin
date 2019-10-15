//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: ???
//            Purpose: To act as the main Game Manager for Ragamuffin. I just laid out some basic functions like Quit, and set up scene management.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog 
// 08/22/2019 Colby Peck: Added static field and property of the Player GameObject to avoid using any GameObject.Find methods elsewhere 
// 10/15/2019 Colby Peck: Added a bunch of methods for loading scenes and saving/loading the game 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Fields/Properties
	public static GameManager gameManager = null;

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

	private static SaveInfo saveInfo;
	private bool loadingSavedGame = false;
	#endregion

	#region Methods
	#region Initialization/De-Initialization
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

		if (saveInfo == null)
		{
			saveInfo = new SaveInfo();
		}

		#region Event Subscriptions

		//Pair subscriptions with desubscriptions to avoid duplicate subsctriptions 
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneLoaded += OnSceneLoaded;

		LevelInfo.LevelInit -= OnLevelInit;
		LevelInfo.LevelInit += OnLevelInit;
		#endregion
	}

	void OnDestroy()
	{
		#region Event Desubscriptions
		//When this object is destroyed, make sure to unsubscribe our delegates from any events or they'll try to call nonexistant methods and throw an NRE 
		SceneManager.sceneLoaded -= OnSceneLoaded;
		LevelInfo.LevelInit -= OnLevelInit;
		#endregion
	}

	#endregion

	#region Save/Load stuff

	public void LoadSavedGame()
	{
		saveInfo = SaveSystem.LoadGame(); //Load our save info 
		if (saveInfo == null)
		{
			Debug.LogError("GameManager.LoadSavedGame(): Saved game found to be null! Returning!");
			return;
		}
		loadingSavedGame = true;
		LevelLoadingManager.LoadSceneByIndex(saveInfo.levelIndex);
	}

	public void SaveGame()
	{
		SaveSystem.SaveGame(saveInfo);
	}

	#region Event Listeners
	void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
	{
		LevelLoadingManager.currentScene = loadedScene;
		saveInfo.levelIndex = loadedScene.buildIndex;
		try
		{
			LevelInfo.currLevelInfo = FindObjectOfType<LevelInfo>();
			LevelInfo.currLevelInfo.Init();
		}
		catch (System.Exception e)
		{
			if (e is System.NullReferenceException)
			{
				Debug.LogError("GameManager.OnSceneLoaded(): Couldn't find an active LevelInfo in the scene! Did you forget to add one?");
			}
			else
			{
				Debug.LogError("GameManager.OnSceneLoaded(): Unforeseen excpetion generated!\n" + e.Message);
			}
		}
	}

	/// <summary>
	/// Does things dependent upon the current LevelInfo being initialized first
	/// </summary>
	public void OnLevelInit()
	{
		try
		{
			if (loadingSavedGame)
			{
				Checkpoint.SetCurrentCheckpoint(LevelInfo.currLevelInfo.levelCheckpoints[saveInfo.levelIndex]);

				loadingSavedGame = false;
			}
		}
		catch (System.Exception e)
		{
			//error report
			Debug.LogError("GameManager.OnLevelInit(): Unforeseen exception generated:\n" + e.Message);
			return;
		}
	}

	public void SetSavedCheckpointIndex(int index)
	{
		saveInfo.checkpointIndex = index;
	}
	#endregion

	#endregion

	public void QuitGame()
	{
#if UNITY_ENGINE
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	#endregion
}
