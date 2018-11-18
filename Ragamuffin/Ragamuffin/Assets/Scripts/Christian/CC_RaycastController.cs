//------------------------------------------------------------------------------------------------

// Author: Christian Cipriano
// Date: 10-2-2017
// Credit: Sebastian Lague <3

// Purpose: New base controller for raycast collision

//------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CC_RaycastController : MonoBehaviour
{
    //Variables for raycasting
    public RayCastOrigins raycastOrigins;
     
    public const float skinWidth = 0.015f;

	[SerializeField]
    const float distanceBetweenRays = 0.05f;

    [HideInInspector]
    public int horizontalRayCount, verticalRayCount;

    public LayerMask collisionMask;
    public BoxCollider2D collider;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void UpdateRayCastOrigins()
    {
        //Creating a skin for the colliders
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2f);

        //Casting raycasts from the corners of the collider
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        //Making all the raycasts equidistant from each other regardless of size
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2f);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight/distanceBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
