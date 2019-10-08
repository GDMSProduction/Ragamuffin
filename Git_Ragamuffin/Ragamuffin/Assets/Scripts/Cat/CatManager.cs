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
// 09/28/2019 Colby Peck: Removed just about everything for total system rework, moved all movement functionality to CatMover.cs 
// 09/29/2019 Colby Peck: Added printLogs bool to enable/disable debug logs 
// 10/07/2019 Colby Peck: Removed old fields; added new fields and properties for patrol functionality 


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


public abstract class StateMachine : MonoBehaviour
{

	#region Fields and Properties
	[SerializeField]
	[Tooltip("Print debug logs?")]
	protected bool printLogs = false;

	private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();
	protected State currentState, previousState;

	protected virtual StateMachine thisMachine { get { return this; } }
	public System.Type PreviousStateType
	{
		get
		{
			if (previousState != null)
			{
				return previousState.GetType();
			}
			else
			{
				return null;
			}
		}
	}
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
			previousState = currentState;
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
	//Serialized fields 
	[Header("Patrol variables")]
	[Tooltip("What point should I start the level at?")]
	[SerializeField] private CatPatrolPoint firstPoint;

	[Tooltip("How long should I wait at every patrol checkpoint before moving to the next one?")]
	[SerializeField] float patrolWaitTime = 1f;

	[Tooltip("Should I go through the patrol points in reverse order when I reach the last one?\nIf not, I'll return to the first one when I reach the end.")]
	[SerializeField] bool reversePatrolWhenDone = true;


	//Public properties for states to access 
	public CatPatrolPoint FirstPoint { get { return firstPoint; } }
	public CatPatrolPoint CurrentPoint { get; set; }
	public float PatrolWaitTime { get { return patrolWaitTime; } }
	public bool ReversePatrolWhenDone { get { return reversePatrolWhenDone; } }
	#endregion

	#region Private Variables
	private AudioSource soundSource;                                    // Sound controller for cat 
	public CatMover Mover { get; private set; }
	public CatAnimatorManager animManager { get; private set; }
	protected new CatState currentState { get { return (CatState)base.currentState; } set { base.currentState = value; } }


	#endregion

	#region Helper Methods and Properties


	/// <summary>
	/// The transform attached to <c>GameManager.Player</c>
	/// </summary>
	private Transform RagTransform
	{
		get
		{
			if (GameManager.Player)
				return GameManager.Player.transform;
			return null;
		}
	}

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
	/// Vector to Rag
	/// </summary>
	private Vector3 VecToRag
	{
		get
		{
			if (RagTransform)
				return RagTransform.position - transform.position;

			return Vector3.zero;
		}
	}

	///// <summary>
	///// Do we currently see Rag?
	///// </summary>
	//private bool SeesRag
	//{
	//	get
	//	{
	//		if (DistToRag < sightDistance) //If Rag is within our sight distance, 
	//		{
	//			UnityEngine.Debug.DrawLine(raycastEye.position, raycastEye.position + (ForwardInWorldSpace * sightDistance)); //For debugging, remove later. 
	//			if (Physics.Raycast(raycastEye.position, VecToRag.normalized, out hitInfo, sightDistance)) //If we fire a raycast and it hits something, 
	//			{

	//				if (hitInfo.collider.tag == "Player") //If the thing we hit has the player tag, 
	//				{
	//					if (!hitInfo.collider.transform.parent.GetComponent<PlayerManagerPDA>().GetHidden()) //If Rag isn't hidden, 
	//					{
	//						return true; //We see Rag. 
	//					}
	//				}
	//			}
	//		}
	//		return false; //Otherwise, we don't see Rag. 
	//	}
	//}

	#endregion

	#region Initialization
	private void Awake()
	{
		animManager = GetComponent<CatAnimatorManager>();
		animManager.Init();
		animManager.printLogs = printLogs;

		Mover = GetComponent<CatMover>();
		Mover.Init(animManager);
		Mover.printLogs = printLogs;

		soundSource = GetComponent<AudioSource>();

		if (firstPoint)
		{
			CurrentPoint = FirstPoint;
			transform.position = CurrentPoint.Position;
		}
		else
		{
			Debug.LogWarning("CatManager.Awake(): No first patrol point set!\nCat cannot function without one, destroying cat.");
			Destroy(gameObject);
			return;
		}
		AddState<Cat_Patrol>();
		AddState<Cat_Alerted>();
		AddState<Cat_Pursuit>();
		AddState<Cat_Dazed>();
		AddState<Cat_Flee>();
		AddState<Cat_Teleport>();

		ChangeState<Cat_Patrol>();

	}
	#endregion

	#region Public Interface
	public void CheckLocation()
	{
		// If cat has reached target, change back to unalerted 
		// State will change in patrol movement if Rag is found 
	}
	public void PlaySound(byte _index)
	{
		soundSource.Play();
	}

	public void ReceiveHit()
	{



	}
	public void RunAway()
	{

	}
	public void StartCheck(Vector3 _targetPosition)
	{
		//Check for rag at a given position 
	}
	#endregion

	#region Private
	private void Attack()
	{

	}

	#endregion


}