using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toad : MonoBehaviour {
    [SerializeField]
    float[] JumpPower = new float[10];
    [SerializeField]
    float[] Tungtimers = new float[10];
    [SerializeField]
    int Counter = 0;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    bool StartJump;
    [SerializeField]
    Toad toad;
    bool colide;
    
   void Start()
    {
        colide = false;
        Counter = 0;
    }
    // Use this for initialization
    void Update()
    {
        if (StartJump)
        {
            Jump();
        }

    }
    public void  Jump()
    {
        if (Counter < JumpPower.Length)
        {
            colide = true;
            StartJump = false;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(Vector2.up * JumpPower[Counter]);

            StartCoroutine(tungWhip());
        }
    }
    IEnumerator tungWhip()
    {
        yield return new  WaitForSeconds(Tungtimers[Counter]);
        if(GetComponent < FrogTung>()!=null)
        GetComponent<FrogTung>().throwtung = true;
           
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (colide)
        {
            if (other.gameObject.tag == "ground"&&Counter <JumpPower.Length)
            {
                toad.Jump();
                Counter++;
            }
        }
        if (other.gameObject.tag == "ground")
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}
