//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpingJackSubState
{
    #region Initialize
    public virtual void Enable() { return; }
    #endregion

    #region Main Update
    public abstract void UpdateState();
    #endregion
}
public class Idle : JumpingJackSubState
{
    #region Variables
    private System.Action StartJump;
    #endregion

    #region Initialization
    public Idle(ref JumpingJackManager _jumpingJackManager) { StartJump = _jumpingJackManager.StartJump; }
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
public class Jump : JumpingJackSubState
{
    #region Variables
    private System.Action Attack;
    private System.Action Jumping;
    private System.Action<byte> PlaySound;
    private System.Action<bool> ResetForce;
    #endregion

    #region Initialization
    public Jump(ref JumpingJackManager _jumpingJackManager)
    {
        Attack = _jumpingJackManager.Attack;
        Jumping = _jumpingJackManager.Jumping;
        PlaySound = _jumpingJackManager.PlaySound;
        ResetForce = _jumpingJackManager.ResetForce;
    }
    public override void Enable()
    {
        ResetForce(true);
        PlaySound(0);
    }
    #endregion

    #region Main Update
    public override void UpdateState()
    {
        Jumping();
        Attack();
    }
    #endregion
}
public class Fall : JumpingJackSubState
{
    #region Variables
    private System.Action Attack;
    private System.Action Falling;
    private System.Action<bool> ResetForce;
    #endregion

    #region Initialization
    public Fall(ref JumpingJackManager _jumpingJackManager)
    {
        Attack = _jumpingJackManager.Attack;
        Falling = _jumpingJackManager.Falling;
        ResetForce = _jumpingJackManager.ResetForce;
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