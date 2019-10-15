//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: ???
//            Purpose: A quick prototype script for returning the player to the main menu. Non-Modular. Used for Proof Of Concept Build.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu_Prototype : MonoBehaviour
{
	GameManager gameManager;

	void Start ()
	{
		gameManager = GameManager.gameManager;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Collider: " + collision + " & Tag: " + collision.gameObject.tag);


		if (collision.gameObject.CompareTag("Player"))
		{
			LevelLoadingManager.LoadSceneByIndex(0);
		}
	}
}
