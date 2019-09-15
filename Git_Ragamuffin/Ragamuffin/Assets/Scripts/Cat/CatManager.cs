//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//	08/13/2019 Colby Peck: Made enum for cat states. Made overload for SetCatState to take the enum. Changed all current calls to the overloaded method. 
//	08/14/2019 Colby Peck: Added AtDestination property and destinationDistance float. Added DistToRag helper property. Removed forward field (Vector3), added helper property WorldForward. Added SeesRag helper property. 
//	08/14/2019 Colby Peck: Added try/catch statement to ChangeCatState. 
//	08/22/2019 Colby Peck:	 Made JumpLeft/Right and TurnLeft/Right methods. Cleaned up OnTriggerEnter.  Added State Dictionary with System.Type being the key. Added ChangeState overload for changing based on state type 
// 08/22/2019 Colby Peck: Built AddState method that takes in a Type of CatState. Changed state machine to use AddState and the new ChangeState<>() methods. 
// 08/22/2019 Colby Peck: Changed ragTransform from a field to a property 
// 08/27/2019 Colby Peck: Removed commented-out code block containing old ChangeState methods 
// 09/14/2019 Colby Peck: Added TODO list 
// 09/15/2019 Colby Peck: Added StateMachine class to clean up CatManager, moved all generic state machine functionality and data out of CatManager and into the new class 
// 09/15/2019 Colby Peck: Blocked out the structure of the new movement System; the methods and fields are put down, they need to be made functional. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
// [X] Move states from CatSubState to CatState, delete CatSubState.cs 
//	[X] Implement Substates as States
// [X] Refactor CatState 
//		[X] Make sure we can access our parent (AVOID CIRCULAR INCLUDES)
//		[X] Build methods for all the Monobehaviour methods we need (Update, FixedUpdate, etc.)
//		[X] Make sure those methods are getting called in CatManager 
// [X] Move Statemachine generic behaviour out of CatManager and into StateMachine 
// [X] Block out movement system 
//		[X] Write empty methods for the functionality we'll need 
//		[X] Figure out what fields and properties will be necessary, add them. Serialize as needed 
//		[X] Make sure states can access the required methods 
// [ ] Build functionality of the Movement system 
//		[ ] Build the cat's methods for perceiving the world 
//		[ ] Build Patrol state and test movement system 
// [ ] Make the other states 
//		[ ] Alerted 
//		[ ] Pusuit 
//		[ ] Dazed 
//		[ ] Flee
//		[ ] Teleport 

public class StateMachine : MonoBehaviour
{
	#region Fields and Properties
	private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();
	protected State currentState;
	protected virtual StateMachine thisMachine { get { return this; } }
	#endregion

	#region State Management
	protected void AddState<T>() where T : State, new() //T is a type that is required to be a CatState, and also have an empty constructor 
	{
		try
		{
			states.Add(typeof(T), new T()); //Try to add a new instance of the given state to the dictionary, use its type as the key 
			states[typeof(T)].Init(thisMachine); //Because we made the constructor parameterless, any variables that need to be set on initialization must be set manually 
		}
		catch (System.Exception e) //Upon exception, 
		{
			UnityEngine.Debug.LogError("CatManager.AddState(): Exception generated! \n" + e.Message); //Tell the console what's happened 
		}
	}
	public void ChangeState<T>()
	{
		try
		{
			currentState = states[typeof(T)];
			currentState.Enable();
		}
		catch (System.Exception e)
		{
			if (e is KeyNotFoundException)
			{
				UnityEngine.Debug.LogError("Tried to change to invalid state type: " + typeof(T));
			}
			else
			{
				UnityEngine.Debug.LogError("CatManager.ChangeCatState(): Unforeseen exception generated: \n" + e.Message);
			}
		}
	}
	#endregion

	#region Monobehaviour Methods
	protected virtual void Update()
	{
		currentState.Tick();
	}

	protected virtual void FixedUpdate()
	{
		currentState.FixedTick();
	}
	#endregion

}

public class CatManager : StateMachine
{

	protected override StateMachine thisMachine { get { return this; } }

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
	//private Dictionary<System.Type, CatState> states = new Dictionary<System.Type, CatState>();
	protected new CatState currentState { get { return (CatState)base.currentState; } set { base.currentState = value; } }


	private float currentAttackTime;                                    // Time between attacks 
	private float currentMoveSpeed;                                     // Current move speed 
	private float teleportDistance;                                     // The distance the cat must teleport to get just outside of the camera's view 
	private float verticalRepositionHeight;
	private long lastAttack;                                            // Stores the time of the last attack, to be used for the calculation of when it should attack again 
	private long lastRaycast;                                           // Stores the time of the last time it looked for Rag, to be used for the next time it looks 
	private RaycastHit hitInfo;                                       // Object intersecting raycast 
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
	/// The transform attached to <c>GameManager.Player</c>
	/// </summary>
	private Transform RagTransform { get { return GameManager.Player.transform; } }

	/// <summary>
	/// Distance between our transform.position and Rag's transform.position
	/// </summary>
	private float DistToRag
	{
		get
		{
			return Vector3.Distance(transform.position, RagTransform.position); //Self-explanatory 
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
			return RagTransform.position - transform.position; //Self-explanatory 
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

	JumpNodeBehavior turnNode;
	private void JumpRight(JumpNodeBehavior node)
	{
		turnNode = node;
		transform.position = new Vector3
		(
		transform.position.x - node.HorizontalDistance,
		node.VerticalRepositionHeight,
		transform.position.z
		);

	}
	private void JumpLeft(JumpNodeBehavior node)
	{
		turnNode = node;
		transform.position = new Vector3
		(
		transform.position.x - node.HorizontalDistance,
		node.VerticalRepositionHeight,
		transform.position.z
		);

	}

	private void TurnLeft()
	{
		transform.rotation = Quaternion.Euler(0, 270, 0);
	}
	private void TurnRight()
	{
		transform.rotation = Quaternion.Euler(0, 90, 0);
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

		AddState<Cat_Patrol>();
		AddState<Cat_Alerted>();
		AddState<Cat_Pursuit>();
		ChangeState<Cat_Patrol>();
		raycastEye = transform.GetChild(0);
		offset = new Vector3(0, -1, 0);

		// If start left is true in the inspector, the cat's will move left
		//transform.rotation = (startLeftDirection) ? Quaternion.Euler(0, 270, 0) : Quaternion.Euler(0, 90, 0);

		//Make sure this works asap
		if (startLeftDirection)
		{
			TurnLeft();
		}
		else
		{
			TurnRight();
		}
	}
	#endregion

	#region Public Interface
	public void CheckLocation()
	{
		// Still performs patrol like movement, but moves towards a specified point 
		PatrolMovement();

		// If cat has reached target, change back to unalerted 
		// State will change in patrol movement if Rag is found 
		if (AtDestination)
		{
			listeningForSound = false;
			ChangeState<Cat_Patrol>();
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
					if (transform.position.x < RagTransform.position.x)
						transform.position = new Vector3(transform.position.x + teleportDistance, transform.position.y, transform.position.z);
					else
						transform.position = new Vector3(transform.position.x - teleportDistance, transform.position.y, transform.position.z);
				}
			}
		}

		// Debugging if check
		if (canSearch)
		{

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
				ChangeState<Cat_Patrol>();
			}
		}

		// If not within attack range, get closer
		if (DistToRag > miniumAttackDistance)
		{
			// Look at Rag
			transform.LookAt(RagTransform.position + offset, Vector3.up);

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
			if (RagTransform.position.x < transform.position.x)
				transform.rotation = Quaternion.Euler(0, 90, 0);

			// If player is on the right of cat, move left
			else
				transform.rotation = Quaternion.Euler(0, 270, 0);

			// If cat is unalerted, toggle to alerted
			if (currentState is Cat_Patrol)
			{
				ChangeState<Cat_Alerted>();
			}

			fleeing = true;
			attacking = false;
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
		ChangeState<Cat_Alerted>();
	}
	#endregion

	#region Private
	private void Attack()
	{
		if (attacking)
		{
			//Used to time attacks
			//if (internalTimer.ElapsedMilliseconds - lastAttack > currentAttackTime * 1000)
			//{
			//	// Put actual attack code here (damage player, play animation, etc.)

			//	// Play attack audio
			//	PlaySound(1);

			//	// Reassign last attack
			//	GetCurrentTime(true);

			//	if (currentAttackTime != timeBetweenAttacks)
			//		currentAttackTime = timeBetweenAttacks;
			//}

			//return;
		}

		// Only makes it here before cat attacks
		// Reset timer
		//GetCurrentTime(true);

		// Allow cat to attack on the next pass
		attacking = true;

		// Allows for differnt first attack time
		currentAttackTime = timeBeforeFirstAttack;
	}
	private void LookForRag()
	{
		//if (SeesRag) //If we see Rag, 
		//{
		//	ChangeCatState<Alerted>();
		//	currentState.ChangeSubstate(0); // Change to pursuit behavior 
		//}
	}
	private void Movement()
	{
		// Moves cat in move direction, at current speed
		if (!listeningForSound)
		{
			transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);
		}
		else
		{
			transform.position = Vector2.Lerp(transform.position, targetPosition, currentMoveSpeed);
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!listeningForSound)
		{
			if (other.tag == "JumpPoint")
			{
				if (canJump)    // Debugging if check
				{
					if (currentState.GetType() == typeof(Cat_Patrol)) // If patrolling
					{
						randomChance = (byte)Random.Range(0, jumpDropChance);   // Retrieving random cat decision

						if (randomChance == 0)  // If cat decides to jump/drop
						{
							if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 1)   // If left side activation
							{
								JumpLeft(other.GetComponent<JumpNodeBehavior>());
							}
							else if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 2)  // If right side activation
							{
								JumpRight(other.GetComponent<JumpNodeBehavior>());
							}
							else if (transform.rotation == Quaternion.Euler(0, 90, 0))  // This type of point doesn't care the direction, it will reflect regardless 
							{
								JumpLeft(other.GetComponent<JumpNodeBehavior>());
							}
							else
							{
								JumpRight(other.GetComponent<JumpNodeBehavior>());
							}
						}
					}
				}
			}
		}
		else if (other.tag == "TurnPoint")
		{
			if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 1)    // If right side activation
			{
				if (transform.position.x < other.transform.position.x)  // If cat is on the left side of this point, make it move left
				{
					TurnLeft();
				}
				else
				{
					return; // Act like nothing happened
				}
			}
			else if (other.GetComponent<NodeBehaviorBase>().GetActivationSide() == 2)   // If left side activation
			{
				if (transform.position.x > other.transform.position.x)  // If cat is on the right side of this point, make it move right
				{
					TurnRight();
				}
				else
				{
					return; // Act like nothing happened
				}
			}
			else    // This type of point doesn't care the direction, it will reflect regardless
			{
				if (transform.rotation == Quaternion.Euler(0, 90, 0)) //if we're facing right, 
				{
					TurnLeft();
				}
				else
				{
					TurnRight();
				}
			}
			if (fleeing)    // If fleeing, change to unalerted
			{
				// Stop fleeing
				ChangeState<Cat_Patrol>();
				fleeing = false;
			}
		}
	}
	#endregion


	#region New Movement System

	//May be useful; if not, remove later 
	public delegate void delegate_ReachedDesitnation();
	public event delegate_ReachedDesitnation ReachedDestination;

	//fields needed for determining behaviour
	[SerializeField] private float destinationDistance = .5f;   //How close we need to be to our destination before we say that we've reached it

	[SerializeField] private float baseMoveSpeed, pursuitMoveSpeed; //move speeds in units per second
	private const float moveSpeedMultiplier = 1f / 50f; //This allows us to convert our units per second values into units per FixedUpdate tick without doing the division every tick. Division is kinda expensive. 

	private Vector3 destination;
	/// <summary>
	/// Sets the cat's destination to a given Vector3
	/// </summary>
	/// <param name="destination">Where are we moving to?</param>
	public void SetDestination(Vector3 destination)
	{
		// Tell our movement system to move towards the destination. 
	}



	private void Move()
	{
		//Move towards our destination
		
		//If we reach our destination, tell any listeners of our event that we reached it 
		if (AtDestination)
		{
			if (ReachedDestination.GetInvocationList().Length > 0) //If we have any listeners, 
			{
				try
				{
					ReachedDestination.Invoke(); //Try to invoke our listeners 
				}
				catch (System.Exception e) //Upon exception, 
				{
					//Tell the console what happened 
					if (e is System.NullReferenceException)
					{
						Debug.LogError("Catmanager.Movement(): Listener of reachedDestination found to be null!\n" + e.Message);
					}
					else
					{
						Debug.LogError("Catmanager.Movement(): unforeseen exception generated!\n" + e.Message);
					}

					return; //Arrest the code 
				}
			}
		}
	}

	#endregion
}