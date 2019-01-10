using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField]
    float moveSpeed = 5.0f;
    [SerializeField]
    float jumpForce = 10.0f;
    float input;
    [SerializeField]
    bool jump;
    float jumpTimer;
    bool grounded;
    bool disableInput;
    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        jump = false;
        grounded = true;
        disableInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        input = Input.GetAxis("Horizontal");
        
        if (!jump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
            }
        }
        if (input < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (input > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.05f);

        //Debug.Log(hit.collider.name);

        if (hit && hit.collider != this)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    //using Fixed Update for the manipulation of a Rigidbody
    void FixedUpdate()
    {
        Vector2 moveVelocity = new Vector2(input * (moveSpeed * 10.0f) * Time.deltaTime, rb2D.velocity.y);
        rb2D.velocity = moveVelocity;
        if (jump)
        {
            rb2D.velocity += new Vector2(0, jumpForce);
            jump = false;

        }
    }
}
