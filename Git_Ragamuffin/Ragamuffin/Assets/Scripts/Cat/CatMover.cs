//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck
//               Date: 09/28/2019 
//            Purpose: Allow the Cat's AI to percieve, move, and path through the world 
// Associated Scripts: CatManager, CatState, CatAnimatorManager 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
// 09/28/2019 Colby Peck: Created script, brought over the movement dunctionality and variables from CatManager 
// 09/29/2019 Colby Peck: Added variables and functionality for setting destination and starting/stopping movement; filled out FixedUpdate() and Init() 
// 09/29/2019 Colby Peck: Made JumpToY() Coroutine, it will require a bit of refactoring once the jump animation is in 
// 09/29/2019 Colby Peck: Added debug functionality and tested the system. No collision detection or pathing yet, but the cat moves where it's told to 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: 
// [X] Block out movement system 
//		[X] Write empty methods for the functionality we'll need 
//		[X] Figure out what fields and properties will be necessary, add them. Serialize as needed 
//		[X] Make sure states can access the required methods 
// [ ] Build functionality of the Movement system 
//		[X] Make basic functionality that moves the cat to a given destination at a given speed 
//		[ ] Build the cat's methods for perceiving the world 
//			[ ] Collision detection 
//			[ ] Raycats 
//		[ ] Make the pathing as good as needed 

public class CatMover : MonoBehaviour
{
	#region Fields and Properties
	public bool printLogs = false;

	#region Serialized things

	[SerializeField] private float destinationDistance = .5f;   //How close we need to be to our destination before we say that we've reached it 
	public Vector2 destination;// { get; private set; }
	[SerializeField] private float moveSpeed = 1.5f;
	/// <summary>
	/// How fast the cat moves, in units per second 
	/// </summary>
	public float MoveSpeed
	{
		get { return moveSpeed / FixedUpdateMultiplier; }
		set { moveSpeed = value * FixedUpdateMultiplier; }
	}
	#endregion


	public bool Stopped;// { get; set; }

	private CatAnimatorManager animManager;
	private const float FixedUpdateMultiplier = 1f / 50f; //This allows us to convert our units per second values into units per FixedUpdate tick without doing the division every tick. Division is kinda expensive. 
	[SerializeField] //serialized for debugging 
	private bool atDestination;
	/// <summary>
	/// Is the distance between our position and our destination less than our destinationDistance?
	/// </summary>
	public bool AtDestination
	{
		get { return atDestination; }
		private set { Stopped = value; atDestination = value; }
	}

	//We only ever go forwards or backwards along the X axis; this enum will allow us to be more explicit in which directio we're going 
	private enum XDirection : short
	{
		Left = -1,
		Right = 1
	}
	[SerializeField] //serialized for debugging 
	private XDirection xDirection = XDirection.Right; //Default to right 
	#endregion

	#region Initialization/De-Initialization

	public void Init(CatAnimatorManager _anim)
	{
		animManager = _anim;
		AtDestination = true; //Default to no movement 
		moveSpeed *= FixedUpdateMultiplier; //This is so we can input a U/s 
	}
	#endregion

	#region Public interface
	/// <summary>
	/// Sets the cat's destination to a given Vector3
	/// </summary>
	/// <param name="destination">Where are we moving to?</param>
	public void SetDestination(Vector2 _destination)
	{
		// Tell our movement system to move towards the destination. 
		destination = _destination;
		AtDestination = false;

		//if (_destination.x < transform.position.x)
		//{
		//	xDirection = XDirection.Left;
		//}
		//else
		//{
		//	xDirection = XDirection.Right;
		//}
	}
	#endregion

	#region Helper methods

	#region Jumping and Turning
	[SerializeField] //serialized for debugging 
	private bool turning = false;
	private void Turn(XDirection xDir)
	{
		if (!turning)
			StartCoroutine(turn(xDir));
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="xDir">Which direction should we be going along the X axis? Should only be 1 or -1</param>
	public void Turn(int xDir)
	{
		Turn((XDirection)xDir);
	}
	private IEnumerator turn(XDirection xDir)
	{
		if (turning) //If we're already turning,
			yield break; //Arrest this coroutine, we don't want multiple instances running 

		turning = true; //Tell the rest of the code we're turning 
		float y = 0; //Default to y == 0 (facing right) 
		if (xDir == XDirection.Left) //If we're turning left, 
		{
			y = 180; //Our y should be flipped 180 degrees 
		}

		float turnAnimationTime = 0.5f; //default value for now, will need to be replaced with the animation's duration later 
		animManager.PlayTurnAnimation((int)xDir, xDir.ToString()); //Play our animation 
		yield return new WaitForSeconds(turnAnimationTime); //Wait for our animation to finish 
		transform.rotation = new Quaternion(transform.rotation.x, y, transform.rotation.z, transform.rotation.w); //Set our rotation to the proper value 
		xDirection = xDir;

		turning = false; //We've finished turning 
	}


	[SerializeField] //serialized for debugging 
	private bool jumping = false;
	private IEnumerator jumpToY(float y)
	{
		if (jumping) //If we're already jumping, 
			yield break; //Don't start another instance of the coroutine 

		jumping = true;
		animManager.PlayJumpAnimation();
		Stopped = true; //Don't want to move during the animation 
		float animationTime = 1; // default value for now, should be replaced with our jump animation's duration in seconds later 
		yield return new WaitForSeconds(animationTime); //Wait for our animation to finish
		gameObject.transform.position = new Vector2(transform.position.x, y); //Teleport to the appropriate Y value 
		Stopped = false; //Animation is done, we can start moving again
		jumping = false; // We're no longer jumping. 
	}
	public void JumpToY(float y)
	{
		if (!jumping)
			StartCoroutine(jumpToY(y));
	}
	#endregion

	#endregion


	#region Updates
	void FixedUpdate()
	{
		if (atDestination) //If we're at our destination, 
		{
			return; //There's no reason to move 
		}

		if (turning || jumping)//If we're jumping or turning, we don't want to be moving 
		{
			return; //Arrest the method 
		}

		if (Vector2.Distance(transform.position, destination) <= destinationDistance) //Do the distance check before anything else 
		{
			AtDestination = true; //Tell the rest of the code that we're at our destination 
			return; //We shouldn't be moving if we're at our destination 
		}
		if (!Stopped)
		{
			if (Mathf.Abs(transform.position.y - destination.y) > .1f)
			{
				if (!jumping) //If we're not already jumping, 
				{
					JumpToY(destination.y); //Jump to the y value 
					return; //Stop this tick 
				}
			}

			if (transform.position.x < destination.x)
			{
				if (xDirection != XDirection.Right) //if our destinatoin is to the right and we're not already facing right, 
				{
					if (!turning) //If we aren't already turning, 
					{
						Turn(XDirection.Right); //Turn to face right
						return;
					}
				}
			}
			else
			{
				if (xDirection != XDirection.Left) //If we're not on the right, we're on the left. If we're not facing left, 
				{
					if (!turning) //If we're not already turning, 
					{
						Turn(XDirection.Left); //Turn to face left 
						return;
					}
				}
			}

			if (!jumping && !turning) //If we aren't jumping or turning, 
			{
				//if (printLogs)
				//	Debug.Log("Translating along X: " + moveSpeed*(short)xDirection);

				transform.Translate(moveSpeed * (short)xDirection, 0, 0,Space.World); //Move along the X axis according to our speed and intended direction 
			}
		}
	}
	#endregion
}
