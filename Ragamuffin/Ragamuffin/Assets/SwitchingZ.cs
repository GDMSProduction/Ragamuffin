using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingZ : MonoBehaviour {
    static bool forwardZObjectsActive=true;
    static bool BackwardzObjectsActivate = false;
    static bool MapChangeBottomObjects = false;
    static bool SideWaysLocation = false;
    [SerializeField]
    List<GameObject> FowardObjects = new List<GameObject>();
    [SerializeField]
    List<GameObject> BehindObjects = new List<GameObject>();
    [SerializeField]
    List<GameObject> MapChangeBottomFloorObjects = new List<GameObject>();
    [SerializeField]
    List<GameObject> SideWaysObjects = new List<GameObject>();
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
            for(int i =0;i < MapChangeBottomFloorObjects.Count; ++i)
            {
                MapChangeBottomFloorObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < SideWaysObjects.Count; ++i)
            {
                SideWaysObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if(BackwardzObjectsActivate==true)
        {
          
            for (int i = 0; i < FowardObjects.Count; ++i)
            {
                FowardObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < BehindObjects.Count; ++i)
            {
                BehindObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
            for (int i = 0; i < MapChangeBottomFloorObjects.Count; ++i)
            {
                MapChangeBottomFloorObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < SideWaysObjects.Count; ++i)
            {
                SideWaysObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (MapChangeBottomObjects == true)
        {
            for (int i = 0; i < MapChangeBottomFloorObjects.Count; ++i)
            {
                MapChangeBottomFloorObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
           
            for (int i = 0; i < FowardObjects.Count; ++i)
            {
                FowardObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < BehindObjects.Count; ++i)
            {
                BehindObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < SideWaysObjects.Count; ++i)
            {
                SideWaysObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }

        }
        else if (SideWaysLocation == true)
        {
            for(int i=0; i < SideWaysObjects.Count; ++i)
            {
                SideWaysObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
            for (int i = 0; i < MapChangeBottomFloorObjects.Count; ++i)
            {
                MapChangeBottomFloorObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }

            for (int i = 0; i < FowardObjects.Count; ++i)
            {
                FowardObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            for (int i = 0; i < BehindObjects.Count; ++i)
            {
                BehindObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
	}
    public static void SetBackwarodsActiveTrue()
    {
        forwardZObjectsActive = false;
        MapChangeBottomObjects = false;
        BackwardzObjectsActivate = true;
        SideWaysLocation = false;
    }
    public static void SetMapChangeTrue()
    {
        forwardZObjectsActive = false;
        BackwardzObjectsActivate = false;
        MapChangeBottomObjects = true;
        SideWaysLocation = false;

    }
    public static void SetFrontObjectivestoTrue()
    {
        forwardZObjectsActive = true;
        BackwardzObjectsActivate = false;
        MapChangeBottomObjects = false;
        SideWaysLocation = false;

    }
    public static void SwitchToSideWaysObjects()
    {

        forwardZObjectsActive = false;
        BackwardzObjectsActivate = false;
        MapChangeBottomObjects = false;
        SideWaysLocation = true;
    }
}

