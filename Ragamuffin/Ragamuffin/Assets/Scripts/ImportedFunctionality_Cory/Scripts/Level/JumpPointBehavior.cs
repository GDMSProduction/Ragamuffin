using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointBehavior : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool startLeftDirection;           // Initial movement direction flag
    [SerializeField] private byte moveSpeed;                    // Self explanatory
    [SerializeField] private sbyte[] movementBound;             // Index 0 for left bound. Index 1 for right bound

    private Vector2 moveDirection;                              // Orb's current move direction
    #endregion

    #region Initialization
    private void Awake()
    {
        // If start left is true in the inspector, the jump orb's move direction will be left and vice versa
        moveDirection = (startLeftDirection) ? Vector2.left : Vector2.right;
    }
    #endregion

    #region Main Update
    private void Update()
    {
        Move();

        // Keeps orbs moving back and forth within their specified range
        BoundMovement();
    }
    #endregion

    #region Private
    private void BoundMovement()
    {
        if (transform.localPosition.x < movementBound[0])
            moveDirection = Vector2.right;
        else if (transform.localPosition.x > movementBound[1])
            moveDirection = Vector2.left;
    }
    private void Move()
    {
        // Moves orb in the specified direction, at the specified speed
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
    #endregion
}