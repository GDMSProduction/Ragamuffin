//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Christian Cipriano
//               Date: ???
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]

public class PlayerGrapple : MonoBehaviour
{
    Vector2 movemenet;
    bool grounded;

    //Grappling variables
    [SerializeField]
    float maxGrappleDistance = 5f, climbingSpeed = 10f;
    [SerializeField]
    LayerMask grappleLayer;
    DistanceJoint2D distanceJoint;
    Vector2 targetPos;
    bool isGrappled;
    RaycastHit2D hit;
    [SerializeField]
    GameObject[] grapplePoints;
    void Start()
    {
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        isGrappled = false;
        grapplePoints = GameObject.FindGameObjectsWithTag("grapple");
    }

    // Update is called once per frame
    void Update()
    {

        //Quick and dirty input for player (TEMPORARY)
        //PlayerInput();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireGrapple();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            DestroyGrapple();
        }

        if (isGrappled && GetComponent<DistanceJoint2D>().enabled)
        {
            //Climbing up
            if (Input.GetKey(KeyCode.W))
            {
                distanceJoint.distance -= climbingSpeed * Time.deltaTime;
            }
            //Climbing down
            if (Input.GetKey(KeyCode.S))
            {
                if (distanceJoint.distance < maxGrappleDistance)
                {
                    distanceJoint.distance += climbingSpeed * Time.deltaTime;
                }
            }
        }
    }

    //Firing the grappling hook
    void FireGrapple()
    {
        GameObject targetGrapplePoint = null;
        float previousDistance = 0;
        float tempDistance = 0;
        float curDistance = 0;

        for (int i = 0; i < grapplePoints.Length; i++)
        {

            //if there is no previous distance, this one is the shortest by default
            if (previousDistance == 0)
            {
                tempDistance = Vector2.Distance(transform.position, grapplePoints[i].transform.position);

                if (tempDistance <= maxGrappleDistance)
                {
                    curDistance = tempDistance;

                }
            }
            //store temporary distance to compare
            tempDistance = Vector2.Distance(transform.position, grapplePoints[i].transform.position);

            //compare if temp is less than previous, if so, it is shortest distance and is the preferable grapple point
            if (tempDistance < previousDistance && tempDistance <= maxGrappleDistance)
            {
                curDistance = tempDistance;
                //set target grapple point
                targetGrapplePoint = grapplePoints[i];
            }
            //}
            previousDistance = curDistance;
        }
        //Setting the target position of the mouse in worldspace
        if (targetGrapplePoint != null)
        {
            targetPos = targetGrapplePoint.transform.position;
        }
        //targetPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        //Drawing a raycast from the player position to the grapple point
        hit = Physics2D.Raycast(transform.position, targetPos - (Vector2)transform.position, maxGrappleDistance, grappleLayer);
        //If they hit a grapple point
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null && hit.collider.tag == "grapple")
        {

            distanceJoint.enabled = true;
            distanceJoint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>(); //The Distance Joint will attach to the Grapple Point
            distanceJoint.connectedAnchor = hit.point - (Vector2)hit.collider.transform.position;
            distanceJoint.distance = Vector2.Distance(transform.position, hit.point);
            isGrappled = true;
        }
        targetGrapplePoint = null;
    }

    void DestroyGrapple()
    {
        isGrappled = false;
        distanceJoint.enabled = false;
    }

    //Cheackin for grounded
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.collider.tag == "Ground")
        {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.collider.tag == "Ground")
        {
            grounded = false;
        }
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(transform.right * -1f * .5f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * .5f, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * 100f);
        }
    }
}
