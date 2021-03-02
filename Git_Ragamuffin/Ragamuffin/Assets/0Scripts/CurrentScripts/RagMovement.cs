﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class RagMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float jumpForce = 150f;
    public float attackRange;

    [SerializeField]
    private GameObject changeLevel;
    public GameObject pinHandle;
    public GameObject Cat;
    public GameObject dummyPin;
    public GameObject dropPoint;
    public string levelToLoad;
    private string level;
    private Rigidbody rb;
    public Transform childRag;
    public Transform catLocation;
    public Vector3 startPosition;
    private Scene scene;
    public bool amIHanging = false;
    public bool lookingRight = true;
    public bool isEquip = false;
    public bool isHiding = false;
    bool ableJump = true;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        level = scene.name;
        PlayerPrefs.SetString("lastlevel",level);
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;
        pinHandle.SetActive(false);
        Cat = GameObject.FindWithTag("Cat");
        catLocation = Cat.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Jumping();
            if (Input.GetKeyDown(KeyCode.R) && isEquip)
            {
                pinHandle.SetActive(false);
                dummyPin.transform.position = dropPoint.transform.position;
                dummyPin.SetActive(true);
                isEquip = false;
            }
        if (Input.GetKeyDown(KeyCode.D))
        {
            childRag.localRotation = Quaternion.Euler(0, 50, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            childRag.localRotation = Quaternion.Euler(0, 180, 0);
        }
        //childRag.localRotation = Quaternion.Euler(0, 50, 0);
        //childRag.localRotation = Quaternion.Euler(0, 180, 0);

        if (isEquip == true && Vector3.Distance(transform.position, catLocation.position) <= attackRange)
        {
            Cat.SendMessage("Spook");
            pinHandle.SetActive(false);
            isEquip = false;
        }

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
                transform.Translate(Vector3.back * forwardSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
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

    private void OnTriggerEnter(Collider other)
    {
        //make trigger tag it water
        if (other.gameObject.tag == ("Water"))
        {
            forwardSpeed = 1f;
        }
        if (other.gameObject.tag == ("EndLevel"))
        {
            changeLevel.GetComponent<LevelLoader>().LoadLevel(levelToLoad);
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

         if (other.gameObject.tag == ("Scarf"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                GetComponent<Rigidbody>().useGravity = false;
                transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
            }
        }

        if (other.gameObject.tag == ("Pin"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dummyPin = other.gameObject;
                isEquip = true;
                pinHandle.SetActive(true);
                dummyPin.SetActive(false);
                Debug.Log("Pin equipped");
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
            forwardSpeed = 5f;
        }
        if (other.gameObject.tag == ("JumpPad"))
        {
            jumpForce = 150;
        }
        if (other.gameObject.tag == ("Scarf"))
        {
                GetComponent<Rigidbody>().useGravity = true;
        }
    }
    //death function reset transform to start point. 
    public void Death()
    {
        gameObject.transform.position = startPosition;
    }
}
