using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSearch : MonoBehaviour
{
    [SerializeField]
    int direction = 0;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    bool chasing = true;
    [SerializeField]
    float chaseMutiply;
    [SerializeField]
    float QchaseMutply;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform startPoint1;
    [SerializeField]
    Transform startPoint2;
    [SerializeField]
    Transform startPoint3;
    [SerializeField]
    Transform startPoint4;
    [SerializeField]
    Transform startPoint5;
    [SerializeField]
    Transform startPoint6;
    [SerializeField]
    Transform startPoint7;
    [SerializeField]
    Transform startPoint8;
    [SerializeField]
    Transform startPoint9;
    [SerializeField]
    Transform startPoint10;
    [SerializeField]
    Transform startPoint11;
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
    [SerializeField]
    Vector3 lastSpoted;
    int searchpath;
    bool searchforPlayer = true;
    [SerializeField]
    bool hide;
    [SerializeField]
    bool attac;
    [SerializeField]
    bool inacourtine;
    bool bpause;
    [SerializeField]
    float angle;
    int oldway;
    bool turn = true;
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
    [SerializeField]
    float tme;
    [SerializeField]
    GameObject priority;
    int priortydirection = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 0)
        {
            StartCoroutine(unpause(3));
            attac = false;
        }
        if (inacourtine == true)
        {
            StartCoroutine(help());
        }
        if (stuned)
        {
            StartCoroutine(Stuned());
            rb2d.velocity = Vector2.zero;
            dontlsten2wayponts = false;
            return;
        }



        
        RaycastHit2D hit = Physics2D.Linecast(startPoint.position, lookDirection.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit2 = Physics2D.Linecast(startPoint1.position, lookDirection1.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit3 = Physics2D.Linecast(startPoint2.position, lookDirection2.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit4 = Physics2D.Linecast(startPoint3.position, lookDirection3.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit5 = Physics2D.Linecast(startPoint4.position, lookDirection4.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit6 = Physics2D.Linecast(startPoint5.position, lookDirection5.position, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit7 = Physics2D.Linecast(startPoint6.position, lookDirection6.position, 1 << LayerMask.NameToLayer("Player"));
        //RaycastHit2D hit8 = Physics2D.Linecast(startPoint7.position, lookDirection7.position, 1 << LayerMask.NameToLayer("Player"));
        //RaycastHit2D hit9 = Physics2D.Linecast(startPoint8.position, lookDirection8.position, 1 << LayerMask.NameToLayer("Player"));
        //RaycastHit2D hit10 = Physics2D.Linecast(startPoint9.position, lookDirection9.position, 1 << LayerMask.NameToLayer("Player"));
        //RaycastHit2D hit11 = Physics2D.Linecast(startPoint10.position, lookDirection10.position, 1 << LayerMask.NameToLayer("Player"));
        //RaycastHit2D hit12 = Physics2D.Linecast(startPoint11.position, lookDirection11.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(startPoint.position, lookDirection.position);
        Debug.DrawLine(startPoint1.position, lookDirection1.position);
        Debug.DrawLine(startPoint2.position, lookDirection2.position);
        Debug.DrawLine(startPoint3.position, lookDirection3.position);
        Debug.DrawLine(startPoint4.position, lookDirection4.position);
        Debug.DrawLine(startPoint5.position, lookDirection5.position);
        Debug.DrawLine(startPoint6.position, lookDirection6.position);
        //   Debug.DrawLine(startPoint7.position, lookDirection7.position);
        // Debug.DrawLine(startPoint.position, lookDirection8.position);
        // Debug.DrawLine(startPoint.position, lookDirection9.position);
        // Debug.DrawLine(startPoint.position, lookDirection10.position);
        // Debug.DrawLine(startPoint.position, lookDirection11.position);








        if (transform.localScale.x <= -1 && direction == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (transform.localScale.x >= 1 && direction == -1)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }






        if ((hit.collider != null && hit.collider.tag == "Player" && hide == false) || (hit2.collider != null && hit2.collider.tag == "Player" && hide == false) || (hit3.collider != null && hit3.collider.tag == "Player" && hide == false) || (hit4.collider != null && hit4.collider.tag == "Player" && hide == false) || (hit5.collider != null && hit5.collider.tag == "Player" && hide == false) || (hit6.collider != null && hit6.collider.tag == "Player" && hide == false) || (hit7.collider != null && hit7.collider.tag == "Player" && hide == false))
        {
            // Debug.Break();
            StopCoroutine(pause());
            StopCoroutine(poschec());
            StopCoroutine(unpause(3));
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

        }
        else if (dontlsten2wayponts == true)
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
                if (Vector3.Distance(lastSpoted, transform.position) < 4)
                {
                     //Debug.Break();
                    searchpath = 2;
                    direction = 0;
                    chasing = false;
                    StartCoroutine("pause");
                    dontlsten2wayponts = false;
                    correctway = true;
                 
                }






            }
            chasing = false;
        }





        //   Vector2 location = new Vector2(startPoint.position.x, lookDirection.position.x);
        //    EnemyVision.SetPosition(0, startPoint.position);
        //    EnemyVision.SetPosition(1, lookDirection.position);

        //   Debug.DrawLine(startPoint.position, lookDirection.position);
        if (attac == false)
        {
            if(priortydirection!= direction)
            rb2d.velocity = new Vector2(direction * maxSpeed * (chasing ? chaseMutiply : 1), rb2d.velocity.y);
            else
            {
                rb2d.velocity = new Vector2(direction * maxSpeed * (chasing ? chaseMutiply : 1.3f   ), rb2d.velocity.y);
            }
        }
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
        if (chasing == false)
        {
            if (dstancee < Vector3.Distance(lastSpoted, transform.position) && correctway == false && chasing == false)
            {
                direction = direction * -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
               
                correctway = true;
            }

            oldway = direction;
            //  correctway = true;
            StopCoroutine(poschec());
   yield return null;
        }
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
        StopCoroutine(catAnmaton());


    }
    IEnumerator unpause(float wait)
    {
        yield return new WaitForSeconds(wait);
        if (chasing == false)
        {
            
            if (priority.transform.position.x>transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            if (transform.localScale.x <= -1 && direction == 1)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                
            }
            else if (transform.localScale.x >= 1 && direction == -1)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            dontlsten2wayponts = false;
            StopAllCoroutines();
        }
     

    }
    IEnumerator Stuned()
    {
        yield return new WaitForSeconds(3);
        stuned = false;
        StopCoroutine(Stuned());
    }
    IEnumerator pause()
    {
        if (chasing == false)
        {
         
            yield return new WaitForSeconds(2);
            if (chasing == false)
            {

                searchpath = 2;
            //    direction = oldway * -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            
                //   oldway = 0;
                dontlsten2wayponts = false;
                StopAllCoroutines();
            }
        }
    }
    IEnumerator turn12()
    {
        yield return new WaitForSeconds(0.5f);
        turn = true;
    }
    IEnumerator turnaround()
    {
        yield return new WaitForSeconds(tme);
        dontlsten2wayponts = false;

    }
    IEnumerator help()
    {
        yield return new WaitForSeconds(4);
        if (inacourtine == true)
        {
            inacourtine = false;
            attac = false;
            
        }
        StopCoroutine(help());
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WayPoint" && chasing == false && dontlsten2wayponts == false && turn == true)
        {
            turn = false;
            StartCoroutine(turn12());
            rb2d.velocity = Vector2.zero;

            if (direction == -1)
            {
                searchpath = 2;

                direction = 1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            }
            else
            {

                searchpath = 2;

                direction = -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        if (other.tag == "clamp" && turn)
        {
            dontlsten2wayponts = true;
            StartCoroutine(turnaround());
            StartCoroutine(turn12());
            turn = false;
            rb2d.velocity = Vector2.zero;
            if (direction == -1)
            {
                searchpath = 2;

                direction = 1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            }
            else
            {

                searchpath = 2;

                direction = -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }



    }
    public void SetAttac(bool _attac)
    {
        attac = _attac;
    }
    public bool getattac()
    {
        return attac;
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