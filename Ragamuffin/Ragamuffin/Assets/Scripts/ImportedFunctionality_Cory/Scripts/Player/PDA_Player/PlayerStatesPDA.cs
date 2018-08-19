using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatesPDA
{
    #region Variables
    protected PlayerManagerPDA playerManager;
    #endregion

    #region Initialization
    public PlayerStatesPDA(PlayerManagerPDA _playerManager) { playerManager = _playerManager; }
    #endregion

    #region Public Interface
    public abstract void MoveRequest(Vector3 _direction);
    #endregion

}
public class IdlePDA : PlayerStatesPDA
{
    #region Initialization
    public IdlePDA(PlayerManagerPDA _playerManager) : base(_playerManager) { return; }
    #endregion

    #region Public Interface
    public override void MoveRequest(Vector3 _direction) { playerManager.Push(1); }
    #endregion
}
public class MovingPDA : PlayerStatesPDA
{
    #region Initialization
    public MovingPDA(PlayerManagerPDA _playerManager) : base(_playerManager) { return; }
    #endregion

    #region Public Interface
    public override void MoveRequest(Vector3 _direction) { playerManager.Move(_direction); }
    #endregion
}