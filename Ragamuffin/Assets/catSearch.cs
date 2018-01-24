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
    Transform lookDirection1;
    [SerializeField]
    Transform lookDirection2;
    [SerializeField]
    Transform lookDirection3;
    [SerializeField]
    Transform lookDirection4;
    [SerializeField]
    Transform lookDirection5;
    [SerializeField]
    Transform lookDirection6;
    [SerializeField]
    Transform lookDirection7;
    [SerializeField]
    Transform lookDirection8;
    [SerializeField]
    Transform lookDirection9;
    [SerializeField]
    Transform lookDirection10;
    [SerializeField]
    Transform lookDirection11;
    [SerializeField]
    Transform lookDirection;
    [SerializeField]
    GameObject player;
    Vector3 spotlocation;
    Vector3 lastSpoted;
    int searchpath;
    bool searchforPlayer = true;
    [SerializeField]
    bool hide;
    [SerializeField]
    bool attac;
    bool inacourtine;
    bool bpause;
    [SerializeField]
    float angle;
    int oldway;
    bool turn = false;
    float dstancee;
    [SerializeField]
    bool dontlsten2wayponts;
    [SerializeField]
    bool correctway;
    [SerializeField]
    GameObject waypont1;
    [SerializeField]
    GameObject waypont2;
    bool stuned;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (direction == 0)
        {
            StartCoroutine(unpause());
        }
        if (stuned)
        {
            StartCoroutine(Stuned());
            rb2d.velocity = Vector2.zero;
            dontlsten2wayponts = false;
            return;
        }



            if (angle < 5.0f)
                print("close");
            RaycastHit2D hit = Physics2D.Linecast(startPoint.position, lookDirection.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit2 = Physics2D.Linecast(startPoint.position, lookDirection1.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit3 = Physics2D.Linecast(startPoint.position, lookDirection2.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit4 = Physics2D.Linecast(startPoint.position, lookDirection3.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit5 = Physics2D.Linecast(startPoint.position, lookDirection4.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit6 = Physics2D.Linecast(startPoint.position, lookDirection5.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit7 = Physics2D.Linecast(startPoint.position, lookDirection6.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit8 = Physics2D.Linecast(startPoint.position, lookDirection7.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit9 = Physics2D.Linecast(startPoint.position, lookDirection8.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit10 = Physics2D.Linecast(startPoint.position, lookDirection9.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit11 = Physics2D.Linecast(startPoint.position, lookDirection10.position, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit12 = Physics2D.Linecast(startPoint.position, lookDirection11.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(startPoint.position, lookDirection.position);
        Debug.DrawLine(startPoint.position, lookDirection1.position);
        Debug.DrawLine(startPoint.position, lookDirection2.position);
        Debug.DrawLine(startPoint.position, lookDirection3.position);
        Debug.DrawLine(startPoint.position, lookDirection4.position);
        Debug.DrawLine(startPoint.position, lookDirection5.position);
        Debug.DrawLine(startPoint.position, lookDirection6.position);
        Debug.DrawLine(startPoint.position, lookDirection7.position);
        Debug.DrawLine(startPoint.position, lookDirection8.position);
        Debug.DrawLine(startPoint.position, lookDirection9.position);
        Debug.DrawLine(startPoint.position, lookDirection10.position);
        Debug.DrawLine(startPoint.position, lookDirection11.position);















        if ((hit.collider != null && hit.collider.tag == "Player" && hide == false) || (hit2.collider != null && hit2.collider.tag == "Player" && hide == false) || (hit3.collider != null && hit3.collider.tag == "Player" && hide == false) || (hit4.collider != null && hit4.collider.tag == "Player" && hide == false) || (hit5.collider != null && hit5.collider.tag == "Player" && hide == false) || (hit6.collider != null && hit6.collider.tag == "Player" && hide == false) || (hit7.collider != null && hit7.collider.tag == "Player" && hide == false) || (hit8.collider != null && hit8.collider.tag == "Player" && hide == false) || (hit9.collider != null && hit9.collider.tag == "Player" && hide == false) || (hit10.collider != null && hit10.collider.tag == "Player" && hide == false) || (hit11.collider != null && hit11.collider.tag == "Player" && hide == false) || (hit12.collider != null && hit12.collider.tag == "Player" && hide == false))
        {
           

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
            dontlsten2wayponts = true;
            correctway = false;
            StopCoroutine(poschec());
        }
        else if(dontlsten2wayponts==true)
        {
           

            if (searchforPlayer == false)
            {
                lastSpoted = player.transform.position;
                dstancee = Vector3.Distance(lastSpoted, transform.position);
                // 
                searchforPlayer = true;
            }


            if (searchpath == 1)
            {



                if (correctway == false)
                {
                    StartCoroutine(poschec());
                }
                if (Vector3.Distance(lastSpoted, transform.position) < 1)
                {
                    searchpath = 2;
                    direction = 0;
                    StartCoroutine("pause");
                    dontlsten2wayponts = false;
                    correctway = true;
                }
              
               




            }
        }
                
                chasing = false;
            
        

        //   Vector2 location = new Vector2(startPoint.position.x, lookDirection.position.x);
        //    EnemyVision.SetPosition(0, startPoint.position);
        //    EnemyVision.SetPosition(1, lookDirection.position);

        //   Debug.DrawLine(startPoint.position, lookDirection.position);
        if(attac==false)
            rb2d.velocity = new Vector2(direction * maxSpeed * (chasing ? chaseMutiply : 1), rb2d.velocity.y);
        else
        {
            if (inacourtine == false)
            {
                StartCoroutine(catAnmaton());
                inacourtine = true;
            }
        }
    }
    IEnumerator poschec()
    {
        yield return new WaitForSeconds(0.2f);
        if (dstancee < Vector3.Distance(lastSpoted, transform.position)&&correctway==false)
        {
            direction = direction * -1;
           transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        correctway = true;
        oldway = direction;
      //  correctway = true;
        StopCoroutine(poschec());
    }
    IEnumerator catAnmaton()
    {

        
        rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(-direction * 1000000, rb2d.velocity.y));
        yield return new WaitForSeconds(.2f);
        rb2d.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);
            attac = false;
            inacourtine = false;
        
        
    }
    IEnumerator unpause()
    {
        yield return new WaitForSeconds(5);
        if (Vector3.Distance(transform.position, waypont1.transform.position) > Vector3.Distance(transform.position, waypont2.transform.position))
        {
            direction = -1;
          

        }
        else
        {
            direction = 1;
        }
        if (transform.localScale.x <=-1 && direction==1)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if(transform.localScale.x>=1&& direction == -1)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        dontlsten2wayponts = false;
        StopAllCoroutines();


    }
    IEnumerator Stuned()
    {
        yield return new WaitForSeconds(3);
        stuned = false;
        StopCoroutine(Stuned());
    }
    IEnumerator pause()
    {
        yield return new WaitForSeconds(2);
        searchpath = 2;
        direction = oldway * -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //   oldway = 0;
        dontlsten2wayponts = false;
        StopAllCoroutines();
    }
    IEnumerator turn12()
    {
        yield return new WaitForSeconds(0.3f);
        turn = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag== "WayPoint"&&chasing==false&&dontlsten2wayponts==false)
        {
            if (direction == -1)
            {
                searchpath = 2;
                   turn = false;
                direction = 1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                searchpath = 2;
                   turn = false;
                direction = -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        

    }
    public void SetAttac(bool _attac)
    {
        attac = _attac;
    }
    public void Sethide(bool _hide)
    {
        hide = _hide;
    }
    public void SetDirection(bool _stuned)
    {
        stuned = _stuned;
    }

}
