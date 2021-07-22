using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShake : MonoBehaviour
{
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
    private float fallSpeed = 2f;
    void Start()
    {
        //Get current position then add 0 to its Z axis
        pointA = transform.eulerAngles + new Vector3(0f, 0f, 0f); //current rotation

        //Get current position then substract 90 to its Z axis
        pointB = transform.eulerAngles + new Vector3(0f, 0f, 90f); //where you want it to rotate
        StartCoroutine("LoseTime");
        posA = gameObject.transform.localPosition;
        posB = transformB.localPosition;
        nexPos = posB;
        startSpeed = fallSpeed;
    }

    void Update()
    {
        PingToThePong();
        if (timeLeft > 7)
        {
            speed = 0.36f;
        }
        if (timeLeft == 7)
        {
            speed = 1f;
        }
        if (timeLeft == 4)
        {
            speed = 2f;
        }
        if (timeLeft == 2)
        {
            speed = 4f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        gameObject.transform.localPosition = posA;
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
        Move();
    }
    private void Move()
    {
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nexPos, fallSpeed * Time.deltaTime);
    }
}
