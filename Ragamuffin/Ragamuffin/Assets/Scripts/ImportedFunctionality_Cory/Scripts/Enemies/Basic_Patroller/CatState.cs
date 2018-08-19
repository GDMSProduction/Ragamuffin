using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatState
{
    #region Initialization
    public virtual void Enable() { return; }
    #endregion

    #region Main Update
    public abstract void UpdateState();
    #endregion

    #region Public Interface
    public virtual void ChangeCatSubState(byte _index) { return; }
    public virtual byte GetStateIndex() { return 0; }
    #endregion
}
public class Unalerted : CatState
{
    #region Variables
    private CatSubState[] availableStates;
    private CatSubState currentState;
    private System.Action<byte> AssignMoveSpeed;
    private System.Action<byte> PrintState;
    #endregion

    #region Initialization
    public Unalerted(CatManager _catManager)
    {
        AssignMoveSpeed = _catManager.AssignMoveSpeed;
        PrintState = _catManager.PrintState;
        availableStates = new CatSubState[2] { new Idle(ref _catManager), new Patrol(ref _catManager) };

        // Puts cat into idle or patrol from the beginning
        byte randomSubState = (byte)Random.Range(0, 2);
        ChangeCatSubState(randomSubState);

    }
    public override void Enable()
    {
        AssignMoveSpeed(0);
        ChangeCatSubState(1);
    }
    #endregion

    #region Public Interface
    public override void ChangeCatSubState(byte _index)
    {
        currentState = availableStates[_index];
        currentState.Enable();
        PrintState(GetStateIndex());
    }
    public override byte GetStateIndex()
    {
        if (currentState == availableStates[0])
            return 0;
        else
            return 1;
    }
    public override void UpdateState() { currentState.UpdateState(); }
    #endregion
}
public class Alerted : CatState
{
    #region Variables
    private CatSubState[] availableStates;
    private CatSubState currentState;
    private System.Action<byte> AssignMoveSpeed;
    private System.Action AssignRandomPatrolTime;
    private System.Action<byte> PrintState;
    #endregion

    #region Initialization
    public Alerted(CatManager _catManager)
    {
        PrintState = _catManager.PrintState;
        availableStates = new CatSubState[2] { new Pursuit(ref _catManager), new Attack(ref _catManager) };
        currentState = availableStates[0];
        AssignMoveSpeed = _catManager.AssignMoveSpeed;
        AssignRandomPatrolTime = _catManager.AssignRandomPatrolTime;
    }
    public override void Enable() { AssignMoveSpeed(1); }
    #endregion

    #region Public Interface
    public override void ChangeCatSubState(byte _index)
    {
        AssignMoveSpeed(1);
        currentState = availableStates[_index];
        currentState.Enable();
        PrintState(GetStateIndex());
    }
    public override byte GetStateIndex()
    {
        if (currentState == availableStates[0])
            return 0;
        else
            return 1;
    }
    public override void UpdateState() { currentState.UpdateState(); }
    #endregion
}
public class Flee : CatState
{
    #region Variables
    private System.Action MoveAway;
    #endregion

    #region Initialization
    public Flee(CatManager _catManager) { MoveAway = _catManager.MoveAway; }
    #endregion

    #region Public Interface
    public override void UpdateState() { MoveAway(); }
    #endregion
}