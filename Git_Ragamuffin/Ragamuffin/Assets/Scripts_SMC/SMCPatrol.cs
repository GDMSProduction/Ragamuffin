using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMCPatrol : MonoBehaviour
{

    public Transform[] navPoints;

    private int DesNav;

    private NavMeshAgent cat;

    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        cat = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        cat.autoBraking = false;

        GotoNextPoint();
        beenAttacked = false;
    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!cat.pathPending && cat.remainingDistance < 0.5f)
            GotoNextPoint();


        Aggro();
    }


   public void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (navPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        cat.destination = navPoints[DesNav].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        DesNav = (DesNav + 1) % navPoints.Length;
        
    }

    public bool isAggro;
    void Aggro()
    {
        if (isAggro)
        {
            cat.destination = Player.transform.position;
           
        }
        
    }

    public IEnumerator WaitToAttack()
    {

        Debug.Log("Is working");
        yield return 5;
        beenAttacked = false;
        StopCoroutine(WaitToAttack());
        

    }

    public void HasBeenHit()
    {

        StartCoroutine(WaitToAttack());
        
        isAggro = false;
    }


    public bool beenAttacked;
    private void OnTriggerEnter(Collider other)
    {

        if (!beenAttacked)
        {
            if (other.gameObject.tag == ("Player"))
            {
                isAggro = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!beenAttacked)
        {

        isAggro = false;
        GotoNextPoint();
        beenAttacked = false;
        }
    }
}

