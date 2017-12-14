﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
  [SerializeField]
    PlayerHeath heath;
    [SerializeField]
    float climbMuply;
    [SerializeField]
    float maxClimbSpeed;
    [SerializeField]
    BoxCollider2D crouch;
    [SerializeField]
    BoxCollider2D Standing;
    [SerializeField]
    MeshRenderer standingPicture;
    [SerializeField]
    MeshRenderer crouchingPicture;
    bool crouching;
    [SerializeField]
    GrappleScript grappleScript;
    Rigidbody2D rb2d;
    Vector2 input;
    // this is the amount of control that a grapple hook has
    [SerializeField]
    float grappleControlMax = 10;
    [SerializeField]
    float grappleStartingControl = 0.2f;
    [SerializeField]
    float grappledControl = 0.5f;
    [SerializeField]
    bool ground = true;
    [SerializeField]
    bool jump = true;
    bool slowed;
    [SerializeField]
    int jumpCount = 0;
    [SerializeField]
    LayerMask groundlayer;
    [SerializeField]
    float jumpForce = 1f;
    [SerializeField]
     float maxSpeed = 7f;
    [SerializeField]
     float sprintMult = 3f;
    [SerializeField]
     float speedDamp = 0.01f;
    bool sprinting;
    [SerializeField]
    float backwardMod = 0.9f;
    float inAirSpeedMult = 0.5f;
    float gravity;
    [SerializeField]
    bool climbing;
    bool canWeClimb;
    [SerializeField]
    bool SlideMode;
   
    // Use this dfor initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gravity = rb2d.gravityScale;
        jump = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
   
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector2.down, 0.1f, groundlayer);
        if (hit.collider != null)
        {
            ground = true;
            
            jumpCount = 0;
        }
        else
        {
            ground = false;
        }
        if (jump&&Input.GetAxis("Jump")!=0&&(grappleScript.GetCurHook()!=null&& grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone()||jumpCount == 0 ||climbing))
        {
            jump = false;
          
            rb2d.AddForce(new Vector2(0, jumpForce));
            jumpCount++;
            if (grappleScript.GetCurHook() != null && grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone())
            {
                grappleScript.EndGrapple();
            }
            climbing = false;
            rb2d.gravityScale = gravity;
            StartCoroutine("JumpCoolDown");

        }
        // if we want sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
          //  sprinting = !sprinting;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouching = !crouching;
        }
        if (crouching)
        {
            crouchingPicture.enabled = true;
            crouch.enabled = true;
            Standing.enabled = false;
            standingPicture.enabled = false;
        }
        else
        {
            crouch.enabled = false;
            Standing.enabled = true;
            crouchingPicture.enabled = false;
            standingPicture.enabled = true;
          
        }
        if (Input.GetAxis("Climb")!=0&&canWeClimb)
        {
            if (climbing)
            rb2d.gravityScale = gravity;
            climbing = !climbing;
            if(grappleScript.GetCurHook()!=null)
            grappleScript.DestroyGrapple();
        }
        if (SlideMode)
        {
          
        }
       else if (canWeClimb&&climbing)
        {
            rb2d.gravityScale = 0;
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (Vector2.right * input.x).normalized, 0.5f, groundlayer);
            if (wallHit.collider == null)
            {
                rb2d.velocity = new Vector2(input.x * maxClimbSpeed * (sprinting ? climbMuply : 1), input.y * maxClimbSpeed * (sprinting ? climbMuply : 1));
            }
            else
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, input.y * maxClimbSpeed * (sprinting ? climbMuply : 1));
            }
      

        }
    else    if (ground && Mathf.Abs(rb2d.velocity.x) > maxSpeed * sprintMult * 1.5f)
        {
            if (slowed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * speedDamp/2, rb2d.velocity.y);
            }
            else
            rb2d.velocity = new Vector2(rb2d.velocity.x * speedDamp, rb2d.velocity.y);
            
        }
    
        else { 
                if (ground)
                {
                    //   GetComponent<Animator>().SetBool("back", false);
                    RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (Vector2.right * input.x).normalized, 0.5f, groundlayer);
                    Debug.DrawRay(transform.position, (Vector2.right * input.x).normalized, Color.white, Time.deltaTime);
                    if (wallHit.collider == null)
                    {
                    // This is when the player is going forwards
                    //   if (input.x < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x || input.x > 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)

                    //   GetComponent<Animator>().SetBool("back", false);
                    if (slowed)
                    {
                        rb2d.velocity = new Vector2(input.x *( maxSpeed/2) * (sprinting ? sprintMult : 1), rb2d.velocity.y);
                    }
                    else
                            rb2d.velocity = new Vector2(input.x * maxSpeed * (sprinting ? sprintMult : 1), rb2d.velocity.y);
                      
                        
                        // This is when the player is going backwards
                      //  else
                        {
                          

                       //     rb2d.velocity = new Vector2(input.x * maxSpeed * backwardMod, rb2d.velocity.y);
                        }
                    }
                 
                }
                // if the players swinging on the grapple hook
           else if (grappleScript.GetCurHook() != null && grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone() && !grappleScript.GetCurHook().GetComponent<GrappleHook>().reelingIn && Mathf.Abs(input.x) > float.Epsilon)
                {
             

                if (Mathf.Sign(input.x) == Mathf.Sign(rb2d.velocity.x))
                    {
                        if (rb2d.velocity.sqrMagnitude < grappleControlMax)
                        {
                            // if the rb2d.velocity is smaller than the small grapple control value then mutply the velocity
                            if (rb2d.velocity.sqrMagnitude < grappleStartingControl)
                            {
                                rb2d.velocity *= Mathf.Abs(input.x) * grappledControl + 1;
                       
                            }
                            // then just add it so that its not to much
                            else
                            {
                                rb2d.velocity += new Vector2(input.x * grappledControl, 0);
                          
                            }
                        }
                    }
                    // The player is moving opposite the swinging direction
                    else
                        rb2d.velocity /= 1.025f;
       
            }
            else if(input.x!=0)
            {
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (Vector2.right * input.x).normalized, 0.5f, groundlayer);
                Debug.DrawRay(transform.position, (Vector2.right * input.x).normalized, Color.white, Time.deltaTime);
                if (wallHit.collider == null && (Mathf.Sign(input.x) != Mathf.Sign(rb2d.velocity.x) || Mathf.Abs(rb2d.velocity.x) < maxSpeed))
                {
                    rb2d.velocity += Vector2.right * input.x * inAirSpeedMult;
                    Debug.Log("Swing");
                }
            }

        }
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");


    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "pullAbleObject")
        {
            if(grappleScript.GetCurHook()!=null&&grappleScript.GetCurHook().GetComponent <GrappleHook>().GetObjecGrappled()==other.gameObject)
            grappleScript.DestroyGrapple();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.tag == "ClimableObject")
        {
            canWeClimb = true;
            
       
        }
        if (other.tag == "puddle")
        {
            slowed = true;
        }

    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Slime")
        {
            SlideMode = true;
          //  rb2d.gravityScale += other.gameObject.GetComponent<Sliide>().slideAMount;
        }

        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "ClimableObject")
        {
            canWeClimb = false;
            climbing = false;
            rb2d.gravityScale = gravity;
        }
        if (other.gameObject.tag == "puddle")
        {
            slowed = false;
        }
    }
     void OnCollisionExit2D(Collision2D other)
    {
      
    }
    public Vector2 Getinput()
    {
        return input;
    }
    IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(0.2f);
        jump = true;
    
    }
    public void takeDamage(float _damage)
    {
        heath.takeDamage(_damage);
    }

}
