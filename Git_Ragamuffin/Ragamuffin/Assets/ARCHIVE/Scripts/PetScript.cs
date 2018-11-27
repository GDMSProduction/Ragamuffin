using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : InVentroyObject
{
    [SerializeField]
    PlayerMovement player;
   
    private void Update()
    {
        if (inventory.GetItem() == this||player.AreWeUsingthePet)
        {
           
            if (this.gameObject.GetComponent<MeshRenderer>()!=null)
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
          
        }
        else 
        {
            player.AreWeUsingthePet = false;
          
        }
    }
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
    public GameObject GetPet()
    {
        return gameObject;
    }
}
