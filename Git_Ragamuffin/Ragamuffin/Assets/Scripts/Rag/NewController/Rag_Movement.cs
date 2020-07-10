﻿using System;
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
	[SerializeField] LayerMask collisionMask;
	[Tooltip("Rag's Mesh")]
	[SerializeField] Transform meshTransform;

	[Tooltip("This float is compared against the dot product of collisions to determine if we've hit the ground or a wall.")]
	[SerializeField] private readonly float downwardAngle = -.75f;
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
			if (rb == null)
				return Vector3.zero;
			return rb.velocity;
		}
		set
		{
			if (rb != null)
				rb.velocity = value;
		}
	}

	public bool UseGravity
	{
		get
		{
			if (rb == null)
				return true;
			return rb.useGravity;
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
		ApplyInput();
		SetRotation();

		if (printLogs)
			Debug.Log("Rag_Movement rb velocity: " + rb.velocity);
	}

	private void OnCollisionEnter(Collision collision)
	{
		Vector3 vecToCollision = collision.contacts[0].point - transform.position; //Draw vector from us to the point of collision 
		vecToCollision.Normalize(); //We only want to use this vector as a direction, we don't want the magnitude. 
		float dot = Vector3.Dot(transform.up, vecToCollision); //If the collision is perfectly underneath us, this will give us a result of -1. 
		OnGround = (dot < -.75f);

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


	[SerializeField]
	[Tooltip("Serialized for visibility. Do not edit in inspector!")]
	private float xInput;

	private void ApplyInput()
	{
		if (disableControls) //Do nothing if controls are disabled 
			return;

		xInput = Input.GetAxisRaw("Horizontal");

		if (Mathf.Abs(rb.velocity.x) < maxSpeedHorizontal) //If we aren't at max speed, 
			rb.AddForce(xInput * horizontalAcceleration, 0, 0, ForceMode.Acceleration); //Accelerate as according to input 

		if (Input.GetKey(KeyCode.Space))
			Jump();
	}

	private void Jump()
	{
		if (OnGround)
			if (Input.GetKeyDown(KeyCode.Space))
				rb.AddForce(0, initialJumpForce, 0, ForceMode.VelocityChange);

		if (Mathf.Abs(rb.velocity.y) < maxSpeedVertical && curJumpHeight < maxJumpHeight)
			rb.AddForce(0, jumpStrength, 0, ForceMode.VelocityChange);
	}

}