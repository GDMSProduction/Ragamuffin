using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CatManager : MonoBehaviour
{
    // Not correctly displaying if fleeing or not

    #region Variables
    // Inspector assignable attributes
    [SerializeField] private bool startLeftDirection;              // Initial movement direction flag
    [SerializeField] private byte jumpDropChance;                  // 1:? chance that the cat decides to jump or drop when it hits orb
    [SerializeField] private byte hitDamage;                       // Damage dealt
    [SerializeField] private byte[] moveSpeeds;                    // Index 0 - Patrol speed. 1 - Pursuit speed
    [SerializeField] private byte sightDistance;                   // How far the cat can see
    [SerializeField] private float miniumAttackDistance;           // How close the cat needs to be to attach
    [SerializeField] private float miniumReceiveHitDistance;       // How close rag has to be to hit the cat. Delete this when hit functionality is implemented
    [SerializeField] private float timeBetweenDistanceChecks;      // Time between checking if close enough to Rag

    private bool fleeing;
    private byte randomChance;                                     // Used to store the cat's decision to jump

    // State Machine
    private CatState[] availableStates; 
    private CatState currentState;

    private float distanceFromRag;                                 // Self-explanatory
    private float currentMoveSpeed;                                // Current move speed
    private float timeBetweenSearches = 0.1f;                      // Used to limit the number of raycasts per second
    private RaycastHit hitObject;                                  // Object intersecting raycast
    private Stopwatch raycastTimer;                                // Timers for cat's behaviors. Index 1 for internal. Index 0 for raycast timing                
    private Transform ragTransform;                                // Self-explanatory
    private Transform raycastEye;                                  // Raycast start position (cat's eye)
    private Vector3 forward;                                       // Cat's forward in worldspace
    private Vector3 offset;                                        // Keeps the cat grounded for chase
    #endregion

    #region Initialization
    private void Awake()
    {
        fleeing = false;
        currentMoveSpeed = moveSpeeds[0];
        availableStates = new CatState[2] { new Unalerted(this), new Alerted(this)};
        currentState = availableStates[0];
        raycastTimer = new Stopwatch();
        raycastTimer.Start();
        ragTransform = GameObject.FindGameObjectWithTag("Player").transform;
        raycastEye = transform.GetChild(1);
        offset = new Vector3(0, -1, 0);

        // If start left is true in the inspector, the cat's will move left
        transform.rotation = (startLeftDirection) ? Quaternion.Euler(0, 270, 0) : Quaternion.Euler(0, 90, 0);
    }
    #endregion

    #region Main Update
    private void Update()
    {
        // Runs the the cat state
        currentState.UpdateState();
    }
    #endregion

    #region Public Interface
    public void AssignMoveSpeed(byte _index)
    {
        // Changes speed for walking, running, fleeing
        currentMoveSpeed = moveSpeeds[_index];
    }
    public void ChangeCatState(byte _index)
    {
        // Self explanatory
        currentState = availableStates[_index];
        currentState.Enable();
    }
    public void PatrolMovement()
    {
        // Moves cat towards target at the assigned move speed
        Movement();

        //Used to restrict the amount of raycasts per second, because they can be expensive
        if (raycastTimer.ElapsedMilliseconds > timeBetweenSearches)
        {
            LookForRag();

            // Restart external timer
            RestartTimer(false);
        }
    }
    public void PursuitBehavior()
    {
        // Check distance between cat and rag
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If not within attack range, get closer
        if (distanceFromRag > miniumAttackDistance)
        {
            // Look at Rag
            transform.LookAt(ragTransform.position + offset, Vector3.up);

            // Moves cat towards Rag at the assigned move speed
            Movement();
        }

        // If within attack range, attack
        else
        {
            //hitDamage to Rag
        }
    }

    // This function is temporary, because this Rag's attack is not currently implemented
    public void ReceiveHit()
    {
        // Check distance between cat and rag
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        if (distanceFromRag < miniumReceiveHitDistance)
        {
            // If player is on the left of cat, move right
            if (ragTransform.position.x < transform.position.x)
                transform.rotation = Quaternion.Euler(0, 90, 0);

            // If player is on the right of cat, move left
            else
                transform.rotation = Quaternion.Euler(0, 270, 0);

            // If cat is unalerted, toggle to alerted
            if (currentState == availableStates[0])
                ChangeCatState(1);

            fleeing = true;

            // Start fleeing
            currentState.ToggleFlee();
        }
    }
    public void RestartTimer(bool _internal)
    {
        raycastTimer.Reset();
        raycastTimer.Start();
    }
    public void RunAway()
    {
        // Right now, the cat just moves away, we can make it do whatever else we want here
        Movement();
    }
    #endregion

    #region Private
    private bool LookForRag()
    {
        // Transform cat's forward to world forward
        forward = transform.TransformDirection(Vector3.forward);

        /// Comment this out for actual game
        // Actual drawn line in scene view
        UnityEngine.Debug.DrawLine(raycastEye.position, raycastEye.position + (forward * sightDistance));

        // If raycast hits an object
        if (Physics.Raycast(raycastEye.position, forward, out hitObject, sightDistance))
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
        // Moves cat in move direction, at current speed
        transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpPoint")
        {
            // If patrolling
            if (currentState == availableStates[0])
            {
                // Retrieving random cat decision
                randomChance = (byte)Random.Range(0, jumpDropChance);

                // If cat decides to jump/drop
                if (randomChance == 0)
                {
                    // Jump up
                    if (transform.position.y == 0)
                        transform.position = new Vector3(transform.position.x, 2.75f, transform.position.z);

                    // Drop down
                    else
                        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
            }
        }
        else if (other.tag == "PatrolPoint")
        {
            // If cat is moving right
            if (other.name == "Right Point")
            {
                // If cat is on the left side of this point, make it move left
                if (transform.position.x < other.transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 270, 0);

                // Act like nothing happened
                else return;
            }

            // If cat is moving left
            else
            {
                // If cat is on the right side of this point, make it move right
                if (transform.position.x > other.transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 90, 0);

                // Act like nothing happened
                else return;
            }


            // If fleeing, change to unalerted
            if (fleeing)
            {
                // Stop fleeing
                currentState.ToggleFlee();
                ChangeCatState(0);
                fleeing = false;
            }
        }
    }
    #endregion
}