//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog
//08/22/2019 Colby Peck: Added Init method, removed old constructors, added parameterless constructor 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatState
{
	#region Variables
	protected System.Action<byte> AssignMoveSpeed;
	public virtual void Init(CatManager _catManager) //Because we made the constructor parameterless, any variables that need to be set on initialization must be set manually using methods or properties 
	{
		AssignMoveSpeed = _catManager.AssignMoveSpeed;
	}
	#endregion

	#region Initialization
	public virtual void Enable()
	{
		return;
	}
	#endregion

	#region Main Update
	public abstract void UpdateState();
	#endregion

	#region Public Interface
	public virtual void ChangeSubstate(byte _index)
	{
		return;
	}

	public CatState() { }
	#endregion
}
public class Unalerted : CatState
{
	#region Variables
	private System.Action PatrolMovement;
	#endregion

	#region Initialization
	public Unalerted() { }

	public override void Enable()
	{
		AssignMoveSpeed(0);
	}
	#endregion

	#region Main Update
	public override void UpdateState()
	{
		PatrolMovement();
	}
	#endregion
}
public class Alerted : CatState
{
	#region Variables
	private System.Action<byte> PlaySound;
	private AlertedStates[] availableStates;
	private AlertedStates currentState;
	#endregion

	#region Initialization
	public Alerted()
	{

	}
	public override void Init(CatManager _catManager)
	{
		base.Init(_catManager);
		PlaySound = _catManager.PlaySound;

		availableStates = new AlertedStates[3]
		{
			new Pursuit(ref _catManager),
			new Flee(ref _catManager),
			new Check(ref _catManager)
		};
		currentState = availableStates[0];
	}
	public override void Enable()
	{
		AssignMoveSpeed(1);
		PlaySound(0);
	}
	#endregion

	#region Main Update
	public override void UpdateState()
	{
		currentState.UpdateState();
	}
	#endregion

	#region Public Interface
	public override void ChangeSubstate(byte _index)
	{
		currentState = availableStates[_index];
		currentState.Enable();
	}
	#endregion
}