using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JackState
{
    #region Initialization
    public virtual void Enable() { return; }
    #endregion

    #region Main UpdateState
    public abstract void UpdateState();
    #endregion

    #region Public Interface
    public virtual void ChangeJackSubState(byte _index) { return; }
    #endregion
}
public class Closed : JackState
{
    #region Variables
    private System.Action LookForRag;
    #endregion

    #region Initialization
    public Closed(JackManager _jackManager) { LookForRag = _jackManager.LookForRag; }
    #endregion

    #region Main UpdateState
    public override void UpdateState()
    {
        LookForRag();

        // Play the standard theme when cranking the JIB
    }
    #endregion
}
public class Open : JackState
{
    #region Variables
    private JackSubState[] availableStates;
    private JackSubState currentState;
    #endregion

    #region Initialization
    public Open(JackManager _jackManager)
    {
        availableStates = new JackSubState[3] { new Idle(ref _jackManager), new Jump(ref _jackManager), new Fall(ref _jackManager) };
        currentState = availableStates[0];
    }
    public override void Enable() { currentState = availableStates[0]; }
    #endregion

    #region Main UpdateState
    public override void UpdateState() { currentState.UpdateState(); }
    #endregion

    #region Public Interface
    public override void ChangeJackSubState(byte _index)
    {
        // Self explanatory
        currentState = availableStates[_index];
        currentState.Enable();
    }
    #endregion
}