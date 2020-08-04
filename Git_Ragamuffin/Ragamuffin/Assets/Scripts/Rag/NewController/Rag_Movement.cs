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

	[Tooltip("Print debug logs?")]
	[SerializeField] bool printLogs = false;

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

	[Tooltip("Should rag's horizontal movement stop the instant the player stops giving horizontal input?")]
	[SerializeField] bool stopImmediately = false;

	[Tooltip("This number determines how quickly rag comes to a stop. Doesn't do anything if stopImmediately is checked.")]
	[SerializeField] private float horizontalFriction = .5f;


	[Space(2)]
	[Header("Jump")]

	[Tooltip("How high Rag can jump.")]
	[Range(0.1f, 20)]
	[SerializeField] private float maxJumpHeight;

	[Tooltip("This force will be applied immediately when the player presses space")]
	[SerializeField] private float initialJumpForce = 10f;

	[Tooltip("This force will be applied continuously if the player continues to hold space after jumping")]
	[Range(0.1f, 10)]
	[SerializeField] private float jumpStrength;

	[Space(2)]
	[Tooltip("Rag's Mesh")]
	[SerializeField] Transform meshTransform;

	[Tooltip("This float is compared against the dot product of collisions to determine if we've hit the ground and should reset our jump")]
	[SerializeField] private float downwardAngle = -.5f;

	[SerializeField]
	[Header("Serialized for visibility. Do not edit in inspector!")]
	private float xInput;
	#endregion

	#region Variables

	/// <summary>
	/// Determines if input moves Rag.
	/// </summary>
	public static bool disableControls = false;
	private Rigidbody rb = null;
	#endregion

	#region Properties
	/// <summary> Velocity of Rag's Rigidbody </summary>
	public Vector2 Velocity
	{
		get
		{
			if (rb != null)
				return rb.velocity;
			else
				return Vector3.zero; //default to 0 if our rigidbody reference isn't set yet 
		}
		set
		{
			if (rb != null)
				rb.velocity = value;
		}
	}

	/// <summary>
	/// useGravity bool of Rag's Rididbody
	/// </summary>
	public bool UseGravity
	{
		get
		{
			if (rb != null)
				return rb.useGravity;
			else
				return true; //default to true if our rigidbody reference isn't set yet 
		}
		set
		{
			if (rb != null)
				rb.useGravity = value;
		}
	}


	#endregion

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		yPosLastFrame = transform.position.y;
	}

	private void FixedUpdate()
	{
		UpdateJumpHeight();

		if (!stopImmediately)
			ApplyFriction();

		ApplyInput();
		SetRotation();
	}
	private void ApplyInput()
	{
		if (disableControls) //Do nothing if controls are disabled 
			return;

		ApplyHorizontalInput();

		if (Input.GetKey(KeyCode.Space))
			Jump();
	}

	private void ApplyHorizontalInput()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		Vector3 forceToAdd = new Vector3(xInput * horizontalAcceleration, 0, 0);

		if (Mathf.Abs(rb.velocity.x) < maxSpeedHorizontal) //If we're below max speed, 
			rb.AddForce(forceToAdd, ForceMode.VelocityChange); //Accelerate as according to input 
		else //if we're above or at max speed, 
		{
			float x = Mathf.Clamp(rb.velocity.x, -maxSpeedHorizontal, maxSpeedHorizontal); //clamp our speed to our max. 
			rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);
		}

		if (xInput == 0 && stopImmediately) //If we're recieving no input and we're supposed to stop immediately, 
		{
			Vector3 newVelocity = new Vector3(0, rb.velocity.y, rb.velocity.z); //Zero out our velocity along the x axis 
			rb.velocity = newVelocity;
		}
	}

	private void Jump()
	{
		if (OnGround)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (printLogs)
					Debug.Log("Rag_Movement: Jumping off of ground!\nForce of jump: " + initialJumpForce);

				rb.AddForce(0, initialJumpForce, 0, ForceMode.VelocityChange);
			}
		}

		if (Mathf.Abs(rb.velocity.y) < maxSpeedVertical && curJumpHeight < maxJumpHeight)
		{
			if (printLogs)
				Debug.Log("Rag_Movement: Continuing jump while in air!\nForce of jump: " + jumpStrength);

			rb.AddForce(0, jumpStrength, 0, ForceMode.VelocityChange);
		}
	}


	private void ApplyFriction()
	{
		if (Mathf.Abs(rb.velocity.x) < 1) //if we're moving really slowly, 
			return; //Don't bother with friction. 

		bool velocityIsNegative = rb.velocity.x < 0;

		if (velocityIsNegative)
		{
			rb.AddForce(horizontalFriction, 0, 0, ForceMode.Acceleration);
		}
		else
		{
			rb.AddForce(-horizontalFriction, 0, 0, ForceMode.Acceleration);
		}

	}

	private void OnCollisionEnter(Collision collision)
	{

		Vector3 vecToCollision = collision.contacts[0].point - transform.position; //Draw vector from us to the point of collision 
		vecToCollision.Normalize(); //We only want to use this vector as a direction, we don't want the magnitude. 
		float dot = Vector3.Dot(-transform.up, vecToCollision); //If the collision is perfectly underneath us, this will give us a result of -1. 
		OnGround = (dot > downwardAngle);

		if (printLogs)
			Debug.Log("Collided, dot = " + dot);

		if (OnGround)
			ResetJumpHeight();
	}

	float yPosLastFrame;
	void UpdateJumpHeight()
	{
		float difference = transform.position.y - yPosLastFrame;

		if (difference > 0) //don't want to subtract from the recorded jump height when falling. 
			curJumpHeight += difference;

		yPosLastFrame = transform.position.y;
	}

	private void SetRotation()
	{
		if (disableControls)
		{
			SetRotation(Velocity.x);
		}
		else
		{
			SetRotation(xInput);
		}
	}

	private void SetRotation(float x)
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
	private float curJumpHeight;

	private void ResetJumpHeight()
	{
		if (OnGround)
			curJumpHeight = 0;
		else
			curJumpHeight = maxJumpHeight;
	}

	public bool OnGround { get; private set; }
}