using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour {
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    MeshRenderer doorrender;
    bool openedthedoor;
    private void Update()
    {
        if (openedthedoor)
        {
            Color aplhareduce;
            aplhareduce = doorrender.material.color;
            aplhareduce.a -= (float)0.01;
            doorrender.material.color = aplhareduce;
            if (aplhareduce.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(inventory.GetItem().GetComponent<Key>()!=null&& inventory.GetItem().GetComponent<Key>().GetDoor() == this.gameObject)
            {
                // inset whateverr code we want to open the door.
                openedthedoor = true; 
            }
        }
    }
}
