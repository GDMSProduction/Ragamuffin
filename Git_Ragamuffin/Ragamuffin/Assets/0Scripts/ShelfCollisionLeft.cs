using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfCollisionLeft : MonoBehaviour
{
    private Animator shelfAnim;

    void Start ()
    {
        shelfAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            Debug.Log("Howdy");
            shelfAnim.SetBool("isReadyLeft", true);
        }
    }
}
