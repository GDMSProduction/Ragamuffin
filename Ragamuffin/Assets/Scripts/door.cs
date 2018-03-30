using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour {
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    MeshRenderer doorrender;
    bool openedthedoor;
    [SerializeField]
    int numberofkeys;
    [SerializeField]
    int counter = 0;
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
            List<InVentroyObject> keys = inventory.GetKeys();
            for (int i = 0; i < keys.Capacity; ++i)
            {
               
                if (keys[i].GetComponent<Key>().GetDoor() == this.gameObject)
                {
                    counter++;
                    inventory.RemoveItem(keys[i]);
                }
                if (counter >= numberofkeys)
                {
                    //inventory.RemoveItem(inventory.GetItem());
                    // inset whateverr code we want to open the door.
                    openedthedoor = true;
                }
            }   
        }
    }
}
