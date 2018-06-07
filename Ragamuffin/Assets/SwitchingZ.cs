using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingZ : MonoBehaviour {
    static bool forwardZObjectsActive=true;
    [SerializeField]
    List<GameObject> FowardObjects = new List<GameObject>();
    [SerializeField]
    List<GameObject> BehindObjects = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame  
	void Update () {
        if (forwardZObjectsActive == true)
        {
            for (int i = 0; i < FowardObjects.Count; ++i)
            {
                FowardObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
            for (int i = 0; i < BehindObjects.Count; ++i)
            {
                BehindObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < FowardObjects.Count; ++i)
            {
                FowardObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < BehindObjects.Count; ++i)
            {
                BehindObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
	}
}
