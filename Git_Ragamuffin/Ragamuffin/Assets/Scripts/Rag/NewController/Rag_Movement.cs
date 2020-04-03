using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//					  Author:	Robert Bauerle  
//                      Date:	7/16/2019  
//					Purpose:	A movement script that functions as a player controller 
// Associated Scripts:	RB_Grapple  
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

[RequireComponent(typeof(BoxCollider))]
public class Rag_Movement : MonoBehaviour
{
	#region Serialized Variables
	[Header("Movement")]

	[Tooltip("Maximum speed at which Rag moves horizontally")]
	[Range(0.1f, 10)]
	[SerializeField]
	private float maxSpeedHorizontal;

	[Tooltip("Maximum speed at which Rag moves vertically.")]
	[Range(0.1f, 10)]
	[SerializeField] private float maxSpeedVertical;

	[Tooltip("How quickly Rag accelerates when moving based on input.")]
	[Range(0.05f, 10)]
	[SerializeField] private float horizontalAcceleration = 0.1f;

	[Tooltip("How quickly rag slows down when on the ground and not moving")]
	[Range(0.1f, 1)]
	[SerializeField] private float friction = .2f;

	[Tooltip("The maximum slope angle that rag can climb up")]
	[Range(1, 89)]
	[SerializeField] private float maxClimbAngle;

	[Space(2)]
	[Header("Jump")]

	[Tooltip("How high Rag can jump.")]
	[Range(0.1f, 20)]
	[SerializeField] private float maxJumpHeight;

	[Tooltip("How fast Rag jumps.")]
	[Range(0.1f, 10)]
	[SerializeField] private float jumpStrength;

	//[Tooltip("A multiplier for all movement-related numbers, gravity included.")]
	//[Range(0.1f, 5)]
	//[SerializeField] private float ragMass = 2;

	[Space(2)]
	[SerializeField] LayerMask collisionMask;
	[Tooltip("Rag's Mesh")]
	[SerializeField] Transform meshTransform;

	#endregion

	#region Variables

	[SerializeField]
	[Tooltip("Serialized for visibility. Do not edit in inspector!")]
	private Vector2 input;

	private const float offsetWidth = 0.1f;
	private const int precision = 10;
	private float boundsY;

	/// <summary>
	/// Whether or not rag is currently affected by gravity
	/// </summary>
	public bool useGravity = true;

	/// <summary>
	/// Disable the control over rag's movement from the player's input
	/// </summary>
	public bool disableControls = false;

	public delegate void MovementDel(ref Vector2 velocity);
	public static MovementDel preTranslateEvent;

	#endregion

	#region Movement Variables
	private Vector2 pVelocity;
	public Vector2 Velocity { get { return pVelocity; } }

	private Vector2 pAcceleration;
	private Vector2 gravityAcceleration;

	[SerializeField] private float gravityConst = -.5f;
	//private float GravityForce { get { return gravityConst * ragMass; } }

	private RaycastOffsets offsets;
	#endregion

	#region Mono Methods
	private void Start()
	{
		offsets = new RaycastOffsets(transform.position, GetComponent<BoxCollider>().bounds, offsetWidth, precision);
		Bounds bounds = GetComponent<BoxCollider>().bounds;
		bounds.Expand(offsetWidth * -2);
		boundsY = bounds.extents.y;
		pAcceleration = Vector2.zero;
	}

	private void FixedUpdate()
	{
		DetectGround();
		DetectInput();

		CollisionCheck();
		ApplyInternalForces();
		ApplyGravity();
		// PreMovement.Invoke();
		ApplyExternalForces(); 

		if (preTranslateEvent != null)
			preTranslateEvent(ref pVelocity);

		ApplyInternalVelocity();

		SetRotation();
	}

	private void ApplyExternalForces()
	{
	
	}

	#endregion


	#region Input Detection

	private void DetectInput()
	{
		//Get and save the input for later use from the axis. We are getting a raw X because of the smooth damping done to emulate acceleration.
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");
	}

	private void SetRotation()
	{
		if (disableControls)
		{
			SetRotationFromX(Velocity.x);
		}
		else
		{
			SetRotationFromX(input.x);
		}
	}

	private void SetRotationFromX(float x)
	{
		if (x > 0)
		{
			meshTransform.rotation = Quaternion.Euler(0, 270, 0);
		}
		else if (x < 0)
		{
			meshTransform.rotation = Quaternion.Euler(0, 90, 0);
		}
	}

	#region Jumping
	private float curJumpHeight;

	private void ResetJumpHeight()
	{
		if (OnGround)
			curJumpHeight = 0;
		else
			curJumpHeight = maxJumpHeight;
	}

	#endregion


	#endregion

	#region Ground Detection
	public bool OnGround { get; private set; }
	private void DetectGround()
	{
		foreach (int i in offsets.down)
		{
			if (Physics.Raycast(transform.position + offsets[i], Vector3.down, offsetWidth * 3, collisionMask))
			{
				OnGround = true;
				return;
			}
		}
		OnGround = false;
	}

	#endregion

	#region Movement
	#region Velocity Helper Functions
	public void AddVelocity(Vector2 change)
	{
		pVelocity += change;
	}

	#region Additional Helper Functions for ChangeVelocity()
	public void AddVelocity(float x = 0, float y = 0)
	{
		AddVelocity(new Vector2(x, y));
	}

	/// <summary>
	/// Set the velocity equal to the inputted vector.
	/// </summary>
	/// <param name="value">the new value for velocity</param>
	public void SetVelocity(Vector3 value)
	{
		pVelocity = value;
	}

	/// <summary>
	/// Set the Y value of the velocity equal to the inputted float.
	/// </summary>
	/// <param name="value">the new Y value in velocity</param>
	public void SetVelocityY(float value)
	{
		pVelocity.y = value;
	}

	#endregion

	#endregion

	private void ApplyInternalForces()
	{
		DetectInput();
		CalculateVelocityHorizontal();
		CalculateVelocityVertical();

		pVelocity += pAcceleration;
	}

	private void CalculateVelocityHorizontal()
	{
		if (disableControls)
			return;

		if (Mathf.Abs(input.x) > 0) //If there's any amount of x input, 
		{
			pAcceleration.x += input.x * horizontalAcceleration * Time.fixedDeltaTime; //Apply it 
		}
		else if (OnGround) //If no input, and we're on the ground, 
		{
			ApplyFrictionHorizontal();
		}

		//Do not accelerate if we're at or past our max speed 
		if (Mathf.Abs(pVelocity.x + pAcceleration.x) >= maxSpeedHorizontal)
		{
			pAcceleration.x = 0;
		}
	}

	private void ApplyFrictionHorizontal()
	{
		pVelocity.x += -pVelocity.x * Time.fixedDeltaTime * friction; //Apply some amount of friction based on the movement

		//Make sure there's a minimum amount of friction applied that isn't scaled by the player's current velocity 
		if (pVelocity.x < 0)
		{
			pVelocity.x += 0.04f;
		}
		else
		{
			pVelocity.x += -0.04f;
		}

		if (Mathf.Abs(pVelocity.x) < .05f) //If we're moving really slow, 
		{
			//Stop all movement and acceleration.
			pAcceleration.x = 0;
			pVelocity.x = 0;
		}
	}

	private void CalculateVelocityVertical()
	{
		if (disableControls)
		{
			pAcceleration.y = 0;
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			pAcceleration.y += jumpStrength * 5;
		}

		if (Input.GetKey(KeyCode.Space) && curJumpHeight <= maxJumpHeight)
		{
			pAcceleration.y += jumpStrength * Time.fixedDeltaTime;
			curJumpHeight += pVelocity.y;
		}
		else
		{
			ResetJumpHeight();
			pAcceleration.y = 0;
		}

		if (Mathf.Abs(pAcceleration.y + pVelocity.y) > maxSpeedVertical)
		{
			pAcceleration.y = 0;
		}
	}

	private void ApplyGravity()
	{
		if (!useGravity)
			return;

		if (!OnGround)
		{
			gravityAcceleration.y += gravityConst * Time.fixedDeltaTime;
		}
		else
		{
			gravityAcceleration.y = 0;
		}

		if (Mathf.Abs(gravityAcceleration.y + pVelocity.y) > maxSpeedVertical)
		{
			gravityAcceleration.y = 0;
		}

		pVelocity += gravityAcceleration;
	}

	private void ApplyInternalVelocity()
	{
		AddVelocity(pAcceleration);

		//Prevent ridiculously small values from being used when the character is not being moved 
		if (Mathf.Abs(pVelocity.x) < .015f)
			pVelocity.x = 0;
		if (Mathf.Abs(pVelocity.y) < .015f)
			pVelocity.y = 0;
		
		if(pVelocity.y < 0 && OnGround)
		{
			pVelocity.y = 0;
		}

		//Move the player based on the final velocity from input and collision 
		transform.Translate(pVelocity);
	}

	private void Climb(ref Vector2 vel, float angle)
	{
		float Y = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(vel.x);
		if (vel.y < Y)
		{
			vel.y = Y;
			OnGround = true;
		}
		vel.x = Mathf.Cos(angle * Mathf.Deg2Rad) * vel.x;
	}
	#endregion

	#region Collision
	private void CollisionCheck()
	{
		YCollisions();
		XCollisions();
	}

	#region Raycasting Helper Functions
	RaycastHit hit;
	private void YCollisions()
	{
		float rayLength = Mathf.Abs(pVelocity.y) + offsetWidth;
		if (pVelocity.y < 0)
		{
			foreach (int i in offsets.down)
			{
				if (GetHit(offsets[i], Vector3.down, rayLength, ref pVelocity, out hit))
				{
					if (hit.transform.GetComponent<DeathVolume>())
					{
						GetComponent<ReSpawnManager>().ReSpawn();
						SetVelocity(Vector3.zero);
						return;
					}
					pVelocity.y = (hit.distance - offsetWidth) * -1;
					rayLength = hit.distance;
				}
			}
		}
		else if (pVelocity.y > 0)
		{
			foreach (int i in offsets.top)
			{
				if (GetHit(offsets[i], Vector3.up, rayLength, ref pVelocity, out hit))
				{
					if (hit.transform.GetComponent<DeathVolume>())
					{
						GetComponent<ReSpawnManager>().ReSpawn();
						SetVelocity(Vector3.zero);
						return;
					}
					pVelocity.y = hit.distance - offsetWidth;
					rayLength = hit.distance;
				}
			}
		}
	}

	private void XCollisions()
	{
		float rayLength = Mathf.Abs(pVelocity.x) + offsetWidth;
		if (pVelocity.x < 0)
		{
			for (int i = 0; i < offsets.left.Length; i++)
			{
				if (GetHit(offsets[offsets.left[i]], Vector3.left, rayLength, ref pVelocity, out hit))
				{
					if (hit.transform.GetComponent<DeathVolume>())
					{
						GetComponent<ReSpawnManager>().ReSpawn();
						SetVelocity(Vector3.zero);
						return;
					}
					float angle = Vector3.Angle(hit.normal, Vector3.up);

					if (i < precision && angle < maxClimbAngle)
					{
						Climb(ref pVelocity, angle);
						rayLength = 0;
					}
					else
					{
						pVelocity.x = (hit.distance - offsetWidth) * -1;
						rayLength = hit.distance;
					}

				}
			}
		}
		else if (pVelocity.x > 0)
		{
			for (int i = 0; i < offsets.right.Length; i++)
			{
				if (GetHit(offsets[offsets.right[i]], Vector3.right, rayLength, ref pVelocity, out hit))
				{
					if (hit.transform.GetComponent<DeathVolume>())
					{
						GetComponent<ReSpawnManager>().ReSpawn();
						SetVelocity(Vector3.zero);
						return;
					}
					float angle = Vector3.Angle(hit.normal, Vector3.up);

					if (i < precision && angle < maxClimbAngle)
					{
						Climb(ref pVelocity, angle);
						rayLength = 0;
					}
					else
					{
						pVelocity.x = hit.distance - offsetWidth;
						rayLength = hit.distance;
					}

				}
			}
		}
	}

	/// Helper function for the collision functions
	/// </summary>
	private bool GetHit(Vector3 offset, Vector3 dir, float length, ref Vector2 vel, out RaycastHit hit)
	{
		Debug.DrawLine(transform.position + offset, transform.position + offset + (dir * (length)), Color.red);
		return Physics.Raycast(transform.position + offset, dir, out hit, length, collisionMask);
	}
	#endregion

	#region structs

	/// <summary>
	/// A container for a bunch of vector3 offsets to save on processing later without having to recalculate them
	/// </summary>
	private struct RaycastOffsets
	{
		public int precision { get; private set; }

		Vector3[] offsets;
		public Vector3 this[int index]
		{
			get
			{
				if (index >= offsets.Length)
				{
					Debug.LogWarning("Attempted to access offsets array with an index out of range. Given index: " + index);
					return Vector3.zero;
				}

				return offsets[index];
			}
		}
		public int[] top, down, left, right, forward, back;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bounds">The bounds of the collider that will be used to find each offset position</param>
		/// <param name="offsetWidth">A multiplier for each offset to have the points be slightly inside of the collider</param>
		public RaycastOffsets(Vector3 pos, Bounds bounds, float offsetWidth, int precision = 3, bool use2D = true)
		{

			precision = Mathf.Clamp(precision, 3, int.MaxValue);
			this.precision = precision;
			bounds.Expand(offsetWidth * -2);
			#region Array initialization
			offsets = new Vector3[(precision * precision * 6) - (precision * 12) + 8];
			int t = 0, d = 0, l = 0, r = 0, f = 0, b = 0;
			top = new int[precision * precision];
			down = new int[precision * precision];
			left = new int[precision * precision];
			right = new int[precision * precision];
			forward = new int[precision * precision];
			back = new int[precision * precision];
			#endregion
			int cur = 0;
			float spacingX = bounds.size.x / (precision - 1);
			float spacingY = bounds.size.y / (precision - 1);
			float spacingZ = bounds.size.z / (precision - 1);

			Vector3 min = (Vector3.back * bounds.extents.z) + (Vector3.down * bounds.extents.y) + (Vector3.left * bounds.extents.x) + (bounds.center - pos);
			for (int i = 0; i < precision; i++)
			{
				for (int ii = 0; ii < precision; ii++)
				{
					for (int iii = 0; iii < precision; iii++)
					{
						if (i > 0 && i < precision - 1 && ii > 0 && ii < precision - 1 && iii > 0 && iii < precision - 1)
							continue;
						offsets[cur] = min + (Vector3.right * spacingX * ii) + (Vector3.forward * spacingZ * i) + (Vector3.up * spacingY * iii);
						if (i == 0)
						{
							back[b] = cur;
							b++;
						}
						else if (i == precision - 1)
						{
							forward[f] = cur;
							f++;
						}
						if (ii == 0)
						{
							left[l] = cur;
							l++;
						}
						else if (ii == precision - 1)
						{
							right[r] = cur;
							r++;
						}
						if (iii == 0)
						{
							down[d] = cur;
							d++;
						}
						else if (iii == precision - 1)
						{
							top[t] = cur;
							t++;
						}
						cur++;
					}
				}
			}

			//Ordering arrays so that the first indexes in the array are the points that are on the bottom row of the face
			OrderArray(ref left, min.y);
			OrderArray(ref right, min.y);
			OrderArray(ref forward, min.y);
			OrderArray(ref back, min.y);
		}

		private void OrderArray(ref int[] array, float y)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (offsets[array[i]].y == y)
				{
					for (int ii = 0; ii < array.Length; ii++)
					{
						if (offsets[array[ii]].y == y)
							continue;
						int temp = array[i];
						array[i] = array[ii];
						array[ii] = temp;
					}
				}
			}
		}
	}
	#endregion

	#endregion
}