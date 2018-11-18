using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatState
{
    #region Variables
    protected System.Action<byte> AssignMoveSpeed;
    #endregion

    #region Initialization
    public CatState(CatManager _catManager) { AssignMoveSpeed = _catManager.AssignMoveSpeed; }
    public virtual void Enable() { return; }
    #endregion

    #region Main Update
    public abstract void UpdateState();
    #endregion

    #region Public Interface
    public virtual void ChangeSubstate(byte _index) { return; }
    #endregion
}
public class Unalerted : CatState
{
    #region Variables
    private System.Action PatrolMovement;
    #endregion

    #region Initialization
    public Unalerted(CatManager _catManager) : base(_catManager) { PatrolMovement = _catManager.PatrolMovement; }
    public override void Enable() { AssignMoveSpeed(0); }
    #endregion

    #region Main Update
    public override void UpdateState() { PatrolMovement(); }
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
    public Alerted(CatManager _catManager) : base(_catManager)
    {
        PlaySound = _catManager.PlaySound;
        availableStates = new AlertedStates[3] { new Pursuit(ref _catManager), new Flee(ref _catManager), new Check(ref _catManager) };
        currentState = availableStates[0];
    }
    public override void Enable()
    {
        AssignMoveSpeed(1);
        PlaySound(0);
    }
    #endregion

    #region Main Update
    public override void UpdateState() { currentState.UpdateState(); }
    #endregion

    #region Public Interface
    public override void ChangeSubstate(byte _index)
    {
        currentState = availableStates[_index];
        currentState.Enable();
    }
    #endregion 
}