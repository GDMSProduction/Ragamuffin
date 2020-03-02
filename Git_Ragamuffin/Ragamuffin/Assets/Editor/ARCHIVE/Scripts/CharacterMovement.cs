using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enum used in the AddForce function for Ragamuffin. Made by
/// </summary>
public enum ForceType { Acceleration, Impulse}

public class CharacterMovement : MonoBehaviour
{
	// TASK LIST
	/* System that can take in multiple inputs and at the end outputs one movement vector.
	 * Force.Acceleration (Applies the force over time, ignoring mass. Used in gravity and constant force cases.)
	 * Force.Impulce (Applies all of the force immediately)
	 * public SetVelocity function (changing this should update the values being used in fixed update. They should instantly change to that new speed)
	 * Gravity
	 * inAir bool
	 * Mass (to be used in force calculations)
	 * Collision
	 */

	readonly float g = 9.81f;
	float velocity = 0.0f;
	float meterDist = 0.0f;
	float meterPerc = 0.0f;
	Vector3 lastUpdatePos;

	float velocityAdditive = 0.0f;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	private void FixedUpdate()
	{

		// Call add force function instead;
		// velocityAdditive += Mathf.Lerp(0, g, meterPerc);
		velocityAdditive = g * Time.fixedDeltaTime;

		transform.Translate( ((Vector3.down * (velocity + velocityAdditive)) * Time.fixedDeltaTime) );

		meterDist += transform.position.magnitude - lastUpdatePos.magnitude;
		if (meterDist <= 1)
		{
			meterPerc = meterDist / 1;
		}
		else
		{
			meterPerc = 0.0f;
		}

		velocity = (transform.position.magnitude - lastUpdatePos.magnitude) / Time.fixedDeltaTime;
		Debug.Log(transform.position.magnitude - lastUpdatePos.magnitude);
		lastUpdatePos = transform.position;
	}


	private void OnCollisionStay(Collision collision)
	{
		// This was just for testing. It stops them as they touch a collider.
		velocity = 0;
		meterPerc = 0;
	}

	public void AddForce()
	{
		
	}
}
