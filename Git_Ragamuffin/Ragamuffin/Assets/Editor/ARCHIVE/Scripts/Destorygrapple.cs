using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destorygrapple : MonoBehaviour {
    [SerializeField]
    GrappleScript grapple;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (grapple.GetCurHook() != null)
                grapple.DestroyGrapple();
        }
    }
}
