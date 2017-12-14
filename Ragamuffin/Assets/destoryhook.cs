using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryhook : MonoBehaviour {
    [SerializeField]
    GrappleScript grapple;


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<GrappleHook>() != null&& other.gameObject.GetComponent<GrappleHook>().Getslowrealin()==true)
        {
            grapple.DestroyGrapple();
        }
    }
}
