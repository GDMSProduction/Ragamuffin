//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Christian Cipriano
//               Date: 9/28/17
//             Credit: Sebastian Lague <3
//            Purpose: Player controller that detects collision using raycasts. This is to avoid using a rigidbody.
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CC_Controller2D : CC_RaycastController
{
    //Slope climbing
    public float maxSlopeAngle = 55f;

    public CollisionInfo collisions;
    Vector2 playerInput;
    // Use this for initialization

    public override void Start()
    {
        base.Start();
        collisions.faceDirection = 1;
    }

    public void Move(Vector2 moveAmount, bool isStanding)
    {
        Move(moveAmount, Vector2.zero, isStanding);
    } 

    public void Move(Vector2 moveAmount, Vector2 input, bool isStanding = false)
    {
        UpdateRayCastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        if(moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollision(ref moveAmount);
        
        if (moveAmount.y != 0)
        {
            VerticalCollision(ref moveAmount); //Any changes made to references affects this too
        }
        transform.Translate(moveAmount);

        //Allow jumping on platforms
        if(isStanding)
        {
            collisions.below = true;
        }
    }

    //Moving horizontally determining collision
    void HorizontalCollision(ref Vector2 moveAmount)
    {
        //To discern which direction is colliding
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        //Drawing the raycasts
        for (int i = 0; i < horizontalRayCount; i++)
        {
            //if colliding with bottom, start with botLeft. If colliding with top, start with topLeft
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //Drawing arrays
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            //Detecting horizontal collision
            if (hit)
            {
                //If inside of an object, go to next raycast
                if (hit.distance == 0)
                {
                    continue;
                }

                //Slope detection
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //If the slope your climbing is less than the maximum slope, then climb it
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    //If the player suddenly climbs a slope while descending, it tops descending and start climbing
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distancetoSlopeStart = 0;

                    //Make sure the collider is actually making contace with the slope
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distancetoSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distancetoSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distancetoSlopeStart * directionX;
                }

                //If there is horizontal obstacle in your way, handle like this
                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }


            }
        }
    }
    //Handles collision on the Y
    void VerticalCollision(ref Vector2 moveAmount) //Takes in a reference to moveAmount
    {
        //To discern which direction is colliding
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;


        //Drawing the raycasts
        for (int i = 0; i < verticalRayCount; i++)
        {
            //if colliding with bottom, start with botLeft. If colliding with top, start with topLeft
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            //Detecting collision
            if(hit)
            {
                if(hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if(collisions.fallingThrough)
                    {
                        continue;
                    }
                    if(playerInput.y == -1)
                    {
                        collisions.fallingThrough = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; //Prevents hitting things that are further away

                //If there is obstacles while climbing slopes
                if(collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        //If the slope changes while climbing
        if(collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(slopeAngle != collisions.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    

    //Function for climbing slopes
    void ClimbSlope(ref Vector2 moveAmount, float _slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(_slopeAngle * Mathf.Deg2Rad) * moveDistance;

        //Maintain moveAmount when climbing slopes
        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(_slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true; //Allow player to jump while on slopes
            collisions.climbingSlope = true;
            collisions.slopeAngle = _slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    //Function for descending slopes
    void DescendSlope (ref Vector2 moveAmount)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);

        //Exclusive Or
        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!collisions.slidingDown)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            //Cast a ray based on which direction you are moving
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            //You are about to see a BUNCH of if statements
            //They are all there to check if there is actually a slope to descend. No need to fear
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad * Mathf.Abs(moveAmount.x)))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle > maxSlopeAngle)
            {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDown = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThrough = false;
    }
    
    //Struct to store collision info
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDown;
        
        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmountOld, slopeNormal;
        public int faceDirection;

        public bool fallingThrough;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDown = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            slopeNormal = Vector2.zero;
        }
    }
}
