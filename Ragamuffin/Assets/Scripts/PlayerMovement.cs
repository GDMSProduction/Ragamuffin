using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject startray;
    [SerializeField]
    GameObject endray;
    [SerializeField]
    GameObject sldemodeendarray;
    [SerializeField]
    GameObject oldendpont;

    [SerializeField]
    PlayerHeath heath;
    [SerializeField]
    death dead;
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
   public bool areweholdingthepet;
 public   bool AreWeUsingthePet;
   public int petusues = 2;
    [SerializeField]
    GrappleScript grappleScript;
    Rigidbody2D rb2d;
    Vector2 input;
    public PetScript petatm;
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
    [SerializeField]
    int axisbloc = 0;
    bool block;
    [SerializeField]
    catSearch cat;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    soundAffect sounds;
    [SerializeField]
    bool spider;
    bool onereset;
    public bool onedge;
    // Use this dfor initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gravity = rb2d.gravityScale;
        jump = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)&&areweholdingthepet&&petusues!=0&&AreWeUsingthePet==false)
        {
            AreWeUsingthePet = true;
            petusues--;
            cat.Sethide(true);
            this.gameObject.layer = 11;
            onereset = true;
        }
        else if(onereset&&AreWeUsingthePet==false)
        {
            cat.Sethide(false);
            this.gameObject.layer = 9;
            onereset = false;
               if (petatm != null && petusues == 0)
            {
                petatm.RelasePet();
            }
        }
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spider == true)
        {
            transform.position = transform.parent.transform.position;
            input.x = 0;
            input.y = 0;
        }

        if (SlideMode)
        {
            if (axisbloc == -1)
            {
                if (input.x >= 0&& input.x!=0)
                {
                    input.x *= -0.5f;

                }
                else
                {
                    input.x = -1;
                }
            }
            else
            {
                if (input.x < 0)
                {
                    input.x = 0.5f;
                }
                else
                {
                    input.x = 1;
                }
            }
        }
        if (SlideMode == true)
        {

            endray = sldemodeendarray;
        }
        else
        {
            endray = oldendpont;
        }
        RaycastHit2D hit = Physics2D.Linecast(startray.transform.position, endray.transform.position, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(startray.transform.position, endray.transform.position);
        if (hit.collider != null)
        {
            ground = true;

            jumpCount = 0;
        }
        else
        {
            ground = false;
        }
        if (spider == false&&jump && Input.GetAxis("Jump") != 0 && (grappleScript.GetCurHook() != null && grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone() || jumpCount == 0 || climbing))
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
        // change these for crouchen u have to set the bool to the correct values pls that why your crouch not correct
        if (crouching)
        {
            crouchingPicture.enabled = false;
            crouch.enabled = true;
            Standing.enabled = false;
            standingPicture.enabled = false;
        }
        else
        {
            crouch.enabled = false;
            Standing.enabled = true;
            crouchingPicture.enabled = false;
            standingPicture.enabled = false;

        }
        if (Input.GetAxis("Climb") != 0 && canWeClimb)
        {
            
                rb2d.gravityScale = gravity;
            climbing = true;
            if (grappleScript.GetCurHook() != null)
                grappleScript.DestroyGrapple();
           
        }
        else if (onedge == true)
        {
            rb2d.velocity = new Vector2(input.x * maxClimbSpeed * (sprinting ? climbMuply : 1), input.y * maxClimbSpeed * (sprinting ? climbMuply : 1));
        }
        else if (canWeClimb && climbing)
        {
            rb2d.gravityScale = 0;
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (Vector2.right * input.x).normalized, 0.5f, groundlayer);
           
            {
                rb2d.velocity = new Vector2(input.x * maxClimbSpeed * (sprinting ? climbMuply : 1), input.y * maxClimbSpeed * (sprinting ? climbMuply : 1));
            }
            //else
            //{
            //    rb2d.velocity = new Vector2(rb2d.velocity.x, input.y * maxClimbSpeed * (sprinting ? climbMuply : 1));
            //}


        }
        else if (ground && Mathf.Abs(rb2d.velocity.x) > maxSpeed * sprintMult * 1.5f)
        {
            if (slowed||AreWeUsingthePet)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * speedDamp / 2, rb2d.velocity.y);
            }
            else
                rb2d.velocity = new Vector2(rb2d.velocity.x * speedDamp, rb2d.velocity.y);
            if (SlideMode == false && input.x != 0)
            {
                if(sounds!=null)
                sounds.PlaySound("steps");
            }
            else
            {
                if (sounds!=null)
                sounds.StopFootSteps();
            }

        }

        else
        {
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
                    if (slowed||AreWeUsingthePet)
                    {
                        rb2d.velocity = new Vector2(input.x * (maxSpeed / 2) * (sprinting ? sprintMult : 1), rb2d.velocity.y);
                    }
                    else
                        rb2d.velocity = new Vector2(input.x * maxSpeed * (sprinting ? sprintMult : 1), rb2d.velocity.y);
                    if (SlideMode == false && input.x != 0)
                    {
                        if(sounds!=null)
                        sounds.PlaySound("steps");
                    }
                    else
                    {
                        if(sounds!=null)
                        sounds.StopFootSteps();
                    }


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
            else if (input.x != 0)
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
        if (other.gameObject.tag == "hide")
        {
            cat.Sethide(true);
            this.gameObject.layer = 11;


        }
        if(other.gameObject.tag== "spider")
        {
            spider = true;
            transform.parent = other.gameObject.transform;
           
            rb2d.gravityScale = 0;
            other.gameObject.GetComponent<spider>().realup = true;
        }

    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Slime")
        {
            SlideMode = true;
            axisbloc = other.gameObject.GetComponent<slideOzz>().WhatAxistoStop;
            //  rb2d.gravityScale += other.gameObject.GetComponent<Sliide>().slideAMount;
        }
        if (other.gameObject.tag == "spider")
        {
    
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
        if (other.gameObject.tag == "Slim")
        {

        }
        if (other.gameObject.tag == "hide"&&AreWeUsingthePet==false)
        {
            cat.Sethide(false);
            this.gameObject.layer = 9;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Slime")
        {
            SlideMode = false;
            axisbloc = 0;
            //  rb2d.gravityScale += other.gameObject.GetComponent<Sliide>().slideAMount;
        }
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
    public float GetHeath()
    {
        return heath.GetHeath();
    }
    public void CatFalty()
    {
        rb2d.AddForce(Vector2.up * 1000);
        dead.delaydeath = true;
    }
    public void HealPlayer()
    {
        heath.HealPlayer(35);
    }
    public void SetGravity(float _gravity)
    {
        rb2d.gravityScale = _gravity;
    }
    public void ResetGravity()
    {
        rb2d.gravityScale = gravity;
    }
}
