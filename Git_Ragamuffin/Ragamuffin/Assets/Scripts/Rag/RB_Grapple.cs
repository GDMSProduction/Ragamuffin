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

    

    [SerializeField] float maxGrappleDist;
    [SerializeField] float elasticity;
    [SerializeField] float grappleSpeed = 5;

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

    private void VelocitySet(ref Vector3 velocity)
    {
        Vector3 pos = controller.transform.position, nextPos = pos + velocity;
        Vector3 dir = curGrapplePoint.position - nextPos;
        float dif = dir.magnitude - curGrappleDist;
        if (dif < 0)
            return;
        float mult = dif * (1 / elasticity) * Time.fixedDeltaTime;
        velocity += dir * mult;
        
    }

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
        controller.JumpCalledEvent += EndGrapple;
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
        controller.JumpCalledEvent -= EndGrapple;
        controller.preTranslateEvent -= VelocitySet;
        curGrapplePoint = null;
        curGrappleDist = 0;
        grappleLine.enabled = false;
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
