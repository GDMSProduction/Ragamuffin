using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 0.4f;
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
    bool jump = false;
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
    // Use this dfor initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
   
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector2.down, 0.1f, groundlayer);
        if (hit.collider != null)
        {
            ground = true;
            jump = false;
            jumpCount = 0;
        }
        else
        {
            ground = false;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&(jumpCount==0||grappleScript.GetCurHook()!=null&& grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone()))
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            jumpCount++;
            if (grappleScript.GetCurHook() != null && grappleScript.GetCurHook().GetComponent<GrappleHook>().GetGrappleHookDone())
            {
                grappleScript.EndGrapple();
            }
        }
        // if we want sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = !sprinting;
        }
        if (ground && Mathf.Abs(rb2d.velocity.x) > maxSpeed * sprintMult * 1.5f)
        {
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
                        {
                            //   GetComponent<Animator>().SetBool("back", false);
                            rb2d.velocity = new Vector2(input.x * maxSpeed * (sprinting ? sprintMult : 1), rb2d.velocity.y);
                      
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
        if (other.gameObject.tag == "ground")
        {
            ground = true;
        }
    }
     void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            ground = false;
        }
    }
}
