using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposiveLego : MonoBehaviour {
    [SerializeField]
    CircleCollider2D ExplosiveCollider;
    bool Blowup = false;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            ExplosiveCollider.enabled = true;
            Blowup = true;
        }
        
    }


}
