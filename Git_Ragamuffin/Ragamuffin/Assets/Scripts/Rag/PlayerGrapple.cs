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
    float maxGrappleDistance = 10f, climbingSpeed = 10f;
    [SerializeField]
    LayerMask grappleLayer;
    DistanceJoint2D distanceJoint;
    Vector2 targetPos;
    bool isGrappled;

    void Start ()
    {
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        isGrappled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Quick and dirty input for player (TEMPORARY)
        //PlayerInput();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireGrapple();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            DestroyGrapple();
        }

        if(isGrappled && GetComponent<DistanceJoint2D>().enabled)
        {
            //Climbing up
            if(Input.GetKey(KeyCode.W))
            {
                distanceJoint.distance -= climbingSpeed * Time.deltaTime;
            }
            //Climbing down
            if(Input.GetKey(KeyCode.S))
            {
                if(distanceJoint.distance < maxGrappleDistance)
                {
                    distanceJoint.distance += climbingSpeed * Time.deltaTime;
                }
            }
        }
    }

    //Firing the grappling hook
    void FireGrapple()
    {
        //Setting the target position of the mouse in worldspace
        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Drawing a raycast from the player position to the grapple point
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - (Vector2)transform.position, maxGrappleDistance, grappleLayer);

        //If they hit a grapple point
        if(hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null && hit.collider.tag == "Grapple")
        {
            distanceJoint.enabled = true;
            distanceJoint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>(); //The Distance Joint will attach to the Grapple Point
            distanceJoint.connectedAnchor = hit.point - (Vector2)hit.collider.transform.position;
            distanceJoint.distance = Vector2.Distance(transform.position, hit.point);
            isGrappled = true;
        }
    }

    void DestroyGrapple()
    {
        distanceJoint.enabled = false;
    }

    //Cheackin for grounded
    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.collider.tag == "Ground")
        {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collider)
    {
        if(collider.collider.tag == "Ground")
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
