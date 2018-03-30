using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startJump : MonoBehaviour {
    [SerializeField]
    HoptoSpot hop;
    bool begen;
    [SerializeField]
    spider spder;
	// Use this for initialization
	void Start () {
        begen = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"&&begen==false)
        {
            begen = true;
            hop.Jump();
            spder.realdown = true;
            
        }
    }
}
