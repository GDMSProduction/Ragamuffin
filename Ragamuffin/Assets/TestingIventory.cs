using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingIventory : MonoBehaviour {
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    InVentroyObject item;
	// Use this for initialization
	void Start () {
      inventory.AddItem(item);
      inventory.AddItem(item);
      inventory.AddItem(item);
      inventory.AddItem(item);


    }

    // Update is called once per frame
    void Update () {
     //   inventory.AddItem(item);
    }
}
