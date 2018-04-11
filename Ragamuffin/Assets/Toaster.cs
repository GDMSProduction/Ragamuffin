using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour {
    bool flingup;
    private void FixedUpdate()
    {
        if (flingup == true)
        {
            transform.position = (Vector3.up*0.1f) + transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "grapple")
        {
            flingup = true;
        }
    }
}
