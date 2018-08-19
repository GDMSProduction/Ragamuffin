using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatesFSM
{
    #region Variables
    protected PlayerManagerFSM playerManager;
    #endregion

    #region Initialization
    public PlayerStatesFSM(PlayerManagerFSM _playerManager) { playerManager = _playerManager; }
    #endregion

    #region Public Interface
    public abstract void MoveRequest(Vector3 _direction);
    #endregion

}
public class IdleFSM : PlayerStatesFSM
{
    #region Initialization
    public IdleFSM(PlayerManagerFSM _playerManager) : base(_playerManager) { return; }
    #endregion

    #region Public Interface
    public override void MoveRequest(Vector3 _direction) { playerManager.ChangePlayerStates(1); }
    #endregion
}
public class MovingFSM : PlayerStatesFSM
{
    #region Initialization
    public MovingFSM(PlayerManagerFSM _playerManager) : base(_playerManager) { return; }
    #endregion

    #region Public Interface
    public override void MoveRequest(Vector3 _direction) { playerManager.Move(_direction); }
    #endregion
}