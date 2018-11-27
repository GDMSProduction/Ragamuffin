//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpingJackState
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
public class Closed : JumpingJackState
{
    #region Variables
    private System.Action LookForRag;
    #endregion

    #region Initialization
    public Closed(JumpingJackManager _jumpingJackManager) { LookForRag = _jumpingJackManager.LookForRag; }
    #endregion

    #region Main UpdateState
    public override void UpdateState()
    {
        LookForRag();

        // Play the standard theme when cranking the JIB
    }
    #endregion
}
public class Open : JumpingJackState
{
    #region Variables
    private JumpingJackSubState[] availableStates;
    private JumpingJackSubState currentState;
    #endregion

    #region Initialization
    public Open(JumpingJackManager _jumpingJackManager)
    {
        availableStates = new JumpingJackSubState[3] { new Idle(ref _jumpingJackManager), new Jump(ref _jumpingJackManager), new Fall(ref _jumpingJackManager) };
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