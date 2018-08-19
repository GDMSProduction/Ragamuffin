using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerFSM : MonoBehaviour
{
    #region Variables
    // Inspector assignable attributes
    [SerializeField] byte moveSpeed;

    // Finite State Machine
    private PlayerStatesFSM currentState;
    private PlayerStatesFSM[] availableStates;
    #endregion

    #region Initialization
    private void Awake()
    {
        currentState = new IdleFSM(this);
        availableStates = new PlayerStatesFSM[] { currentState, new MovingFSM(this) };
    }
    #endregion

    #region Public Interface
    public void ChangePlayerStates(byte _index) { currentState = availableStates[_index]; }
    public void Move(Vector3 _direction)
    {
        // Move player in the direction that was passed int, at the speed assigned in the inspector
        transform.Translate(_direction * moveSpeed *Time.deltaTime);
    }
    #endregion

    #region Blackbox
    private void Update() { PlayerInput(); }
    private void PlayerInput()
    {
        // If any key is pressed or held
        if (Input.anyKey)
        {
            // If either of these keys are being pressed or held, request movement from the state machine
            if (Input.GetKey(KeyCode.LeftArrow))
                currentState.MoveRequest(Vector3.left);
            if (Input.GetKey(KeyCode.RightArrow))
                currentState.MoveRequest(Vector3.right);
            if (Input.GetKey(KeyCode.UpArrow))
                currentState.MoveRequest(Vector3.forward);
            if (Input.GetKey(KeyCode.DownArrow))
                currentState.MoveRequest(Vector3.back);
        }
        // If no key is being pressed
        else
        {
            // If player's state is moving, then switch to not moving
            if (currentState == availableStates[1])
                ChangePlayerStates(0);
        }
    }
    #endregion
}