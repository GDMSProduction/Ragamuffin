using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSubState
{
    #region Variables
    private System.Action RestartInternalTimer;
    #endregion

    #region Initialization
    public CatSubState(ref CatManager _catManager) { RestartInternalTimer = _catManager.RestartInternalTimer; }
    public virtual void Enable() { RestartInternalTimer(); }
    #endregion

    #region Main Update
    public virtual void UpdateState() { return; }
    #endregion
}

#region Unalerted Substates
public class Idle : CatSubState
{
    #region Variable
    private System.Action AssignRandomIdleTime;
    private System.Action IdleTime;
    #endregion

    #region Initialization
    public Idle(ref CatManager _catManager) : base(ref _catManager)
    {
        AssignRandomIdleTime = _catManager.AssignRandomIdleTime;
        IdleTime = _catManager.IdleTime;
    }
    public override void Enable()
    {
        base.Enable();
        AssignRandomIdleTime();
    }
    #endregion

    #region Main Update
    public override void UpdateState() { IdleTime(); }
    #endregion
}
public class Patrol : CatSubState
{
    #region Variable
    private System.Action AssignRandomPatrolTarget;
    private System.Action AssignRandomPatrolTime;
    private System.Action PatrolMovement;
    private System.Action RestartRayCastTimer;
    #endregion

    #region Initialization
    public Patrol(ref CatManager _catManager) : base(ref _catManager)
    {
        AssignRandomPatrolTarget = _catManager.AssignRandomPatrolTarget;
        AssignRandomPatrolTime = _catManager.AssignRandomPatrolTime;
        PatrolMovement = _catManager.PatrolMovement;
        RestartRayCastTimer = _catManager.RestartRayCastTimer;
    }
    public override void Enable()
    {
        base.Enable();
        AssignRandomPatrolTarget();
        AssignRandomPatrolTime();
        RestartRayCastTimer();
    }
    #endregion

    #region Main Update
    public override void UpdateState() { PatrolMovement(); }
    #endregion
}
#endregion

#region Alerted Substates
public class Pursuit : CatSubState
{
    #region Variable
    private System.Action PursuitMovement;
    #endregion

    #region Initialization
    public Pursuit(ref CatManager _catManager) : base(ref _catManager) { PursuitMovement = _catManager.PursuitMovement; }
    #endregion

    #region Main Update
    public override void UpdateState() { PursuitMovement(); }
    #endregion
}
public class Attack : CatSubState
{
    #region Variable
    private System.Action AttackTime;
    #endregion

    #region Initialization
    public Attack(ref CatManager _catManager) : base(ref _catManager) { AttackTime = _catManager.AttackTime; }
    #endregion

    #region Main Update
    public override void UpdateState() { AttackTime(); }
    #endregion
}
#endregion