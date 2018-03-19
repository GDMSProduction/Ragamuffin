using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edges : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<PlayerMovement>().SetGravity(0);
            other.GetComponent<PlayerMovement>().onedge=true;

        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().ResetGravity();
            other.GetComponent<PlayerMovement>().onedge=false;

        }
    }
}
