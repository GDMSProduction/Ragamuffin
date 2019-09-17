using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Robert Bauerle
//               Date: 9/7/2019
//            Purpose: The script for rag's grapple
// Associated Scripts: Rag_Movement
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

[RequireComponent(typeof(Rag_Movement), typeof(LineRenderer))]
public class RB_Grapple : MonoBehaviour
{

    #region Variables


    [Tooltip("The range of Rag's grapple")]
    [SerializeField] float maxGrappleDist;
    [Tooltip("How fast the grapple will launch towards the grapple point when used")]
    [SerializeField] float grappleSpeed = 5;
    [Tooltip("How fast rag climbs up the grapple.")]
    [SerializeField] float climbSpeed = 3;
    [Tooltip("How fast rag climbs down the grapple.")]
    [SerializeField] float downSpeed = 6;

    /// <summary>
    /// The grapple distance at which rag connected to the grapple point. (essentially the max range at which the grapple will become 'taut')
    /// </summary>
    float curGrappleDist;

    /// <summary>
    /// Current grapple point that rag is connected to./
    /// </summary>
    Transform curGrapplePoint;

    /// <summary>
    /// All of the grapple points within the scene
    /// </summary>
    GameObject[] grapplePoints;

    Rag_Movement controller;
    LineRenderer grappleLine;
    bool isGrappled;

    #endregion

    #region Mono Functions
    private void Start()
    {
        controller = GetComponent<Rag_Movement>();
        grapplePoints = GameObject.FindGameObjectsWithTag("grapple");
        grappleLine = GetComponent<LineRenderer>();
        if (grappleLine.positionCount < 2)
            grappleLine.positionCount = 2;
        grappleLine.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrappled)
        {
            BeginGrapple();
        }

        if (isGrappled && Input.GetKeyDown(KeyCode.Mouse1))
        {
            EndGrapple();
        }
    }
    #endregion

    #region Grapple Functionality
    #region Grapple Variables
    Vector3 nextPos, dir, iVel, perpendicular;
    float dif, dif1, dot, r;
    bool left;

    /// <summary>
    /// Is Rag currently climb up or down the grapple
    /// </summary>
    public bool IsClimbing
    {
        get;
        private set;
    }
    #endregion
    private void VelocitySet(ref Vector3 velocity)
    {
        nextPos = controller.transform.position + velocity;
        dir = curGrapplePoint.position - nextPos;
        dif = dir.magnitude - curGrappleDist;
        if (dif < 0)
            return;
        iVel = velocity;
        dif1 = curGrappleDist - (curGrapplePoint.position - controller.transform.position).magnitude;
        r = dif1 / (Mathf.Abs(dif) + dif1);
        velocity *= r;

        perpendicular = Quaternion.AngleAxis(90, Vector3.forward) * dir.normalized;
        dot = Vector3.Dot(iVel.normalized, perpendicular);
        left = dot > 0;
        

        if (iVel.y < 0 && -iVel.y > Mathf.Abs(iVel.x))
        {
            if (nextPos.x < curGrapplePoint.position.x && left && (controller.input.x >= 0))
            {
                left = false;
            }
            else if (nextPos.x > curGrapplePoint.position.x && !left && (controller.input.x <= 0))
            {
                left = true;
            }
        }
        
        velocity += (1 - r) * (left ? perpendicular : -perpendicular) * iVel.magnitude;


        Debug.DrawLine(controller.transform.position, controller.transform.position + velocity.normalized * 5, Color.red);

        if (controller.input.y > 0 && dir.magnitude > 2)
        {
            controller.transform.Translate(dir.normalized * Time.fixedDeltaTime * climbSpeed, Space.World);
            curGrappleDist -= Time.fixedDeltaTime * climbSpeed;
            IsClimbing = true;
        }
        else if (controller.input.y < 0 && dir.magnitude < (maxGrappleDist - (Time.fixedDeltaTime * downSpeed)))
        {
            controller.transform.Translate(dir.normalized * Time.fixedDeltaTime * -climbSpeed, Space.World);
            curGrappleDist += Time.fixedDeltaTime * downSpeed;
            IsClimbing = true;
        }
        else
            IsClimbing = false;
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
        StartCoroutine(GrappleLine(curGrapplePoint.position, controller.transform));
    }

    IEnumerator GrappleLine(Vector3 point, Transform player)
    {
        float t = 0;
        do
        {
            t += Time.fixedDeltaTime * grappleSpeed;
            if (t > 1)
                t = 1;
            yield return new WaitForFixedUpdate();
            grappleLine.SetPosition(0, player.position);
            grappleLine.SetPosition(1, Vector3.Lerp(player.position, point, t));
        } while (isGrappled && t < 1);
        controller.preTranslateEvent += VelocitySet;
        curGrappleDist = (player.position - point).magnitude;
        do
        {
            yield return new WaitForFixedUpdate();
            grappleLine.SetPosition(0, player.position);
        } while (isGrappled);
    }

    private void EndGrapple()
    {
        controller.preTranslateEvent -= VelocitySet;
        curGrapplePoint = null;
        curGrappleDist = 0;
        grappleLine.enabled = false;
        isGrappled = false;
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
            curGrappleDist = mag;
        }
            
    }
    #endregion
}
