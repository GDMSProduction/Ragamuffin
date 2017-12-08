using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSearch : MonoBehaviour {
    int direction = 0;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    float maxSpeed;
    bool chasing;
    [SerializeField]
    float chaseMutiply;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        rb2d.velocity = new Vector2(direction * maxSpeed * (chasing ? chaseMutiply : 1), rb2d.velocity.y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "WayPoint")
        {
            if(direction==-1)
            direction = 1;
            else
            direction = -1;
        }
    }
}
