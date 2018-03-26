using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InVentroyObject {
    [SerializeField]
    GameObject door;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject GetDoor()
    {
        return door;
    }
  new  private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
