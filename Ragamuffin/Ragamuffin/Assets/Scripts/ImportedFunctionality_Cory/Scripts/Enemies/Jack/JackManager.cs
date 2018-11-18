using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public sealed class JackManager : MonoBehaviour
{
    #region Variables
    // Inspector assignable attributes
    [SerializeField] private AudioClip[] sounds;                   // Sounds for jack behaviors.
    [Header("Allows Jack to move side-to-side")]
    [SerializeField] private bool horizontalMovement;
    [Header("Amount of damage dealt")]
    [SerializeField] private byte hitDamage;                       // Damage dealt
    [SerializeField] private float gravitationalForce;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float initialJumpForce;
    [SerializeField] private float maxJumpHeight;
    [Header("Max range of attack")]
    [SerializeField] private float maxAttackDistance;              // How close the jack needs to be to attack
    [Header("Min range to Rag, before Jack starts jumping")]
    [SerializeField] private float minimumJumpDistance;            // How close Rag needs to be to jump
    [Header("Min range to Rag, before Jack comes out of box")]
    [SerializeField] private float minimumOpenDistance;            // How close Rag needs to be to open up
    [Header("Minimum distance Jack can receive Rag damage")]
    [SerializeField] private float miniumReceiveHitDistance;       // How close Rag needs to be to hit Jack
    [SerializeField] private float timeBetweenAttacks;             // Self-explanatory
    [SerializeField] private float timeBetweenJumps;               // Self-explanatory
    
    // Debugging Tools
    [Header("Warning: If 'canJump' is deselected, Jack cannot attack either")]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canJump;

    private AudioSource soundSource;                               // Sound controller for jack
    private bool isGrounded;                                       // Self-explanatory
    private bool isJumping;                                        // Self-explanatory
    private float currentForce;
    private float distanceFromRag;                                 // Self-explanatory

    // State Machine
    private JackState[] availableStates;
    private JackState currentState;

    private long lastAttack;                                       // Stores the time of the last attack, to be used for the calculation of when it should attack again
    private long lastJump;                                         // Stores the time of the last jump, to be used for the calculation of when it should jump again
    private Stopwatch internalTimer;                               // Timers for jack's behaviors              
    private Transform ragTransform;                                // Self-explanatory
    private Vector3 direction;
    #endregion

    #region Initialization
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        isGrounded = true;
        isJumping = false;
        availableStates = new JackState[2] { new Closed(this), new Open(this) };
        currentState = availableStates[0];
        internalTimer = new Stopwatch();
        internalTimer.Start();
        ragTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion

    #region Main Update
    private void Update()
    {
        // Updates the the jack state machine
        currentState.UpdateState();
    }
    #endregion

    #region Public Interface
    public void Attack()
    {
        // Debugging if check
        if (canAttack)
        {
            // Attack animation

            //Used to time attacks
            if (internalTimer.ElapsedMilliseconds - lastAttack > timeBetweenAttacks * 1000)
            {
                // Put actual attack code here (damage player, play animation, etc.)

                // Play attack audio
                // PlaySound(1);

                // Reassign last attack
                GetCurrentTime(true);
            }
        }
    }
    public void ChangeJackState(byte _index)
    {
        // Self explanatory
        currentState = availableStates[_index];
        currentState.Enable();
    }
    public void Falling()
    {
        // Move down
        transform.Translate(Vector3.down * currentForce * Time.deltaTime);

        // Down force intensifies
        currentForce += gravitationalForce;

        // If Jack is heigh enough, start jumping
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
            isGrounded = true;

            if (!StopJump())
                currentState.ChangeJackSubState(1);
        }
    }
    public void GetCurrentTime(bool _attack)
    {
        if (_attack)
            lastAttack = internalTimer.ElapsedMilliseconds;
        else
            lastJump = internalTimer.ElapsedMilliseconds;
    }
    public void Jumping()
    {
        // If on the ground and enough time has passed, jump
        if (isGrounded)
        {
            //if (internalTimer.ElapsedMilliseconds - lastJump > timeBetweenJumps * 1000)
            //{
                isGrounded = false;
                //GetCurrentTime(false);
            //}
        }
        else
        {
            // Move up
            transform.Translate(Vector3.up * currentForce * Time.deltaTime);

            if (horizontalMovement)
            {
                // Choose which direction to jump
                direction = (transform.position.x < ragTransform.position.x) ? Vector3.left : Vector3.right;

                // Move in Rag's direction
                transform.Translate(direction * horizontalMoveSpeed * Time.deltaTime);
            }
            
            // If Jack is heigher than this height, fall
            if (transform.position.y > maxJumpHeight)
                currentState.ChangeJackSubState(2);
        }
    }
    public void LookForRag()
    {
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If Rag is close enough
        if (distanceFromRag < minimumOpenDistance)
        {
            // If you see Rag, open up
            ChangeJackState(1);
        }
    }
    public void PlaySound(byte _index)
    {
        soundSource.clip = sounds[_index];
        soundSource.Play();
    }
    // This function is temporary, because this Rag's attack is not currently implemented
    public void ReceiveHit()
    {
        // Check distance between jack and rag
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        if (distanceFromRag < miniumReceiveHitDistance)
        {
            // Hit reaction
        }
    }
    public void ResetForce(bool _jumping) { currentForce = (_jumping) ? initialJumpForce : (byte)0; }
    public void StartJump()
    {
        // Debugging if check
        if (canJump)
        {
            distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

            // If Rag is close enough start jumping
            if (distanceFromRag < minimumJumpDistance)
                currentState.ChangeJackSubState(1);
        }
    }
    public bool StopJump()
    {
        distanceFromRag = Vector3.Distance(transform.position, ragTransform.position);

        // If Rag is far enough stop jumping
        if (distanceFromRag > minimumJumpDistance)
        {
            currentState.ChangeJackSubState(0);
            return true;
        }

        // If Rag isn't far enough
        return false;
    }
    #endregion
}