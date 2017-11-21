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
    bool jump = true;
    // Use this dfor initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ground || grappleScript.curHook == null)
            transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);
        else if(!ground)
        {



            // if the players swinging on the grapple hook
            if (grappleScript.curHook != null && grappleScript.curHook.GetComponent<GrappleHook>().done && !grappleScript.curHook.GetComponent<GrappleHook>().reelingIn && Mathf.Abs(input.x) > float.Epsilon)
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
