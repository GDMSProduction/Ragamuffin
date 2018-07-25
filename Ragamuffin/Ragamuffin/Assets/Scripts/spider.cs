using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spider : MonoBehaviour {
    [SerializeField]
    Rigidbody2D rb2d;
    [HideInInspector]
  public  bool realup;
    [SerializeField]
    float speed = 0.2f;
    [SerializeField]
    float heght;
   
    bool stop = true;
   [HideInInspector]
  public  bool realdown;
    [SerializeField]
    float groundheght;
    [SerializeField]
    Toad toad;
    bool jumpToad;
    [SerializeField]
    float howLongToWait;
    [SerializeField]
    PlayerHeath heath;
	// Use this for initialization
	void Start () {
        stop = true;	
	}
    bool boost;
	// Update is called once per frame
	void Update () {
        if (realup == true&&stop==false)
        {
            if (boost == false)
            {
                rb2d.AddForce(Vector2.up *31);
                boost = true;
            }
            rb2d.AddForce(Vector2.up * speed);
            if (jumpToad == false)
            {
                StartCoroutine(ToadStarttheAssult());
                jumpToad = true;
            }


        }
        if (realdown)
        {
            rb2d.AddForce(Vector2.down * speed);
        }
        if (transform.position.y > heght&&realup)
        {
            // load scene;
            stop = true;
            rb2d.velocity = Vector2.zero;
        }
        if(transform.position.y < groundheght&&stop==true)
        {
            stop = false;
            realdown = false;
            rb2d.velocity = Vector2.zero;
        }

    }
    // the toads begen 2 jump
    IEnumerator ToadStarttheAssult()
    {
        yield return new WaitForSeconds(howLongToWait);
        toad.Jump();
    }
    public void HurtPlayer(float damage)
    {
        heath.takeDamage(damage);
    }
}
