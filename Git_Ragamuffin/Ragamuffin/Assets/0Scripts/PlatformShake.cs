using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShake : MonoBehaviour
{
    //change to local position if put on a child object 

    public float speed = 0.36f;
    Vector3 pointA;
    Vector3 pointB;
    public int timeLeft = 10;
    [SerializeField]
    private Vector3 posA;
    private Vector3 posB;
    private Vector3 nexPos;
    private float startSpeed;
    [SerializeField]
    private Transform transformB = null;
    private float fallSpeed = 6f;
    public bool canFall = false;
    public float speed0;
    public float speed1;
    public float speed2;
    public float speed3;
    void Start()
    {
        //Get current position then add 0 to its Z axis
        pointA = transform.eulerAngles + new Vector3(0f, 0f, 5f); //current rotation

        //Get current position then substract 90 to its Z axis
        pointB = transform.eulerAngles + new Vector3(0f, 0f, -5f); //where you want it to rotate
        posA = gameObject.transform.position; // local position 
        posB = transformB.position; //local position
        nexPos = posB;
        startSpeed = fallSpeed;
    }

    void Update()
    {
        if (timeLeft > 7)
        {
            speed = speed0;             //0.36f;
        }
        if (timeLeft == 7)
        {
            speed = speed1;                      //0.75f;
        }
        if (timeLeft == 4)
        {
            speed = speed2;                        //1.2f;
        }
        if (timeLeft == 2)
        {
            speed = speed3;                             //2f;
        }
        if(canFall)
        {
            PingToThePong();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!canFall)
        {
            canFall = true;
            StartCoroutine("LoseTime");
            RestartCountDown();
        }
    }
    void PingToThePong()
    {
        if (timeLeft == 0)
        {
            return;
        }
        if (timeLeft != 0)
        {   //PingPong between 0 and 1
            float time = Mathf.PingPong(Time.time * speed, 1);
            transform.eulerAngles = Vector3.Lerp(pointA, pointB, time);
        }
    }
    void RestartCountDown()
    {
        gameObject.transform.position = posA;
        StopCoroutine("LoseTime");
        timeLeft = 10;
        StartCoroutine("LoseTime");
    }
    IEnumerator LoseTime()
    {
        while (timeLeft >= 1)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
    void FixedUpdate()
    {
        if(timeLeft == 0)
        {
            Move();
        }
    }
    private void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nexPos, fallSpeed * Time.deltaTime);
    }
}
