using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoptoSpot : MonoBehaviour {
    [SerializeField]
    float[] JumpPower = new float[10];
    [SerializeField]
    float[] jumpcooldown = new float[10];
    int Counter = 0;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    bool StartJump;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
    }
    public void Jump()
    {
        if (Counter < JumpPower.Length)
        {
            StartJump = false;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(Vector2.up * JumpPower[Counter]);
            rb2d.AddForce(Vector2.right * JumpPower[Counter]);

            StartCoroutine(JumpCOoldown());
          
        }
      
    }
    IEnumerator JumpCOoldown()
    {
        yield return new WaitForSeconds(jumpcooldown[Counter]);
        Counter++;
        Jump();
     
    }
}
