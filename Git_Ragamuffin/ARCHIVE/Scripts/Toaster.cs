using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour {
    [SerializeField]
    PlayerMovement player;
    [SerializeField]
    float force = 8000;
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "grapple")
        {
            StartCoroutine(StartPoolBack());
            player.SetToastLaunch(true);
        }
    }
    IEnumerator StartPoolBack()
    {
        yield return new WaitForSeconds(5);
        Vector2 force2add;
       if (player.transform.position.x<transform.position.x)
         force2add = (Vector2.up * force) + Vector2.left;
        else
        {
            force2add = (Vector2.up * force) + Vector2.right;
        }
        player.ToasterJump(force2add);
        player.SetToastLaunch(false);
    }

}
