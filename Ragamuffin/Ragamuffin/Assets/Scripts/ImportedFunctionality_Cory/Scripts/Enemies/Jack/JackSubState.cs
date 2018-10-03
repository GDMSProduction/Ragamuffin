using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JackSubState
{
    #region Initialize
    public virtual void Enable() { return; }
    #endregion

    #region Main Update
    public abstract void UpdateState();
    #endregion
}
public class Idle : JackSubState
{
    #region Variables
    private System.Action StartJump;
    #endregion

    #region Initialization
    public Idle(ref JackManager _jackManager) { StartJump = _jackManager.StartJump; }
    #endregion

    #region Main Update
    public override void UpdateState()
    {
        // Sit there and sway, maybe make noise

        // Checking to see if Rag is close enough to star jumping
        StartJump();
    }
    #endregion
}
public class Jump : JackSubState
{
    #region Variables
    private System.Action Attack;
    private System.Action Jumping;
    private System.Action<bool> ResetForce;
    #endregion

    #region Initialization
    public Jump(ref JackManager _jackManager)
    {
        Attack = _jackManager.Attack;
        Jumping = _jackManager.Jumping;
        ResetForce = _jackManager.ResetForce;
    }
    public override void Enable() { ResetForce(true); }
    #endregion

    #region Main Update
    public override void UpdateState()
    {
        Jumping();
        Attack();
    }
    #endregion
}
public class Fall : JackSubState
{
    #region Variables
    private System.Action Attack;
    private System.Action Falling;
    private System.Action<bool> ResetForce;
    #endregion

    #region Initialization
    public Fall(ref JackManager _jackManager)
    {
        Attack = _jackManager.Attack;
        Falling = _jackManager.Falling;
        ResetForce = _jackManager.ResetForce;
    }
    public override void Enable() { ResetForce(false); }
    #endregion

    #region Main Update
    public override void UpdateState()
    {
        Falling();
        Attack();
    }
    #endregion
}