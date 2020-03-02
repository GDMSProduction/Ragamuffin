//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck 
//               Date: 10/15/2019 
//            Purpose: Store level-specific info like checkpoints, talk with the GameManager for level initialization and checkpoint loading/setting 
// Associated Scripts: Checkpoint, GameManager 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog 
// 10/15/2019 Colby Peck: Created the script; made it talk with the checkpoint and game manager scripts 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
	public static LevelInfo currLevelInfo;

	/// <summary>
	/// This event is called after the current level info initializes
	/// </summary>
	public static System.Action LevelInit;


	public Checkpoint[] levelCheckpoints;

	public void Init()
	{
		currLevelInfo = this;
		LevelInit.Invoke();
	}

	void CheckpointActivated(Checkpoint activeCheckpoint)
	{
		bool containsCheckpoint = false; //Do we have the checkpoint in our array? Default to false. 

		for (int i = 0; i < levelCheckpoints.Length; i++) //for each of our checkpoints, 
		{
			if (levelCheckpoints[i].Equals(activeCheckpoint)) //If the checkpoint is the activated checkpoint, 
			{
				containsCheckpoint = true; //Our array contains the checkpoint 
				GameManager.gameManager.SetSavedCheckpointIndex(i); //Set the saved checkpoint index in the game manager accordingly 
			}
		}

		if (!containsCheckpoint) //If we don't have the activated checkpoint 
		{
			//Tell the console what's happened, 
			Debug.LogError("SceneInfo.CheckpointActivated(): Activated checkpoint not found in the serialized array!\nAll level checkpoints must be added to the array for game loading to work correctly!"); 
			return; //Arrest the code 
		}
	}
}
