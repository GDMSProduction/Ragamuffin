//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog
// 08/22/2019 Colby Peck: Added Init method, removed old constructors, added parameterless constructor 
// 09/14/2019 Colby Peck: Deleted CatSubstate.cs; moved all substates into this script 
// 09/14/2019 Colby Peck: Refactored CatState heavily; removed all broken functionality from existing states 
// 09/14/2019 Colby Peck: Added State base class, CatState child class, and empty classes for each state that will be needed 
// 10/07/2019 Colby Peck: Filled out Patrol State. has no exit conditions yet, will add them when the states the cat would exit to aren't empty. 
// 10/07/2019 Colby Peck: Tested basic functionality of patrol state. It works as intended!


// [ ] Make states' basic functionality: 
//		[X] Patrol 
//		[ ] Alerted 
//		[ ] Pusuit 
//		[ ] Dazed 
//		[ ] Flee 
//		[ ] Teleport 

// [ ] Test states' basic functionality: 
//		[X] Patrol 
//		[ ] Alerted 
//		[ ] Pusuit 
//		[ ] Dazed 
//		[ ] Flee 
//		[ ] Teleport 

// [ ] Add exit conditions: 
//		[ ] Patrol 
//		[ ] Alerted 
//		[ ] Pusuit 
//		[ ] Dazed 
//		[ ] Flee 
//		[ ] Teleport 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	#region Variables and Properties
	protected StateMachine parent;
	/// <summary>
	/// The CatManager this state is attached to 
	/// </summary>
	//protected virtual StateMachine Parent { get { return parent; } set { parent = value; } }
	protected bool ExitConditions() { return false; } //Might not end up using this; remove if proves to be unhelpful 

	#endregion

	#region Constructor
	/// <summary>
	/// Constructor <b>must</b> take no parameters
	/// </summary>
	public State() { }
	#endregion

	#region Methods
	#region Initialization/De-Initialization
	/// <summary>
	/// This is called when this state is added to a CatManager machine
	/// </summary>
	/// <param name="_parent">What CatManager are we being attached to?</param>
	//Because we made the constructor parameterless, any variables that need to be set on initialization must be set manually using methods or properties
	public virtual void Init(StateMachine _parent)
	{
		parent = _parent;
		if (parent == null)
		{
			Debug.LogError(GetType().ToString() + ": Parent found to be null");
		}
	}



	/// <summary>
	/// This method is called whenever this state is changed to
	/// </summary>
	public virtual void Enable() { }
	/// <summary>
	/// This method is called whenever the CatManager changes to a different state
	/// </summary>
	public virtual void Disable() { }
	#endregion

	#region Updates
	/// <summary>
	/// Called in Update
	/// </summary>
	public virtual void Tick() { }

	/// <summary>
	/// Called in FixedUpdate
	/// </summary>
	public virtual void FixedTick() { }
	#endregion
	#endregion
}

public abstract class CatState : State
{
	protected CatManager Parent { get { return (CatManager)base.parent; } set { parent = value; } } //Declare our parent as a CatManager so we can access all its specific stuff 

	//Need to refactor Init to set our parent correctly 
	public void Init(CatManager _parent)
	{
		Parent = _parent;

		//Bit of error checking is always good 
		if (!(Parent is CatManager)) //If our parent isn't a CatManager, 
		{
			Debug.LogError(GetType().ToString() + ": Parent not CatManager!"); //Yell at the console about it 
		}
	}
}
public class Cat_Patrol : CatState
{
	float timer = 0;
	bool goingBackwards = false;
	public override void Enable()
	{
		Parent.Mover.SetDestination(Parent.CurrentPoint.Position);
	}

	//Patrols between set points, changes to pursuit state if he sees rag.Needs functionality for returning to patrol from other states 
	public override void Tick() //Every frame, 
	{
		if (Parent.Mover.AtDestination) //If we're at our destination, 
		{
			timer += Time.deltaTime; //Start our timer 

			if (timer > Parent.PatrolWaitTime) //If our timer exceeds the amount of time we're supposed to wait at a given patrol point, 
			{
				Parent.CurrentPoint = NextPoint(); //Set our current point to the next one 
				Parent.Mover.SetDestination(Parent.CurrentPoint.Position); //Set our parent's destination to the current point 
				timer = 0; //reset the timer 
			}
		}
	}

	CatPatrolPoint NextPoint()
	{
		if(!goingBackwards) //If we aren't going backwards, 
		{
			if(Parent.CurrentPoint.nextPoint == null) //If the next point doesn't exist, 
			{
				if(Parent.ReversePatrolWhenDone) //If we're supposed to go backwards when we reach the last checkpoint, 
				{
					goingBackwards = true; //We're going backwards 
				}
				else //Otherwise, 
				{
					return Parent.FirstPoint; //We're supposed to go back to the first point and start the patrol over. 
				}
			}
		}
		else //If we ARE going backwards, 
		{
			if(Parent.CurrentPoint.previousPoint == null) //If the previous checkpoint doesn't exist, 
			{
				goingBackwards = false; //We've reached the end, we should stop going backwards 
			}
		}

		//This chunk has to be here AFTER the null checking so every code path returns a value. 
		if(!goingBackwards) //If we aren't going backwards, 
		{
			return Parent.CurrentPoint.nextPoint; //return the next point 
		}
		else //If we ARE going backwards, 
		{
			return Parent.CurrentPoint.previousPoint; //return the previous point 
		}

	}

}


public class Cat_Alerted : CatState
{
	//Player makes a noise, causing the cat to come investigate.  
}

public class Cat_Pursuit : CatState
{
	//Cat sees player, is currently chasing them. Upon getting close enough, the cat will begin attacking the player. If the player successfully counter-attacks, the cat will become dazed. 
}

public class Cat_Dazed : CatState
{
	// Remains dazed for a given amount of time, becomes undazed if he goes off-screen. Reverts to patrol state when exiting state. 
}

public class Cat_Flee : CatState
{
	//Cat is hurt from some source: runs away to a given location, stays there for a given amount of time, then reverts to patrol state. 
}

public class Cat_Teleport : CatState
{
	//Cat is meant to move to a point in front of the player, but he’s behind the player. Play an animation of the cat running across the foreground, teleport the cat machine then set the cat to the patrol state at its new location.
}