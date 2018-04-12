using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour {
    bool flingup;
    [SerializeField]
    PlayerMovement player;
    private void FixedUpdate()
    {
        if (flingup == true)
        {
            
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "grapple")
        {
            StartCoroutine(StartPoolBack());
        }
    }
    IEnumerator StartPoolBack()
    {
        yield return new WaitForSeconds(5);
       
        Vector2 force2add = (Vector2.up * 8000) + Vector2.left;
        player.ToasterJump(force2add);
    }
}
