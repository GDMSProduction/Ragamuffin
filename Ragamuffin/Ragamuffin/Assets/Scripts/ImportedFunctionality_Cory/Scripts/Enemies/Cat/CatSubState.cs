using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlertedStates
{
    #region Initialization
    public virtual void Enable() { return; }
    #endregion

    #region Main Update
    public abstract void UpdateState();
    #endregion
}

public class Pursuit : AlertedStates
{
    #region Variable
    private System.Action PursuitBehavior;
    #endregion

    #region Initialization
    public Pursuit(ref CatManager _catManager) { PursuitBehavior = _catManager.PursuitBehavior; }
    #endregion

    #region Main Update
    public override void UpdateState() { PursuitBehavior(); }
    #endregion
}
public class Flee : AlertedStates
{
    #region Variables
    private System.Action<byte> PlaySound;
    private System.Action RunAway;
    #endregion

    #region Initialization
    public Flee(ref CatManager _catManager)
    {
        PlaySound = _catManager.PlaySound;
        RunAway = _catManager.RunAway;
    }
    public override void Enable() { PlaySound(2); }
    #endregion

    #region Main Update
    public override void UpdateState() { RunAway(); }
    #endregion
}