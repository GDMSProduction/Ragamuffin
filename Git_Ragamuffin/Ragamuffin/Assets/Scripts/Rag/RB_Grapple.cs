using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//                   Author:	Robert Bauerle 
//                      Date:	9/7/2019 
//                 Purpose:	The script for rag's grapple 
// Associated Scripts:	Rag_Movement 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

[RequireComponent(typeof(Rag_Movement), typeof(LineRenderer))]
public class RB_Grapple : MonoBehaviour
{
	#region Variables
	#region Serialized
	[Tooltip("The range of Rag's grapple")]
	[SerializeField] private float maxGrappleDist;
	[Tooltip("How fast the grapple will launch towards the grapple point when used")]
	[SerializeField] private float grappleSpeed = 5;
	[Tooltip("How fast rag climbs up the grapple.")]
	[SerializeField] private float climbUpSpeed = 3;
	[Tooltip("How fast rag climbs down the grapple.")]
	[SerializeField] private float climbDownSpeed = 6;
	#endregion

	#region Private
	/// <summary> The grapple distance at which rag connected to the grapple point. (essentially the max range at which the grapple will become 'taut') </summary>
	private float curDistToPoint;

	/// <summary> Current grapple point that rag is connected to. </summary>
	private Transform curGrapplePoint;

	/// <summary> All of the grapple points within the scene </summary>
	private GameObject[] grapplePoints;

	private Rag_Movement ragMovement;
	private LineRenderer grappleLine;
	private bool isGrappled;
	private float minGrappleLength = 2;
	#endregion

	#endregion

	#region Properties
	/// <summary> Is Rag currently climb up or down the grapple </summary>
	public bool IsClimbing { get; private set; }


	#region Helper properties
	private Vector3 RagPositionNextFrame { get { return ragMovement.transform.position + ragMovement.Velocity; } }
	private Vector3 VecToGrapplePoint { get { return curGrapplePoint.position - RagPositionNextFrame; } } 
	#endregion

	#endregion

	#region Mono Methods
	private void Start()
	{
		ragMovement = GetComponent<Rag_Movement>();
		grapplePoints = GameObject.FindGameObjectsWithTag("grapple");
		grappleLine = GetComponent<LineRenderer>();
		if (grappleLine.positionCount < 2)
			grappleLine.positionCount = 2;
		grappleLine.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrappled && !ragMovement.OnGround)
		{
			BeginGrapple();
		}

		if (isGrappled && (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)))
		{
			EndGrapple();
		}
	}
	#endregion

	#region Grapple Functionality
	private void VelocitySet(ref Vector3 ragVelocity)
	{
		if (ragMovement.OnGround)
		{
			EndGrapple();
			return;
		}
		if (EnablePlayerControl(VecToGrapplePoint))
		{
			return;
		}
		ChangeVelocity(ref ragVelocity);
		ApplyClimbing();
	}

	private void ApplyClimbing()
	{
		IsClimbing = true;
		Vector3 translationVec = VecToGrapplePoint.normalized * Time.fixedDeltaTime;

		if (ragMovement.input.y > 0 && VecToGrapplePoint.magnitude > minGrappleLength)
		{
			translationVec *= climbUpSpeed;
		}
		else if (ragMovement.input.y < 0 && VecToGrapplePoint.magnitude < maxGrappleDist)
		{
			translationVec *= -climbDownSpeed;
		}
		else
		{
			IsClimbing = false;
			return;
		}

		ragMovement.transform.Translate(translationVec, Space.World);
		curDistToPoint = VecToGrapplePoint.magnitude;
	}

	private void ChangeVelocity(ref Vector3 ragVelocity)
	{
		float angle = Vector3.Angle(Vector3.up, VecToGrapplePoint);

		Vector3 worldZAxis = Vector3.forward;
		Vector3 perpendicular = Vector3.Cross(worldZAxis, VecToGrapplePoint.normalized);

		float dot = Vector3.Dot(ragVelocity.normalized, perpendicular);

		Vector3 eh = perpendicular;
		if (ragMovement.transform.position.x > curGrapplePoint.position.x)
		{
			eh = -perpendicular;
		}


		ragVelocity += (eh
				* (angle / 180)
				*.25f
				* ragMovement.Gravity
				* (ragMovement.transform.position.y > curGrapplePoint.position.y ? 5 : .7f))

			+ (ragMovement.input.x * (ragMovement.input.y == 0 ? 1 : 0.5f)
				* -perpendicular * Time.fixedDeltaTime
				* ((180 - angle) / 180));

		if (VecToGrapplePoint.magnitude > curDistToPoint)
		{
			ragVelocity = ((
				-VecToGrapplePoint.normalized
				* curDistToPoint) + curGrapplePoint.position)
				- ragMovement.transform.position;
		}

		ragVelocity.z = 0;
		ragVelocity = Vector3.ClampMagnitude(ragVelocity, ragMovement.moveSpeed * 3 * Time.fixedDeltaTime);
	}

	private bool EnablePlayerControl(Vector3 vecToGrapplePoint)
	{
		float distExtended = vecToGrapplePoint.magnitude - curDistToPoint;
		if (distExtended >= 0 && distExtended <= 0.05f * curDistToPoint)
		{
			ragMovement.useGravity = false;
			ragMovement.disableControls = true;
		}
		else if (distExtended < 0)
		{
			ragMovement.useGravity = true;
			ragMovement.disableControls = false;
			return true;
		}
		else if (distExtended > curDistToPoint)
		{
			Debug.LogError("Grapple has broken. Distance between grapple point and Rag is much larger than it should be.");
		}
		return false;
	}
	#endregion

	#region Helper Functions
	private void BeginGrapple()
	{
		FindGrapplePoint();

		if (curGrapplePoint != null)
			LaunchGrapple();
#if UNITY_EDITOR
		else
			Debug.Log("No Grapple Point within grappling distance.");
#endif
	}

	private void LaunchGrapple()
	{
		isGrappled = true;
		grappleLine.enabled = true;
		StartCoroutine(GrappleLine(curGrapplePoint.position, ragMovement.transform));
	}

	IEnumerator GrappleLine(Vector3 point, Transform player)
	{
		float t = 0;
		do
		{
			t += Time.fixedDeltaTime * grappleSpeed;
			if (t > 1)
				t = 1;
			grappleLine.SetPosition(0, player.position);
			grappleLine.SetPosition(1, Vector3.Lerp(player.position, point, t));
			yield return new WaitForFixedUpdate();
		} while (isGrappled && t < 1);
		Rag_Movement.preTranslateEvent += VelocitySet;
		curDistToPoint = (player.position - point).magnitude;
		do
		{
			grappleLine.SetPosition(0, player.position);
			yield return new WaitForFixedUpdate();

		} while (isGrappled);
	}

	public void EndGrapple()
	{
		if (!isGrappled)
			return;
		Rag_Movement.preTranslateEvent -= VelocitySet;
		curGrapplePoint = null;
		curDistToPoint = 0;
		grappleLine.enabled = false;
		isGrappled = false;
		ragMovement.useGravity = true;
		ragMovement.disableControls = false;

		//Small extra boost for the feels-good
		ragMovement.ChangeVelocity(ragMovement.Velocity * 0.1f);
	}

	private void FindGrapplePoint()
	{
		int index = 0;
		float mag = (transform.position - grapplePoints[0].transform.position).sqrMagnitude;
		for (int i = 1; i < grapplePoints.Length; i++)
		{
			float curMag = (grapplePoints[i].transform.position - transform.position).sqrMagnitude;
			if (curMag < mag)
			{
				index = i;
				mag = curMag;
			}
		}
		mag = (grapplePoints[index].transform.position - transform.position).magnitude;
		if (mag < maxGrappleDist)
		{
			curGrapplePoint = grapplePoints[index].transform;
			curDistToPoint = mag;
		}
	}
	#endregion
}
