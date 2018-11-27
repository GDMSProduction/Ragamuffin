using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSuperCharge : MonoBehaviour
{

    [SerializeField]
    catSearch cat;
    void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            cat.SetDirection(true);
            other.gameObject.GetComponent<PlayerMovement>().takeDamage(40);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
            if (other.gameObject.transform.position.x > transform.position.x)
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
            }
        }
    }
}