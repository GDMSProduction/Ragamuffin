using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlertedStates
{
    #region Variables
    private System.Action<bool> RestartTimer;
    #endregion

    #region Initialization
    public AlertedStates(ref CatManager _catManager) { RestartTimer = _catManager.RestartTimer; }
    public virtual void Enable() { RestartTimer(true); }
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
    public Pursuit(ref CatManager _catManager) : base(ref _catManager) { PursuitBehavior = _catManager.PursuitBehavior; }
    #endregion

    #region Main Update
    public override void UpdateState() { PursuitBehavior(); }
    #endregion
}
public class Flee : AlertedStates
{
    #region Variable
    private System.Action RunAway;
    #endregion

    #region Initialization
    public Flee(ref CatManager _catManager) : base(ref _catManager) { RunAway = _catManager.RunAway; }
    #endregion

    #region Main Update
    public override void UpdateState() { RunAway(); }
    #endregion
}