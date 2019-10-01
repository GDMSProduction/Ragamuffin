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
	//Patrols between set points, changes to pursuit state if he sees rag.Needs functionality for returning to patrol from other states 
	public override void Tick()
	{
		//Parent.Mover.SetDestination(new Vector3(1,1,1));
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

public class Cat_Teleport
{
	//Cat is meant to move to a point in front of the player, but he’s behind the player. Play an animation of the cat running across the foreground, teleport the cat machine then set the cat to the patrol state at its new location.
}