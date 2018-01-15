using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour {
    [SerializeField]
    GameObject respawn;
    [SerializeField]
    PlayerHeath heath;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < -50||heath.GetHeath() <=0)
        {
            transform.position = respawn.transform.position;
            heath.ResetHeath();
        }

    }
}
