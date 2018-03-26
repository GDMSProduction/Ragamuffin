using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVentroyObject : MonoBehaviour {

   
  public  Sprite sprite;
    [SerializeField]
    GameObject itemObject;

    public GameObject GetObject()
    {
        return itemObject;
    }
}
