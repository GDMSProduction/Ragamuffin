using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {
    [SerializeField]
    float HealAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player"){
            other.gameObject.GetComponent<PlayerMovement>().HealPlayer(HealAmount);
            Destroy(gameObject);
        }
    }
}
