using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour {
    [SerializeField]
    float damage;
    void OnTriggerStay2D(Collider2D other)
    {


        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().takeDamage(damage);
        }
    }

}
