using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator doorAnim;
    bool isOpening;

    void Start()
    {
        //doorAnim = gameObject.GetComponent<Animator>();
        isOpening = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("hsfhdkjfh");
            doorAnim.SetBool("isOpening", true);
        }
    }
}
