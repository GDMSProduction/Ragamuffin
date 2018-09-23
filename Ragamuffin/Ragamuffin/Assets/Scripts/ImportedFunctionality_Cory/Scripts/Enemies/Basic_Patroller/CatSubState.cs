using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlertedStates
{
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
    #region Variable
    private System.Action RunAway;
    #endregion

    #region Initialization
    public Flee(ref CatManager _catManager) { RunAway = _catManager.RunAway; }
    #endregion

    #region Main Update
    public override void UpdateState() { RunAway(); }
    #endregion
}