using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SMC_move : MonoBehaviour
{
    private Rigidbody rb;

    public float forwardSpeed = 1.5f;

    public float jumpForce = 150f;

    bool ableJump = true;

    public bool isHiding = false;

    public Vector3 startPosition;




    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        startPosition = transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        Jumping();





    }

    public bool disableMovement = false;

    private void FixedUpdate()
    {

        if (disableMovement)
        {

        }
        else
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

    }




    private void Jumping()
    {
        //if hiding is true do nothing
        //if hiding is false jump
        if (isHiding || disableMovement)
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

    public bool amIHanging = false;

    private void OnTriggerEnter(Collider other)
    {
        //make trigger tag it water
        if (other.gameObject.tag == ("Water"))
        {
            forwardSpeed = 2.5f;

        }


        //Make trigger tag it fire
        if (other.gameObject.tag == ("Fire"))
        {
            Death();

        }


        if (other.gameObject.tag == ("CheckPoint"))
        {
            startPosition = other.transform.position;
            Debug.Log("CheckPoint updated!");
        }

        if (other.gameObject.tag == ("JumpPad"))
        {
            jumpForce = 250;
        }


        //if (other.gameobject.tag == ("picture"))
        //{
        //    debug.log("hi do something");
        //    other.gameobject.getcomponent<picturecollection>().iscollected = true;
        //    other.gameobject.getcomponent<picturecollection>().images[0].color = color.red;

        //    if (other.gameobject.getcomponent<picturecollection>().iscollected == true)
        //    {
        //        destroy(other.gameobject);
        //    }
        //}
    }

    

    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.tag == ("Hanger"))
        {
            

            if (Input.GetKeyDown(KeyCode.E))
            {
                

                gameObject.transform.position = other.transform.position;

                gameObject.GetComponent<Rigidbody>().isKinematic = true;

                amIHanging = true;

                disableMovement = true;
            }

            if (amIHanging == true)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;

                    amIHanging = false;

                    disableMovement = false;
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.W) && amIHanging == true)
        {

            if (other.gameObject.tag == ("Climbable"))
            {
                gameObject.transform.position = other.transform.position;

                gameObject.GetComponent<Rigidbody>().isKinematic = false;

                amIHanging = false;

                disableMovement = false;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Water"))
        {
            forwardSpeed = 4.5f;
           
        }


        if (other.gameObject.tag == ("JumpPad"))
        {
            jumpForce = 150;
        }
    }

    //death function reset transform to start point. 
    public void Death()
    {
        gameObject.transform.position = startPosition;
    }
}
