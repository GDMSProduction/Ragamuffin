//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//	08/13/2019 Colby Peck: Made enum for cat states. Made overload for SetCatState to take the enum. Changed all current calls to the overloaded method. 
//	08/14/2019 Colby Peck: Added AtDestination property and destinationDistance float. Added DistToRag helper property. Removed forward field (Vector3), added helper property WorldForward. Added SeesRag  helper property. 
//	08/14/2019 Colby Peck: Added try/catch statement to ChangeCatState. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //this needs to be removed when possible 

public sealed class CatManager : MonoBehaviour
{
	#region ToSort

	[SerializeField] private float destinationDistance = .1f;

	#endregion

	#region Serialized Variables
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
	[SerializeField] private float maxSearchDistance;                   // Max distance cat can be from Rag 
	[Header("How far cat can see")]
	[SerializeField] private byte sightDistance;                        // How far the cat can see 
	[Header("Max distance until cat is on screen")]
	[SerializeField] private float onScreenDistToRag;             // How close the cat needs to be on screen 
	[Header("Minimum distance cat can begin attacking")]
	[SerializeField] private float miniumAttackDistance;                // How close the cat needs to be to attach 
	[Header("Minimum distance cat can receive Rag damage")]
	[SerializeField] private float miniumReceiveHitDistance;            // How close rag has to be to hit the cat. Delete this when hit functionality is implemented 
	[Header("First speed is patrol. Second speed is chase")]
	[SerializeField] private float[] moveSpeeds;                        /// Make this into two floats; baseSpeed and purSuitSpeed (?)
	[SerializeField] private float timeBeforeFirstAttack;               // Self-explanatory 
	[SerializeField] private float timeBetweenAttacks;                  // Self-explanatory 
	[SerializeField] private float timeBetweenSearches;                 // Used to limit the number of raycasts per second 

	// Debugging Tools
	[SerializeField] private bool canAttack;
	[SerializeField] private bool canJump;
	[SerializeField] private bool canMove;
	[SerializeField] private bool canSearch;
	#endregion

	#region Private Variables
	private AudioSource soundSource;                                    // Sound controller for cat 
	private bool attacking;                                             // If cat has started attacking 
	private bool listeningForSound;                                              // If cat is checking for a sound 
	private bool fleeing;                                               // Flag for running away 
	private bool onScreen;                                              // On screen flag 
	private byte randomChance;                                          // Used to store the cat's decision to jump 

	// State Machine
	private CatState[] availableStates;
	private CatState currentState;

	private float currentAttackTime;                                    // Time between attacks 
	private float currentMoveSpeed;                                     // Current move speed 
	private float teleportDistance;                                     // The distance the cat must teleport to get just outside of the camera's view 
	private float verticalRepositionHeight;
	private long lastAttack;                                            // Stores the time of the last attack, to be used for the calculation of when it should attack again 
	private long lastRaycast;                                           // Stores the time of the last time it looked for Rag, to be used for the next time it looks 
	private RaycastHit hitInfo;                                       // Object intersecting raycast 
	private Stopwatch internalTimer;                                    /// Timing system needs to be reworked
	private Transform ragTransform;                                     /// Remove this when static player reference is made
	private Transform raycastEye;                                       // Raycast start position (cat's eye) 
	private Vector3 offset;                                             // Keeps the cat grounded for chase 
	private Vector3 targetPosition;
	#endregion

	#region Helper Methods and Properties
	/// <summary>
	/// Is the distance between our position and our destination less than our destinationDistance?
	/// </summary>
	private bool AtDestination
	{
		get
		{
			return (Vector3.Distance(transform.position, targetPosition) < destinationDistance); //If our distance between us and the destination is less than our destination distance, return true. Otherwise, return false. 
		}
	}

	/// <summary>
	/// Distance between our transform.position and Rag's transform.position
	/// </summary>
	private float DistToRag
	{
		get
		{
			return Vector3.Distance(transform.position, ragTransform.position); //Self-explanatory 
		}
	}

	/// <summary>
	/// Our transform.forward in context of world space
	/// </summary>
	private Vector3 ForwardInWorldSpace
	{
		get
		{
			return transform.TransformDirection(transform.forward); //Take our forward and translate it to world space
		}
	}

	/// <summary>
	/// Vector to Rag
	/// </summary>
	private Vector3 VecToRag
	{
		get
		{
			return ragTransform.position - transform.position; //Self-explanatory 
		}
	}

	/// <summary>
	/// Do we currently see Rag?
	/// </summary>
	private bool SeesRag
	{
		get
		{
			if (DistToRag < sightDistance) //If Rag is within our sight distance, 
			{
				UnityEngine.Debug.DrawLine(raycastEye.position, raycastEye.position + (ForwardInWorldSpace * sightDistance)); //For debugging, remove later. 
				if (Physics.Raycast(raycastEye.position, VecToRag.normalized, out hitInfo, sightDistance)) //If we fire a raycast and it hits something, 
				{

					if (hitInfo.collider.tag == "Player") //If the thing we hit has the player tag, 
					{
						if (!hitInfo.collider.transform.parent.GetComponent<PlayerManagerPDA>().GetHidden()) //If Rag isn't hidden, 
						{
							return true; //We see Rag. 
						}
					}
				}
			}
			return false; //Otherwise, we don't see Rag. 
		}
	}


	#endregion

	#region Initialization
	private void Awake()
	{
		soundSource = GetComponent<AudioSource>();
		fleeing = false;
		onScreen = true;

		// This calculation will always put the cat just off screen
		const float tDSetup = 25;
		teleportDistance = (maxSearchDistance - tDSetup);

		currentMoveSpeed = moveSpeeds[0];
		availableStates = new CatState[2] { new Unalerted(this), new Alerted(this) };
		currentState = availableStates[0];
		internalTimer = new Stopwatch();
		internalTimer.Start();
		ragTransform = GameObject.FindGameObjectWithTag("Player").transform;        //FIX THIS LINE JESUS CHRIST 
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
		if (AtDestination)
		{
			listeningForSound = false;
			ChangeCatState(CatStateType.Unalerted);
		}
	}

	/// <summary>
	/// Changes the cat's current state
	/// </summary>
	/// <param name="_index">What is the index of the state we're changing to?</param>
	public void ChangeCatState(byte _index)
	{
		// Self explanatory
		try
		{
			currentState = availableStates[_index];
			currentState.Enable();
		}
		catch (System.Exception e)
		{
			if (e is System.IndexOutOfRangeException)
			{
				///need to remove the System.Diagnostics using if we want to use Debug!!!
				//Debug.LogError("Tried to change to invalid state index: " + _index.ToString());
			}
			else
			{
				//Debug.LogError("CatManager.ChangeCatState(): Unforeseen exception generated: \n" + e.message)
			}
		}

	}

	/// <summary>
	/// Changes the cat's current state
	/// </summary>
	/// <param name="type">What type is the state we're changing to?</param>
	public void ChangeCatState(CatStateType type)
	{
		ChangeCatState((byte)type);
	}

	public void GetCurrentTime(bool _attack)
	{
		if (_attack)
		{
			lastAttack = internalTimer.ElapsedMilliseconds;
		}
		else
		{
			lastRaycast = internalTimer.ElapsedMilliseconds;
		}
	}
	public void PatrolMovement()
	{
		// Debugging if check 
		if (canMove)
		{
			// Moves cat towards target at the assigned move speed 
			Movement();
		}

		// If cat is on-screen  
		if (DistToRag < onScreenDistToRag)
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

			// If checking a position, we don't want cat to turn around or teleport 
			if (!listeningForSound)
			{
				if (DistToRag > maxSearchDistance)
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
		// If Rag is allowed to be lost
		if (ragIsLoseable)
		{
			// If Rag is far enough
			if (DistToRag > sightDistance)
			{
				// Change back to unalerted patrol
				ChangeCatState(CatStateType.Unalerted);
			}
		}

		// If not within attack range, get closer
		if (DistToRag > miniumAttackDistance)
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

		if (DistToRag < miniumReceiveHitDistance)
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
		listeningForSound = true;
		targetPosition = _targetPosition;
		ChangeCatState(CatStateType.Alerted);

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
		if (SeesRag) //If we see Rag, 
		{
			ChangeCatState(CatStateType.Alerted); //Become alerted 
			currentState.ChangeSubstate(0); // Change to pursuit behavior 
		}
	}
	private void Movement()
	{
		// Moves cat in move direction, at current speed
		if (!listeningForSound)
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);
		else
			transform.position = Vector2.Lerp(transform.position, targetPosition, currentMoveSpeed);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!listeningForSound)
		{
			//if (onScreen)
			//{
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
					ChangeCatState(CatStateType.Unalerted);
					fleeing = false;
				}
			}
			//}
		}
	}
	#endregion
}

public enum CatStateType : byte
{
	Unalerted = 0,
	Alerted = 1,
}