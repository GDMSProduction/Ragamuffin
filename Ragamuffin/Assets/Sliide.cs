using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliide : MonoBehaviour {
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    float mass;
void Update()
    {
        if(mass>=60)
        rb2d.mass = mass;
        else
        {
            rb2d.mass = 60;
            Debug.Log("Do not Set the mass to Less than 60");
            Debug.Break();
            
        }
    }
   
    
}
