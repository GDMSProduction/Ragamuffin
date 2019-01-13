using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField]
    float moveSpeed = 6.0f;
    [SerializeField]
    float jumpForce = 3.0f;
    float jumpTimer;
    float input;
    bool grounded;
    //allowJump is for resetting the ability to jump on button press
    //this keeps the player from holding down the button and making the player character contantly jump
    bool allowJump;
    //jumpRequest is for when the player presses the jump button
    bool jumpRequest;
    bool disableInput;

    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        grounded = true;
        disableInput = false;
        jumpRequest = false;
        allowJump = true;
        jumpTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //obtain input from player
        if (!disableInput)
        {
            input = Input.GetAxis("Horizontal");
        }

        //check for jump
        if (Input.GetAxis("Jump") > 0)
        {
            jumpRequest = true;
        }
        //this resets the ability to jump when the player releases the jump button
        else
        {
            allowJump = true;
        }
        
        
        //this rotates the player left/right based on left/right movement keys
        //left input is negative, or less than zero
        //right input is positive
        if (input < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (input > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    //using Fixed Update for the manipulation of a Rigidbody
    void FixedUpdate()
    {
        Movement();
        Jump();
    }

    //for movement
    //simple but effective, and speed can be motified through the inspector
    void Movement()
    {
        Vector2 moveVelocity = new Vector2(input * (moveSpeed * 10.0f) * Time.deltaTime, rb2D.velocity.y);
        rb2D.velocity = moveVelocity;
    }

    //jump action and parameter cleanup
    void Jump()
    {
        if (grounded && jumpRequest && allowJump)
        {
            rb2D.velocity += new Vector2(0, jumpForce);
            grounded = false;
            allowJump = false;
        }
        jumpRequest = false;
    }

    //checks for player collision with ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        //layer 9 is the Ground layer
        if (collision.gameObject.layer == 9)
        {
            grounded = true;
        }
    }
}
