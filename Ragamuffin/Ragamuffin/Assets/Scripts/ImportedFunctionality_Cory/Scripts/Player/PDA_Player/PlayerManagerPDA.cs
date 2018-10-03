using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManagerPDA : MonoBehaviour
{
    #region Variables
    // Inspector assignable attributes
    [SerializeField]private byte moveSpeed;

    private System.Action ReceiveHit;
    private bool hidden;
    private List<PlayerStatesPDA> stateStack;                   // Push Down Automata Stack
    private PlayerStatesPDA[] availableStates;                  // Stored states
    private Renderer renderForColor;
    #endregion

    #region Initialization
    private void Awake()
    {
        ReceiveHit = GameObject.Find("Cat").GetComponent<CatManager>().ReceiveHit;
        // Creates the stack, assigns states to an array, and pushes first state on the stack
        stateStack = new List<PlayerStatesPDA>();
        availableStates = new PlayerStatesPDA[] { new IdlePDA(this), new MovingPDA(this) };
        Push(0);
        renderForColor = GetComponent<Renderer>();
    }
    #endregion

    #region Public Interface
    public bool GetHidden() { return hidden; }
    public void Move(Vector3 _direction)
    {
        // Move player in the direction that was passed int, at the speed assigned in the inspector
        transform.Translate(_direction * moveSpeed *Time.deltaTime);
    }
    public void Pop()
    {
        // Removes the topmost state off of the stack
        stateStack.RemoveAt(stateStack.Count - 1);
    }
    public void Push(byte _index)
    {
        // If there is no state currently on the stack, just add it
        if (stateStack.Count == 0)
            stateStack.Add(availableStates[_index]);

        // If there is a state on the stack, add this one to the end
        else
            stateStack.Insert(stateStack.Count, availableStates[_index]);
    }
    #endregion

    #region Private
    private void Update() { PlayerInput(); }
    private void PlayerInput()
    {
        // If any key is pressed or held
        if (Input.anyKey)
        {
            // If either of these keys are being pressed or held, request movement from the state on the top of the stack
            if (Input.GetKey(KeyCode.LeftArrow))
                stateStack[stateStack.Count - 1].MoveRequest(Vector3.left);
            if (Input.GetKey(KeyCode.RightArrow))
                stateStack[stateStack.Count - 1].MoveRequest(Vector3.right);
            if (Input.GetKey(KeyCode.UpArrow))
                stateStack[stateStack.Count - 1].MoveRequest(Vector3.forward);
            if (Input.GetKey(KeyCode.DownArrow))
                stateStack[stateStack.Count - 1].MoveRequest(Vector3.back);
            if (Input.GetKeyDown(KeyCode.Space))
                ToggleHiding();
            if (Input.GetKeyDown(KeyCode.Return))
                ReceiveHit();
        }
        // If no key is being pressed
        else
        {
            // If player's state is moving, then pop the top most state off of the stack
            if (stateStack[stateStack.Count - 1] == availableStates[1])
                Pop();
        }


        if (Input.GetKeyUp(KeyCode.Space))
            ToggleHiding();
    }
    private void ToggleHiding()
    {
        hidden = !hidden;
        renderForColor.material.color = (hidden) ? Color.blue : Color.white;
    }
    #endregion
}