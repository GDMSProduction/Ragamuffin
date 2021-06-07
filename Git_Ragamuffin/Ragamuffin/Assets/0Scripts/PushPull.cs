using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{
    bool canPushPull = false;
    public float test = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        test = 0f;
    }
    private void OnTriggerExit(Collider other)
    {
        test = 1f;
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, -test * Time.deltaTime, 0);
        //transform.position += new Vector3(test * Time.deltaTime, 0, 0);
        //transform.AddForce(Vector3.down * 5f * Time.deltaTime);
    }
}
