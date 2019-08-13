//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public sealed class CatManager : MonoBehaviour
{
    #region Variables
    // Inspector assignable attributes
    [SerializeField] private AudioClip[] sounds;                        // Sounds for cat behaviors. Index 0 - Alerted. Index 1 - Attacking. Index 2 - Fleeing. Index 3 - OnScreen
    [Header("Allows cat to lose sight of Rag")]
    [SerializeField] private bool ragIsLoseable;                            
    [SerializeField] private bool startLeftDirection;                   // Initial movement direction flag
    [Header("Probability that cat will jump up/down when triggered")]
    [SerializeField] private byte jumpDropChance;                       // 1:X chance that the cat decides to jump or drop when it hits orb
    [Header("Amount of damage dealt")]
    [SerializeField] private byte hitDamage;                            // Damage dealt
    [Header("Max distance cat can be from Rag")]
    [SerializeField] private byte maxSearchDistance;                    // Max distance cat can be from Rag
    [Header("How far cat can see")]
    [SerializeField] private byte sightDistance;                        // How far the cat can see
    [Header("Max distance until cat is on screen")]
    [SerializeField] private float onScreenDistanceFromRag;             // How close the cat needs to be on screen
    [Header("Minimum distance cat can begin attacking")]
    [SerializeField] private float miniumAttackDistance;                // How close the cat needs to be to attach
    [Header("Minimum distance cat can receive Rag damage")]
    [SerializeField] private float miniumReceiveHitDistance;            // How close rag has to be to hit the cat. Delete this when hit functionality is implemented
    [Header("First speed is patrol. Second speed is chase")]
    [SerializeField] private float[] moveSpeeds;                        // Index 0 - Patrol speed. 1 - Pursuit speed
    [SerializeField] private float timeBeforeFirstAttack;               // Self-explanatory
    [SerializeField] private float timeBetweenAttacks;                  // Self-explanatory
    [SerializeField] private float timeBetweenSearches;                 // Used to limit the number of raycasts per second

    // Debugging Tools
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canSearch;

    private AudioSource soundSource;                                    // Sound controller for cat
    private bool attacking;                                             // If cat has started attacking
    private bool checking;                                              // If cat is checking for a sound
    private bool fleeing;                                               // Flag for running away
    private bool onScreen;                                              // On screen flag
    private byte randomChance;                                          // Used to store the cat's decision to jump

    // State Machine
    private CatState[] availableStates; 
    private CatState currentState;

    private float currentAttackTime;                                    // Time between attacks
    private float currentMoveSpeed;                                     // Current move speed
    private float distanceFromRag;                                      // Self-explanatory
    private float teleportDistance;                                      // The distance the cat must teleport to get just outside of the camera's view
    private float verticalRepositionHeight;
    private long lastAttack;                                            // Stores the time of the last attack, to be used for the calculation of when it should attack again
    private long lastRaycast;                                           // Stores the time of the last time it looked for Rag, to be used for the next time it looks
    private RaycastHit hitObject;                                       // Object intersecting raycast
    private Stopwatch internalTimer;                                    // Timers for cat's behaviors              
    private Transform ragTransform;                                     // Self-explanatory
    private Transform raycastEye;                                       // Raycast start position (cat's eye)
    private Vector3 forward;                                            // Cat's forward in worldspace
    private Vector3 offset;                                             // Keeps the cat grounded for chase
    private Vector3 targetPosition;
	//
    #endregion

    #region Initialization
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        fleeing = false;
        onScreen = true;

        // This calculation will always put the cat just off screen
        const byte tDSetup = 25;
        teleportDistance = (byte)(maxSearchDistance - tDSetup);

        currentMoveSpeed = moveSpeeds[0];
        availableStates = new CatState[2] { new Unalerted(this), new Alerted(this)};
        currentState = availableStates[0];
        internalTimer = new Stopwatch();
        internalTimer.Start();
        ragTransform = GameObject.FindGameObjectWithTag("Player").transform;
        raycastEye = transform.GetChild(0);
        offset = new Vector3(0, -1, 0);

        // If start left is true in the inspector, the cat's will move left
        transform.rotation = (startLeftDirection) ? Quaternion.Euler(0, 270, 0) : Quaternion.Euler(0, 90, 0);
    }
    #endregion

    #region Main Update
    private void Update()
    {
        // Updates the the cat state machine
        currentState.UpdateState();
    }
    #endregion

    #region Public Interface
    public void AssignMoveSpeed(byte _index)
    {
        // Changes speed for walking, running, fleeing
        currentMoveSpeed = moveSpeeds[_index];
    }
    public void CheckLocation()
    {
        // Still performs patrol like movement, but moves towards a specified point
        PatrolMovement();

        // If cat's as reached target, change back to unalerted
        // State will change in patrol movement if Rag is found
        if (transform.position == targetPosition)
        {
            checking = false;
            ChangeCatState(0);
        }
    }
    public void ChangeCatState(byte _index)
    {
        // Self explanatory
        currentState = availableStates[_index];
        currentState.Enable();
    }
    public void GetCurrentTime(bool _attack)
    {
        if (_attack)
            lastAttack = internalTimer.ElapsedMilliseconds;
        else
            lastRaycast = internalTimer.ElapsedMilliseconds;
    }
    public void PatrolMovement()
    {
        // Debugging if check
        if (canMove)
        {
            // Moves cat towards target at the assigned move speed
            Movement();
        }

        // If cat goes too far away from rag, turn him around and move him closer
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If cat is on-screen
        if (distanceFromRag < onScreenDistanceFromRag)
        {
            if (!onScreen)
            {
                onScreen = true;

                // Play on-screen sound
                PlaySound(3);
            }
        }

        // If cat is off-screen
        else
        {
            if (onScreen)
                onScreen = false;

            // If checking a position, we don't want cat to turn arround or teleport
            if (!checking)
            {
                if (distanceFromRag > maxSearchDistance)
                {
                    // Turn cat around
                    transform.rotation = (transform.rotation == Quaternion.Euler(0, 90, 0)) ? Quaternion.Euler(0, 270, 0) : Quaternion.Euler(0, 90, 0);

                    // Teleport closer
                    // If cat is on Rag's left
                    if (transform.position.x < ragTransform.position.x)
                        transform.position = new Vector3(transform.position.x + teleportDistance, transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x - teleportDistance, transform.position.y, transform.position.z);
                }
            }
        }

        // Debugging if check
        if (canSearch)
        {
            //Used to restrict the amount of raycasts per second, because they can be expensive
            if (internalTimer.ElapsedMilliseconds - lastRaycast > timeBetweenSearches * 1000)
            {
                LookForRag();

                // Reassign last raycast
                GetCurrentTime(false);
            }
        }
    }
    public void PlaySound(byte _index)
    {
        soundSource.clip = sounds[_index];
        soundSource.Play();
    }
    public void PursuitBehavior()
    {
        // Check distance between cat and rag
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If Rag is allowed to be lost
        if (ragIsLoseable)
        {
            // If Rag is far enough
            if (distanceFromRag > sightDistance)
            {
                // Change back to unalerted patrol
                ChangeCatState(0);
            }
        }

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
            // Debugging if check
            if (canAttack)
                Attack();
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
            {
                ChangeCatState(1);

                // Change to flee behavior
                currentState.ChangeSubstate(1);
            }

            fleeing = true;
            attacking = false;

            // Start fleeing
            currentState.ChangeSubstate(1);
        }
    }
    public void RunAway()
    {
        // Right now, the cat just moves away, we can make it do whatever else we want here
        Movement();
    }
    public void StartCheck(Vector3 _targetPosition)
    {
        checking = true;
        targetPosition = _targetPosition;
        ChangeCatState(1);

        // Change to check behavior
        currentState.ChangeSubstate(2);
    }
    #endregion

    #region Private
    private void Attack()
    {
        if (attacking)
        {
            //Used to time attacks
            if (internalTimer.ElapsedMilliseconds - lastAttack > currentAttackTime * 1000)
            {
                // Put actual attack code here (damage player, play animation, etc.)

                // Play attack audio
                PlaySound(1);

                // Reassign last attack
                GetCurrentTime(true);

                if (currentAttackTime != timeBetweenAttacks)
                    currentAttackTime = timeBetweenAttacks;
            }

            return;
        }

        // Only makes it here before cat attacks
        // Reset timer
        GetCurrentTime(true);

        // Allow cat to attack on the next pass
        attacking = true;

        // Allows for differnt first attack time
        currentAttackTime = timeBeforeFirstAttack;
    }
    private void LookForRag()
    {
        // Transform cat's forward to world forward
        forward = transform.TransformDirection(Vector3.forward);

        // If within range and grounded
        if (distanceFromRag < sightDistance)
        {
            /// Remove for actual game
            // Actual drawn line in scene view
            UnityEngine.Debug.DrawLine(raycastEye.position, raycastEye.position + (forward * sightDistance));

            // If raycast hits an object
            if (Physics.Raycast(raycastEye.position, forward, out hitObject, sightDistance))
            {
                // If player, change state to alerted and substate to pursuit
                if (hitObject.collider.tag == "Player")
                {
                    // If player is not hidden
                    if (!hitObject.collider.transform.parent.GetComponent<PlayerManagerPDA>().GetHidden())
                    {
                        // Change to alerted and return true because I found Rag
                        ChangeCatState(1);

                        // Change to pursuit behavior
                        currentState.ChangeSubstate(0);
                    }
                }
            }
        }
    }
    private void Movement()
    {
        // Moves cat in move direction, at current speed
        if (!checking)
            transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);
        else
            transform.position = Vector2.Lerp(transform.position, targetPosition, currentMoveSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!checking)
        {
            //if (onScreen)
            {
                if (other.tag == "JumpPoint")
                {
                    // Debugging if check
                    if (canJump)
                    {
                        // If patrolling
                        if (currentState == availableStates[0])
                        {
                            // Retrieving random cat decision
                            randomChance = (byte)Random.Range(0, jumpDropChance);

                            // If cat decides to jump/drop
                            if (randomChance == 0)
                            {
                                // If left side activation
                                if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 1)
                                    transform.position = new Vector3(transform.position.x + other.GetComponent<JumpNodeBehavior>().GetHorizontalDistance(), other.GetComponent<JumpNodeBehavior>().GetverticalRepositionHeight(), transform.position.z);

                                // If right side activation
                                else if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 2)
                                    transform.position = new Vector3(transform.position.x - other.GetComponent<JumpNodeBehavior>().GetHorizontalDistance(), other.GetComponent<JumpNodeBehavior>().GetverticalRepositionHeight(), transform.position.z);

                                // This type of point doesn't care the direction, it will reflect regardless
                                else
                                {
                                    if (transform.rotation == Quaternion.Euler(0, 90, 0))
                                        transform.position = new Vector3(transform.position.x + other.GetComponent<JumpNodeBehavior>().GetHorizontalDistance(), other.GetComponent<JumpNodeBehavior>().GetverticalRepositionHeight(), transform.position.z);
                                    else
                                        transform.position = new Vector3(transform.position.x - other.GetComponent<JumpNodeBehavior>().GetHorizontalDistance(), other.GetComponent<JumpNodeBehavior>().GetverticalRepositionHeight(), transform.position.z);
                                }
                            }
                        }
                    }
                }
                else if (other.tag == "TurnPoint")
                {
                    // If right side activation
                    if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 1)
                    {
                        // If cat is on the left side of this point, make it move left
                        if (transform.position.x < other.transform.position.x)
                            transform.rotation = Quaternion.Euler(0, 270, 0);

                        // Act like nothing happened
                        else return;
                    }

                    // If left side activation
                    else if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 2)
                    {
                        // If cat is on the right side of this point, make it move right
                        if (transform.position.x > other.transform.position.x)
                            transform.rotation = Quaternion.Euler(0, 90, 0);

                        // Act like nothing happened
                        else return;
                    }

                    // This type of point doesn't care the direction, it will reflect regardless
                    else
                        transform.rotation = (transform.rotation == Quaternion.Euler(0, 90, 0)) ? Quaternion.Euler(0, 270, 0) : Quaternion.Euler(0, 90, 0);

                    // If fleeing, change to unalerted
                    if (fleeing)
                    {
                        // Stop fleeing
                        currentState.ChangeSubstate(0);
                        ChangeCatState(0);
                        fleeing = false;
                    }
                }
            }
        }
    }
    #endregion
}