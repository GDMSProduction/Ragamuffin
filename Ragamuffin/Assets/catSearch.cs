using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSearch : MonoBehaviour {
    [SerializeField]
    int direction = 0;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    float maxSpeed;
    bool chasing;
    [SerializeField]
    float chaseMutiply;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform lookDirection;
    [SerializeField]
    GameObject player;
    Vector3 spotlocation;
    Vector3 lastSpoted;
    int searchpath;
    bool searchforPlayer;
    [SerializeField]
    bool hide;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
            RaycastHit2D hit = Physics2D.Linecast(startPoint.position, lookDirection.position, 1 << LayerMask.NameToLayer("Player"));
            if (hit.collider != null && hit.collider.tag == "Player"&& hide==false)
            {
                if (chasing == false)
                {
                    spotlocation = transform.position;
                }
                chasing = true;
                if (player.transform.position.x > transform.position.x)
                {
                    direction = 1;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    direction = -1;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                }
                searchpath = 1;
                searchforPlayer = false;
            }
            else
            {
                if (searchforPlayer == false)
                {
                    lastSpoted = transform.position;
                }
                else
                {
                    if (searchpath == 1)
                    {
                        if (spotlocation.x > transform.position.x)
                        {
                        Debug.Log("rght");
                    
                            direction = 1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                        else
                        {
                            direction = -1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                        Debug.Log("left");
                    
                    }
                        if (Vector3.Distance(spotlocation, transform.position) < 0.1f)
                        {
                            searchpath = 2;
                 
                        }
                    }
                    else if (searchpath == 2)
                    {
                        if (lastSpoted.x > transform.position.x)
                        {
                            direction = 1;
                        Debug.Log("rght");
                   
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                        else
                        {
                            direction = -1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                        Debug.Log("left");
                    
                    }
                        if (Vector3.Distance(lastSpoted, transform.position) < 2)
                        {
                            searchpath = 3;
                    
                        }
                    }
                    else if (searchpath == 3)
                    {
                        if (spotlocation.x > transform.position.x)
                        {
                            direction = 1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                        Debug.Log("Rght");
                     
                    }
                        else
                        {
                            direction = -1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                        Debug.Log("left");
                     
                    }
                        if (Vector3.Distance(spotlocation, transform.position) < 2)
                        {
                            searchpath = -1;
                        }
                    }



                }
                searchforPlayer = true;
                chasing = false;

            }
        
        //   Vector2 location = new Vector2(startPoint.position.x, lookDirection.position.x);
        //    EnemyVision.SetPosition(0, startPoint.position);
        //    EnemyVision.SetPosition(1, lookDirection.position);

        //   Debug.DrawLine(startPoint.position, lookDirection.position);

            rb2d.velocity = new Vector2(direction * maxSpeed * (chasing ? chaseMutiply : 1), rb2d.velocity.y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "WayPoint"&&chasing==false)
        {
            if (direction == -1)
            {
                direction = 1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                direction = -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
