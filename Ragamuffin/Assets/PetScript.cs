using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : InVentroyObject
{
    new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().areweholdingthepet = true;
            other.GetComponent<PlayerMovement>().petusues = 2;
            this.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
            other.GetComponent<PlayerMovement>().petatm = this.GetComponent<PetScript>();
        }
    }
    public void RelasePet()
    {
        transform.parent = null;
        Destroy(gameObject);
    }
}
