using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMC_move : MonoBehaviour
{
    private Rigidbody rb;

    public float forwardSpeed = 1.5f;

    public float jumpForce = 150f;

    bool ableJump = true;

    public bool isHiding = false    ;
    



    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        

    }


    // Update is called once per frame
    void Update()
    {
        Jumping();

    }



    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * -forwardSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }



        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.forward * -forwardSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
    }




    private void Jumping()
    {
        //if hiding is true do nothing
        //if hiding is false jump
        if (isHiding)
        {


            
        }
        else
        {
            if (ableJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(0, jumpForce, 0);
                ableJump = false;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        //Tag anything Rag walks on ground.
        if (collision.gameObject.tag == ("Ground"))
        {
            ableJump = true;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        //make trigger tag it water
        if (other.gameObject.tag == ("Water"))
        {
            forwardSpeed = 0.75f;
            Debug.Log("Swimming");
        }


        //Make trigger tag it fire
        if (other.gameObject.tag == ("Fire"))
        {
            Destroy(gameObject);
            Debug.Log("Burning");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Water"))
        {
            forwardSpeed = 1.5f;
            Debug.Log("Dryland");
        }
    }

    //death function reset transform to start point. 
}
