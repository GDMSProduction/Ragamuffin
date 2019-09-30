//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck
//               Date: 09/28/2019 
//            Purpose: Manage all the cat's scripted runtime animations, prevent CatManager and CatMover from bloating 
// Associated Scripts: CatManager, CatState, CatMover 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
// 09/29/2019 Colby Peck: Created script; added methods that will be needed, but no functionality can be made until animations are in. They just print debug logs for now. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorManager : MonoBehaviour
{
	public bool printLogs = true; //Make this default to false after animations are in. For now, it's needed to know that the methods are getting called correctly 

	private Animator anim;


	public void Init()
	{
		anim = GetComponentInParent<Animator>();
	}


	/// <param name="direction">Which direction should we be going along the X axis? Should only be 1 or -1</param>
	/// <param name="directionMessage">Debug message to log </param>
	public void PlayTurnAnimation(int direction, string directionMessage = null) //Remove the directionMessage later, it's only there for debugging purposes 
	{
		if (printLogs)
			Debug.Log("CatAnimatorManager: turning " + directionMessage + "; animation not yet implemented");
	}

	public void PlayJumpAnimation()
	{
		if (printLogs)
			Debug.Log("CatAnimatorManager: Jumping animation not yet implemented");
	}

}
