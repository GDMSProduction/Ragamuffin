using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CatManager : MonoBehaviour
{
    // Not correctly displaying if fleeing or not

    #region Variables
    // Inspector assignable attributes
    [SerializeField] private AudioClip[] sounds;                   // Sounds for cat behaviors. Index 0 - Alerted. Index 1 - Attacking. Index 2 - Fleeing. Index 3 - OnScreen
    [SerializeField] private bool startLeftDirection;              // Initial movement direction flag
    [SerializeField] private byte jumpDropChance;                  // 1:? chance that the cat decides to jump or drop when it hits orb
    [SerializeField] private byte hitDamage;                       // Damage dealt
    [SerializeField] private byte maxSearchDistance;               // Max distance cat can be from Rag
    [SerializeField] private byte[] moveSpeeds;                    // Index 0 - Patrol speed. 1 - Pursuit speed
    [SerializeField] private byte sightDistance;                   // How far the cat can see
    [SerializeField] private float miniumAttackDistance;           // How close the cat needs to be to attach
    [SerializeField] private float miniumReceiveHitDistance;       // How close rag has to be to hit the cat. Delete this when hit functionality is implemented
    [SerializeField] private float timeBetweenAttacks;             // Self-explanatory
    [SerializeField] private float timeBetweenSearches;            // Used to limit the number of raycasts per second

    private AudioSource soundSource;                               // Sound controller for cat
    private bool fleeing;                                          // Flag for running away
    private bool onScreen;                                         // On screen flag
    private byte randomChance;                                     // Used to store the cat's decision to jump
    private byte teleportDistance;                                 // The distance the cat must teleport to get just outside of the camera's view

    // State Machine
    private CatState[] availableStates; 
    private CatState currentState;

    private float distanceFromRag;                                 // Self-explanatory
    private float currentMoveSpeed;                                // Current move speed
    private long lastAttack;                                       // Stores the time of the last attack, to be used for the calculation of when it should attack again
    private long lastRaycast;                                      // Stores the time of the last time it looked for Rag, to be used for the next time it looks
    private RaycastHit hitObject;                                  // Object intersecting raycast
    private Stopwatch internalTimer;                               // Timers for cat's behaviors              
    private Transform ragTransform;                                // Self-explanatory
    private Transform raycastEye;                                  // Raycast start position (cat's eye)
    private Vector3 forward;                                       // Cat's forward in worldspace
    private Vector3 offset;                                        // Keeps the cat grounded for chase
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
    public void GetCurrentTime(bool _attack)
    {
        if (_attack)
            lastAttack = internalTimer.ElapsedMilliseconds;
        else
            lastRaycast = internalTimer.ElapsedMilliseconds;
    }
    public void PatrolMovement()
    {
        // Moves cat towards target at the assigned move speed
        Movement();

        // If cat goes too far away from rag, turn him around and move him closer
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If cat is on-screen
        if (distanceFromRag < 30)
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

        //Used to restrict the amount of raycasts per second, because they can be expensive
        if (internalTimer.ElapsedMilliseconds - lastRaycast > timeBetweenSearches * 1000)
        {
            LookForRag();

            // Reassign last raycast
            GetCurrentTime(false);
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
            Attack();
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
            currentState.ChangeSubstate(true);
        }
    }
    public void RunAway()
    {
        // Right now, the cat just moves away, we can make it do whatever else we want here
        Movement();
    }
    #endregion

    #region Private
    private void Attack()
    {
        //Used to time attacks
        if (internalTimer.ElapsedMilliseconds - lastAttack > timeBetweenAttacks * 1000)
        {
            // Put actual attack code here (damage player, play animation, etc.)

            // Play attack audio
            PlaySound(1);

            // Reassign last attack
            GetCurrentTime(true);
        }
    }
    private bool LookForRag()
    {
        // Transform cat's forward to world forward
        forward = transform.TransformDirection(Vector3.forward);

        // If within range and grounded
        if (distanceFromRag < 20 && (transform.position.y == 0))
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
                    if (!hitObject.collider.GetComponent<PlayerManagerPDA>().GetHidden())
                    {
                        // Change to alerted and return true because I found Rag
                        ChangeCatState(1);
                        return true;
                    }
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
                    {
                        if (transform.rotation == Quaternion.Euler(0, 90, 0))
                            transform.position = new Vector3(transform.position.x + 5, 2.75f, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - 5, 2.75f, transform.position.z);
                    }

                    // Drop down
                    else
                    {
                        if (transform.rotation == Quaternion.Euler(0, 90, 0))
                            transform.position = new Vector3(transform.position.x + 5, 0, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - 5, 0, transform.position.z);
                    }
                }
            }
        }
        else if (other.tag == "TurnPoint")
        {
            if (other.name == "Right Point")
            {
                // If cat is on the left side of this point, make it move left
                if (transform.position.x < other.transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 270, 0);

                // Act like nothing happened
                else return;
            }            
            else if (other.name == "Left Point")
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
                currentState.ChangeSubstate(false);
                ChangeCatState(0);
                fleeing = false;
            }
        }
    }
    #endregion
}