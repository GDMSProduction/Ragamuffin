using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<death>().setRespawn(gameObject);
    }
}
