using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVentroyObject : MonoBehaviour {

    [SerializeField]
   protected Inventory inventory;
  public  Sprite sprite;
    [SerializeField]
    GameObject itemObject;

    public GameObject GetObject()
    {
        return itemObject;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inventory.AddItem(this);
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inventory.AddItem(this);
        }
    }
}
