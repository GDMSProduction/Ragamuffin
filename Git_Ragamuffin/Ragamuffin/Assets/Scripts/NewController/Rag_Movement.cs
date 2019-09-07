using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Robert Bauerle
//               Date: 7/16/2019
//            Purpose: A movement script that functions as a player controller
// Associated Scripts: None
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

[RequireComponent(typeof(BoxCollider))]
public class Rag_Movement : MonoBehaviour
{
    #region Serialized Variables
    [Header("Movement")]
    [Tooltip("Speed at which Rag moves.")]
    [Range(0.1f, 100)]
    [SerializeField] float moveSpeed;
    [Tooltip("How long it takes rag to reach his movement speed.")]
    [Range(0.05f, 10)]
    [SerializeField] float moveAcceleration = 0.1f;
    [Tooltip("The maximum slope angle that rag can climb up")]
    [Range(1, 89)]
    [SerializeField] float maxClimbAngle;
    [Header("==============================================================================================================================================")]
    [Header("Jump")]
    [Tooltip("How high Rag can jump.")]
    [Range(0.1f, 100)]
    [SerializeField] float maxJumpHeight;
    [Tooltip("How fast Rag jumps.")]
    [Range(0.1f, 100)]
    [SerializeField] float jumpStrength;
    [Range(0.1f, 5)]
    [Tooltip("A multiplier for all movement-related numbers, gravity included.")]
    [SerializeField] float gravityMult = 2;
    [Header("==============================================================================================================================================")]
    [SerializeField] LayerMask collisionMask;
    [Tooltip("Rag's Mesh")]
    [SerializeField] Transform meshTransform;

    #endregion

    #region Variables
    private Vector2 input;

    private const float offsetWidth = 0.1f;
    private const int precision = 10;
    private float boundsY;



    public delegate void MovementDel(ref Vector3 velocity);
    public MovementDel preTranslateEvent;

    #endregion

    #region Mono Methods
    private void Start()
    {
        offsets = new RaycastOffsets(transform.position, GetComponent<BoxCollider>().bounds, offsetWidth, precision);
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        bounds.Expand(offsetWidth * -2);
        boundsY = bounds.extents.y;
    }

    private void FixedUpdate()
    {
        DetectGround();
        InputDetection();
        Jump();
        HorizontalMove();
        Move();
    }


    #endregion

    #region Input Detection

    private void InputDetection()
    {
        //Get and save the input for later use from the axis. We are getting a raw X because of the smooth damping done to emulate acceleration.
        input.x = Input.GetAxisRaw("Horizontal");

        //Hard-coded rotation setting for rag's mesh depending on the direction rag is currently moving in.
        if (input.x > 0)
        {
            meshTransform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (input.x < 0)
        {
            meshTransform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    #region Jumping
    private float dist;
    public System.Action JumpCalledEvent;

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpCalledEvent != null)
                JumpCalledEvent();
        }
        if (Input.GetKey(KeyCode.Space) && dist < maxJumpHeight)
        {
            SetVelocityY(jumpStrength * Time.fixedDeltaTime * gravityMult);
            dist += pVel.y;
        }
        else
        {
            if (!onGround)
                ChangeVelocity(y: gravity * gravityMult * Time.fixedDeltaTime);
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        if (onGround)
            dist = 0;
        else
            dist = maxJumpHeight;
    }

    #endregion

    #region Horizontal Movement
    float hMoveVel;
    private void HorizontalMove()
    {
        float target = input.x * moveSpeed * Time.fixedDeltaTime;
        SetVelocityX(Mathf.SmoothDamp(pVel.x, target, ref hMoveVel, moveAcceleration));
    }

    #endregion

    #endregion

    #region Ground Detection
    bool onGround;
    void DetectGround()
    {
        foreach (int i in offsets.down)
        {
            if (Physics.Raycast(transform.position + offsets[i], Vector3.down, offsetWidth * 3, collisionMask))
            {
                onGround = true;
                return;
            }
        }
        onGround = false;
    }

    #endregion

    #region Collision

    #region Movement

    #region Movement Variables
    /// <summary>
    /// Rag's current velocity.
    /// </summary>
    private Vector3 pVel;
    public Vector3 Velocity { get { return pVel / Time.fixedDeltaTime; } }

    private const float gravity = -3;
    

    
    private RaycastOffsets offsets;


    #endregion

    #region Velocity Helper Functions

    public void ChangeVelocity(Vector3 change)
    {
        pVel += change;
    }

    #region Additional Helper Functions for ChangeVelocity()
    public void ChangeVelocity(float x = 0, float y = 0, float z  = 0)
    {
        ChangeVelocity(new Vector3(x, y, z));
    }
    /// <summary>
    /// Set the velocity equal to the inputted vector.
    /// </summary>
    /// <param name="value">the new value for velocity</param>
    public void SetVelocity(Vector3 value)
    {
        ChangeVelocity(value - pVel);
    }
    /// <summary>
    /// Set the X value of the velocity equal to the inputted float.
    /// </summary>
    /// <param name="value">the new X value in velocity</param>
    public void SetVelocityX(float value)
    {
        ChangeVelocity(x: value - pVel.x);
    }
    /// <summary>
    /// Set the Y value of the velocity equal to the inputted float.
    /// </summary>
    /// <param name="value">the new Y value in velocity</param>
    public void SetVelocityY(float value)
    {
        ChangeVelocity(y: value - pVel.y);
    }
    /// <summary>
    /// Set the Z value of the velocity equal to the inputted float.
    /// </summary>
    /// <param name="value">the new Z value in velocity</param>
    public void SetVelocityZ(float value)
    {
        ChangeVelocity(z: value - pVel.z);
    }
    #endregion

    #endregion

    private void Move()
    {
        //Raycasting for collision
        VerticalCollisions();
        HorizontalCollisionsX();
        if (!use2D)
            HorizontalCollisionsZ();
        else
            pVel.z = 0;

        //Prevent ridiculously small values from being used when the character is not being moved
        if (Mathf.Abs(pVel.x) < .015f)
            pVel.x = 0;
        if (Mathf.Abs(pVel.y) < .015f)
            pVel.y = 0;
        if (Mathf.Abs(pVel.z) < .015f)
            pVel.z = 0;

        if (preTranslateEvent != null)
            preTranslateEvent(ref pVel);

        //Move the player based on the final velocity from input and collision
        transform.Translate(pVel);
    }

    #endregion

    #region Raycasting Helper Functions

    RaycastHit hit;
    private void VerticalCollisions()
    {
        float rayLength = Mathf.Abs(pVel.y) + offsetWidth;
        if (pVel.y < 0)
        {
            foreach (int i in offsets.down)
            {
                if (GetHit(offsets[i], Vector3.down, rayLength, ref pVel, out hit))
                {
                    pVel.y = (hit.distance - offsetWidth) * -1;
                    rayLength = hit.distance;
                }
            }
        }
        else if (pVel.y > 0)
        {
            foreach (int i in offsets.top)
            {
                if (GetHit(offsets[i], Vector3.up, rayLength, ref pVel, out hit))
                {
                    pVel.y = hit.distance - offsetWidth;
                    rayLength = hit.distance;
                }
            }
        }
    }

    private void HorizontalCollisionsX()
    {
        float rayLength = Mathf.Abs(pVel.x) + offsetWidth;
        if (pVel.x < 0)
        {
            for (int i = 0; i < offsets.left.Length; i++)
            {
                if (GetHit(offsets[offsets.left[i]], Vector3.left, rayLength, ref pVel, out hit))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (i < precision && angle < maxClimbAngle)
                    {
                        Climb(ref pVel, angle);
                        rayLength = 0;
                    }
                    else
                    {
                        pVel.x = (hit.distance - offsetWidth) * -1;
                        rayLength = hit.distance;
                    }

                }
            }
        }
        else if (pVel.x > 0)
        {
            for (int i = 0; i < offsets.right.Length; i++)
            {
                if (GetHit(offsets[offsets.right[i]], Vector3.right, rayLength, ref pVel, out hit))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (i < precision && angle < maxClimbAngle)
                    {
                        Climb(ref pVel, angle);
                        rayLength = 0;
                    }
                    else
                    {
                        pVel.x = hit.distance - offsetWidth;
                        rayLength = hit.distance;
                    }

                }
            }
        }
    }

    [Tooltip("Whether or not the character will be moving along the Z-axis at all")]
    [SerializeField] bool use2D;
    private void HorizontalCollisionsZ()
    {
        float rayLength = Mathf.Abs(pVel.z) + offsetWidth;
        if (pVel.z < 0)
        {
            for (int i = 0; i < offsets.back.Length; i++)
            {
                if (GetHit(offsets[offsets.back[i]], Vector3.back, rayLength, ref pVel, out hit))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (i < precision && angle < maxClimbAngle)
                    {
                        Climb(ref pVel, angle);
                        rayLength = 0;
                    }
                    else
                    {
                        pVel.z = (hit.distance - offsetWidth) * -1;
                        rayLength = hit.distance;
                    }

                }
            }
        }
        else if (pVel.z > 0)
        {
            for (int i = 0; i < offsets.forward.Length; i++)
            {
                if (GetHit(offsets[offsets.forward[i]], Vector3.forward, rayLength, ref pVel, out hit))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (i < precision && angle < maxClimbAngle)
                    {
                        Climb(ref pVel, angle);
                        rayLength = 0;
                    }
                    else
                    {
                        pVel.z = hit.distance - offsetWidth;
                        rayLength = hit.distance;
                    }

                }
            }
        }
    }

    /// <summary>
    /// Helper function for the collision functions
    /// </summary>
    private bool GetHit(Vector3 offset, Vector3 dir, float length, ref Vector3 vel, out RaycastHit hit)
    {
        //Debug.DrawLine(transform.position + offset, transform.position + offset + (dir * (length)), Color.red);
        return Physics.Raycast(transform.position + offset, dir, out hit, length, collisionMask);
    }

    private void Climb(ref Vector3 vel, float angle)
    {
        float Y = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(vel.x);
        if (vel.y < Y)
        {
            vel.y = Y;
            onGround = true;
        }
        vel.x = Mathf.Cos(angle * Mathf.Deg2Rad) * vel.x;
    }
    #endregion

    #region structs

    /// <summary>
    /// A container for a bunch of vector3 offsets to save on processing later without having to recalculate them
    /// </summary>
    private struct RaycastOffsets
    {
        public int precision { get; private set; }

        Vector3[] offsets;
        public Vector3 this[int index]
        {
            get
            {
                if (index >= offsets.Length)
                {
                    Debug.LogWarning("Attempted to access offsets array with an index out of range. Given index: " + index);
                    return Vector3.zero;
                }
                    
                return offsets[index];
            }
        }
        public int[] top, down, left, right, forward, back;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds">The bounds of the collider that will be used to find each offset position</param>
        /// <param name="offsetWidth">A multiplier for each offset to have the points be slightly inside of the collider</param>
        public RaycastOffsets(Vector3 pos, Bounds bounds, float offsetWidth, int precision = 3, bool use2D = true)
        {
            
            precision = Mathf.Clamp(precision, 3, int.MaxValue);
            this.precision = precision;
            bounds.Expand(offsetWidth * -2);
            #region Array initialization
            offsets = new Vector3[(precision * precision * 6) - (precision * 12) + 8];
            int t = 0, d = 0, l = 0, r = 0, f = 0, b = 0;
            top = new int[precision * precision];
            down = new int[precision * precision];
            left = new int[precision * precision];
            right = new int[precision * precision];
            forward = new int[precision * precision];
            back = new int[precision * precision];
            #endregion
            int cur = 0;
            float spacingX = bounds.size.x / (precision - 1);
            float spacingY = bounds.size.y / (precision - 1);
            float spacingZ = bounds.size.z / (precision - 1);
            
            Vector3 min = (Vector3.back * bounds.extents.z) + (Vector3.down * bounds.extents.y) + (Vector3.left * bounds.extents.x) + (bounds.center - pos);
            for (int i = 0; i < precision; i++)
            {
                for (int ii = 0; ii < precision; ii++)
                {
                    for (int iii = 0; iii < precision; iii++)
                    {
                        if (i > 0 && i < precision - 1 && ii > 0 && ii < precision - 1 && iii > 0 && iii < precision - 1)
                            continue;
                        offsets[cur] = min + (Vector3.right * spacingX * ii) + (Vector3.forward * spacingZ * i) + (Vector3.up * spacingY * iii);
                        if (i == 0)
                        {
                            back[b] = cur;
                            b++;
                        }
                        else if (i == precision - 1)
                        {
                            forward[f] = cur;
                            f++;
                        }
                        if (ii == 0)
                        {
                            left[l] = cur;
                            l++;
                        }
                        else if (ii == precision - 1)
                        {
                            right[r] = cur;
                            r++;
                        }
                        if (iii == 0)
                        {
                            down[d] = cur;
                            d++;
                        }
                        else if (iii == precision - 1)
                        {
                            top[t] = cur;
                            t++;
                        }
                        cur++;
                    }
                }
            }

            //Ordering arrays so that the first indexes in the array are the points that are on the bottom row of the face
            OrderArray(ref left, min.y);
            OrderArray(ref right, min.y);
            OrderArray(ref forward, min.y);
            OrderArray(ref back, min.y);
        }

        private void OrderArray(ref int[] array, float y)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (offsets[array[i]].y == y)
                {
                    for (int ii = 0; ii < array.Length; ii++)
                    {
                        if (offsets[array[ii]].y == y)
                            continue;
                        int temp = array[i];
                        array[i] = array[ii];
                        array[ii] = temp;
                    }
                }
            }
        }
    }

    

    #endregion

    #endregion
}
