using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CatManager : MonoBehaviour
{

    // Not correctly displaying if fleeing or not

    #region Variables
    // Inspector assignable attributes
    [SerializeField] private byte[] idleTimeRange;                 // Index 0 - Low side of range. 1 - High side of range
    [SerializeField] private byte[] moveSpeeds;                    // Index 0 - Patrol speed. 1 - Pursuit speed
    [SerializeField] private byte[] patrolTimeRange;               // Index 0 - Low side of range. 1 - High side of range
    [SerializeField] private float miniumAttackDistance;
    [SerializeField] private float miniumReceiveHitDistance;
    [SerializeField] private float timeBetweenDistanceChecks;      // Time between checking if close enough to Rag
    [SerializeField] private float timeBetweenSearches;            // Time between ray cast checking

    private byte mapBound;                                         // Bounds of map on the X-axis
    private byte randomIdleTime;                                   // How long idles this time
    private byte randomPatrolTime;                                 // How long patrols this time
    private const byte rayCastDistance = 50;

    // State Machine
    private CatState[] availableStates; 
    private CatState currentState;

    private float distanceFromRag;
    private float usedMoveSpeed;                                   // Current move speed
    private RaycastHit hitObject;                                  // Object intersecting raycast
    private Stopwatch internalTimer;                               // Timer the cat uses for its state
    private Stopwatch rayCastTimer;                               
    private Transform playerTransform;
    private Vector3 forward;
    private Vector3 targetPosition;                                // Location the cat  moves towards
    #endregion

    #region Initialization
    private void Awake()
    {
        internalTimer = new Stopwatch();
        rayCastTimer = new Stopwatch();
        mapBound = (byte)(GameObject.Find("Ground").transform.lossyScale.x * 5);
        availableStates = new CatState[3] { new Unalerted(this), new Alerted(this), new Flee(this) };
        currentState = availableStates[0];
        usedMoveSpeed = moveSpeeds[0];
        PrintState(currentState.GetStateIndex());
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition.y = transform.position.y;
        AssignRandomPatrolTarget();
    }
    #endregion

    #region Main Update
    private void Update() { currentState.UpdateState(); }
    #endregion

    #region Public Interface
    public void AssignMoveSpeed(byte _index) { usedMoveSpeed = moveSpeeds[_index]; }
    public void AssignRandomIdleTime() { randomIdleTime = (byte)Random.Range(idleTimeRange[0], idleTimeRange[1]); }
    public void AssignRandomPatrolTarget()
    {
        // Choose a random target direction
        byte randomMapBound = (byte)Random.Range(0, 2);

        if (randomMapBound == 0)
            AssignTarget(true);
        else
            AssignTarget(false);
    }
    public void AssignRandomPatrolTime() { randomPatrolTime = (byte)Random.Range(patrolTimeRange[0], patrolTimeRange[1]); }
    public void AttackTime()
    {
        // Set target position, so cat knows when Rag is far enough
        targetPosition = playerTransform.position;

        if (internalTimer.ElapsedMilliseconds > timeBetweenDistanceChecks)
        {
            // Check distance between cat and rag
            distanceFromRag = Vector3.Distance(transform.position, targetPosition);

            // If not close enough, pursue rag
            if (distanceFromRag > miniumAttackDistance)
                currentState.ChangeCatSubState(0);

            RestartInternalTimer();
        }
    }
    public void ChangeCatState(byte _index)
    {
        currentState = availableStates[_index];
        currentState.Enable();
        PrintState(currentState.GetStateIndex());
    }
    public void IdleTime()
    {
        // If enemy has been idling for longer than idle time, start patrolling
        if (internalTimer.Elapsed.Seconds > randomIdleTime)
            currentState.ChangeCatSubState(1);
    }
    public void MoveAway()
    {
        // Moves cat towards target at the assigned move speed
        Movement();

        // If target x is positive
        if (targetPosition.x == mapBound)
        {
            // If cat x is greater than map bounds, target is now negative
            if (transform.position.x > mapBound)
            {
                // Turn around
                AssignTarget(false);

                // If rag wasn't found, set to unalerted
                if (!LookForRag())
                    ChangeCatState(0);
            }
        }

        // If target x is negative
        else if (targetPosition.x == -mapBound)
        {
            // If cat x is less than negative map bounds, target is positive
            if (transform.position.x < -mapBound)
            {
                // Turn around
                AssignTarget(true);

                // If rag wasn't found, set to idle
                if (!LookForRag())
                    ChangeCatState(0);
            }
        }
    }
    public void PatrolMovement()
    {
        // Moves cat towards target at the assigned move speed
        Movement();
        
        if (rayCastTimer.ElapsedMilliseconds > timeBetweenSearches)
        {
            LookForRag();
            RestartRayCastTimer();
        }

        // If target x is positive
        if (targetPosition.x == mapBound)
        {
            // If cat x is greater than map bounds, target is now negative
            if (transform.position.x > mapBound)
                AssignTarget(false);
        }

        // If target x is negative
        else if(targetPosition.x == -mapBound)
        {
            // If cat x is less than negative map bounds, target is positive
            if (transform.position.x < -mapBound)
                AssignTarget(true);
        }

        // If enemy has been patrolling for longer than their time to patrol, they will return to idle
        if (internalTimer.Elapsed.Seconds > randomPatrolTime)
            currentState.ChangeCatSubState(0);
    }
    public void PrintState(byte _subState)
    {
        if (currentState != null)
        {
            if (currentState == availableStates[0])
            {
                if (_subState == 0)
                    UnityEngine.Debug.Log("Unalerted Idle");
                else
                    UnityEngine.Debug.Log("Unalerted Patrol");
            }
            else if (currentState == availableStates[1])
            {
                if (_subState == 0)
                    UnityEngine.Debug.Log("Alerted Pursuit");
                else
                    UnityEngine.Debug.Log("Alerted Attack");
            }
            else
                UnityEngine.Debug.Log("Fleeing");
        }
    }
    public void PursuitMovement()
    {
        // Set target position, so cat doesn't overshoot Rag
        targetPosition = playerTransform.position;

        transform.LookAt(targetPosition, Vector3.up);

        // Moves cat towards target at the assigned move speed
        Movement();

        if (internalTimer.ElapsedMilliseconds > timeBetweenDistanceChecks)
        {
            // Check distance between cat and rag
            distanceFromRag = Vector3.Distance(transform.position, targetPosition);

            // If close enough, attack rag
            if (distanceFromRag < miniumAttackDistance)
                currentState.ChangeCatSubState(1);

            RestartInternalTimer();
        }
    }
    public void ReceiveHit()
    {
        // Check distance between cat and rag
        distanceFromRag = Vector3.Distance(transform.position, targetPosition);

        if (distanceFromRag < miniumReceiveHitDistance)
        {
            if (currentState != availableStates[2])
            {
                // If player is on the left of cat
                if (playerTransform.position.x > transform.position.x)
                    AssignTarget(false);

                // If player is on the right of cat
                else
                    AssignTarget(true);

                // Change state to fleeing
                ChangeCatState(2);
            }
        }
    }
    public void RestartInternalTimer()
    {
        internalTimer.Reset();
        internalTimer.Start();
    }
    public void RestartRayCastTimer()
    {
        rayCastTimer.Reset();
        rayCastTimer.Start();
    }
    #endregion

    #region BlackBox
    private void AssignTarget(bool _setPositive)
    {
        if (_setPositive)
        {
            targetPosition.x = mapBound;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            targetPosition.x = -mapBound;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    private bool LookForRag()
    {
        // Transform cat's forward to world forward
        forward = transform.TransformDirection(Vector3.forward);

        // If raycast hits an object
        if (Physics.Raycast(transform.position, forward, out hitObject, rayCastDistance))
        {
            // If player, change state to alerted and substate to pursuit
            if (hitObject.collider.tag == "Player")
            {
                // If player is not hidden
                if (!hitObject.collider.GetComponent<PlayerManagerPDA>().GetHidden())
                {
                    // Change to alerted and return true because I found Rag
                    ChangeCatState(1);
                    return true;
                }
            }
        }

        // Didn't see Rag, return false
        return false;
    }
    private void Movement()
    {
        // Moves enemy towards the target position
        transform.Translate(Vector3.forward * usedMoveSpeed * Time.deltaTime);
    }
    #endregion
}