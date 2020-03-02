using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour {
    [SerializeField]
    Rigidbody2D rgb2;
    float gravity;
    private void Start()
    {
        gravity = 1;
        rgb2.gravityScale = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="grapple")
        rgb2.gravityScale = gravity;
    }
}
