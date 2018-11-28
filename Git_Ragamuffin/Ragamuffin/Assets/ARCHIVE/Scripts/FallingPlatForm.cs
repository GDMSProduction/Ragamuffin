using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatForm : MonoBehaviour {
    bool fall;
    [SerializeField]
    float falldownseconds;
    Vector3 startspot;
    Quaternion rotation;
    GrappleScript grappleScript;

	// Use this for initialization
	void Start () {
        startspot = transform.position;
        rotation = transform.rotation;
        GetComponent<Rigidbody2D>().gravityScale = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (fall == true)
        {
            GetComponent<Rigidbody2D>().gravityScale = 4;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.rotation = rotation;
        }
    }
    IEnumerator falldown()
    {
        yield return new WaitForSeconds(falldownseconds);
        fall = true;
        StartCoroutine(Reset());
    }
    IEnumerator Reset()
    {
      
        yield return new WaitForSeconds(1);
        fall = false;
        transform.position = startspot;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.rotation = rotation;
        if (grappleScript != null)
        {
            if (grappleScript.GetCurHook() != null&&grappleScript.GetCurHook().GetComponent<GrappleHook>().Poolme==gameObject)
            {
                grappleScript.DestroyGrapple();
            }
        }
        StopAllCoroutines();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            StartCoroutine(falldown());
        }
        else if( collision.gameObject.tag == "grapple")
        {
            GrappleHook grapple = null;
            StartCoroutine(falldown());
            grapple = collision.gameObject.GetComponent<GrappleHook>();
            grappleScript = grapple.player.GetComponent<GrappleScript>();

        }
    }
}
