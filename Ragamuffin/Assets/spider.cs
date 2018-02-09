using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spider : MonoBehaviour {
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    bool realup;
    [SerializeField]
    float speed = 0.2f;
    [SerializeField]
    float heght;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (realup == true)
        {
            transform.Translate(Vector2.up*speed);
        }
        if (transform.position.y > heght)
        {
            // load scene;
            realup = false;
        }

    }
}
