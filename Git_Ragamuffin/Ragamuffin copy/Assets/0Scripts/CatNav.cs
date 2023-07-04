using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CatNav : MonoBehaviour
{
    //Remember to bake before use!!

    [Header("Navigation")]
    [SerializeField] [Tooltip("Populate the array with nav points. (EX. empty game objects)")]
    Transform[] CatNavPoints;
    [Tooltip("What nav point the agent is on in the array.")]
    public int navpointIndex;
    [Tooltip("How close the nav point has to be before finding the next one.")]
    public float navDistance = 1;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public NavMeshAgent agent;

    [Header("Player/Masks/UI")]
    public Transform playerLocation;
    public GameObject player;
    public LayerMask whatIsGround, whatIsPlayer;
    [Tooltip("Contains the fill meter.")]
    public Image maskCatAgitation;
    [Tooltip("Cat level in UI.")]
    public Image catlv1, catlv2, catlv3, catlv4;


    [Header("Attack Stats")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange, sightRangeBase;
    public bool playerInSightRange, playerInAttackRange;
    [Tooltip("Max agitation")]
    public float catAgitationMin = 0;
    [Tooltip("Min agitation")]
    public float catAgitationMax = 100;
    [Tooltip("Current agitation")]
    public float catAgitationCurrent = 0;
    [Tooltip("The max amount agitation can increase within a given time frame")]
    public float increaseAgitationThresholdMax = 20;
    [Tooltip("What the threshold is currently holding.")]
    public float incThreshold;
    [Tooltip("The max amount agitation can decrease within a given time frame")]
    public float decreaseAgitationThresholdMax = 50;
    [Tooltip("What the threshold is currently holding.")]
    public float decThreshold;
    [Tooltip("Current Agitation Level")]
    public float agitationLevel = 1;
    [Tooltip("Timer before agitation will be increased again.")]
    public float incTimer;
    private float incHolder; // holds time
    private bool incTiming = false; //Truns timing on and off
    [Tooltip("Timer before agitation will be decrease again.")]
    public float decTimer;
    private float decHolder; // holds time 
    private bool decTiming = false; //Turns timing on and off



    [Header("Spook Stats")]
    [Tooltip("Bool that controls if the cat is scared.")]
    public bool spooked = false;
    [Tooltip("Time it takes for the cat to be no longer scared.")]
    public float timeBetweenSpooks = 5.0f;
    public float howlongSpooked; // how long the cat has been spooked

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerLocation = player.transform;
        changeAgitationLevel();
    }

    // Update is called once per frame
    void Update()
    {
        GetFill();

        if(incTiming == true)
        {
            incHolder += Time.deltaTime;
        }
        if(decTiming == true)
        {
            decHolder += Time.deltaTime;
        }

        if(incHolder >= incTimer)
        {
            incTiming = false;
            incHolder = 0;
            incThreshold = 0;
        }
        if (decHolder >= decTimer)
        {
            decTiming = false;
            decHolder = 0;
            decThreshold = 0;
        }


        if (!spooked)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        }
        if(howlongSpooked >= timeBetweenSpooks)
        {
            spooked = false;
            howlongSpooked = 0;
        }
        if(spooked) { howlongSpooked += Time.deltaTime; }
        if (!playerInSightRange && !playerInAttackRange) { Patrolling(); };
        if (playerInSightRange && !playerInAttackRange) { ChasePlayer(); };
        if (playerInSightRange && playerInAttackRange) { AttackPlayer(); };

        if(catAgitationCurrent < 0) { catAgitationCurrent = 0; } // prevent cat agitation from being negative in all given contexts 
    }
    private void Patrolling()
    {
        /*
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude <2f) walkPointSet = false;
        */

        agent.SetDestination(CatNavPoints[navpointIndex].position); // setting destination to current nav point index
        if (Vector3.Distance(CatNavPoints[navpointIndex].position, gameObject.transform.position) < navDistance && !playerInSightRange) // increasing the index when in navDistance
        {
            IncreaseIndex();
        }
    }
    private void SearchWalkPoint()
    {
        // Search walk point is no longer being referenced/used
        float randomZ = Random.Range(-walkPointRange,walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 1f, whatIsGround)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(playerLocation.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        //transform.LookAt(player);

        Vector3 lookPos = playerLocation.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        };
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void IncreaseIndex()
    {
        navpointIndex++; //increasing the index causes the agent to travel to the next point in the array
        if (navpointIndex >= CatNavPoints.Length)
        {
            navpointIndex = 0; // reseting nav point index so the agent will start again from the first point
        }
    }

    public void Spook() // function that spooks the cat. Will be forced to Patrol until timeBetweenSpooks has passed.
    {
        spooked = true;
        playerInAttackRange = false;
        playerInSightRange = false;
    }

    public void addAgitation(float amount) //increase agitation by float
    {
        if (incThreshold <= increaseAgitationThresholdMax)
        {
            if (catAgitationCurrent < catAgitationMax)
            {
                catAgitationCurrent = catAgitationCurrent += amount;
                if (catAgitationCurrent > catAgitationMax) { catAgitationCurrent = catAgitationMax; }
                incThreshold = incThreshold += amount;
                Debug.Log("Agitation = " + catAgitationCurrent);
                incTiming = true;
            }

            if (catAgitationCurrent <= 25)
            {
                agitationLevel = 1;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 50 & catAgitationCurrent > 25)
            {
                agitationLevel = 2;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 75 & catAgitationCurrent > 50)
            {
                agitationLevel = 3;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 100 & catAgitationCurrent > 75)
            {
                agitationLevel = 4;
                changeAgitationLevel();
            }
        }

    }
    public void reduceAgitation(float amount) // reduce agitation by float
    {
        if (decThreshold <= decreaseAgitationThresholdMax)
        {
            if (catAgitationCurrent > catAgitationMin)
            {
                catAgitationCurrent = catAgitationCurrent -= amount;
                if (catAgitationCurrent < catAgitationMin) { catAgitationCurrent = catAgitationMin; }
                decThreshold = decThreshold += amount;
                Debug.Log("Agitation = " + catAgitationCurrent);
                decTiming = true;
            }

            if (catAgitationCurrent <= 25)
            {
                agitationLevel = 1;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 50 & catAgitationCurrent > 25)
            {
                agitationLevel = 2;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 75 & catAgitationCurrent > 50)
            {
                agitationLevel = 3;
                changeAgitationLevel();
            }
            if (catAgitationCurrent <= 100 & catAgitationCurrent > 75)
            {
                agitationLevel = 4;
                changeAgitationLevel();
            }
        }
    }
    public void changeAgitationLevel() 
    {
        if(agitationLevel == 1)
        {
            catlv1.enabled = true;
            catlv2.enabled = false;
            catlv3.enabled = false;
            catlv4.enabled = false;

            sightRange = sightRangeBase;
           // Debug.Log("agitation lv 1");
        }
        if(agitationLevel == 2)
        {
            catlv1.enabled = false;
            catlv2.enabled = true;
            catlv3.enabled = false;
            catlv4.enabled = false;

            sightRange = sightRangeBase + 1;
           // Debug.Log("agitation lv 2");
        }
        if (agitationLevel == 3)
        {
            catlv1.enabled = false;
            catlv2.enabled = false;
            catlv3.enabled = true;
            catlv4.enabled = false;

            sightRange = sightRangeBase * 2;
           // Debug.Log("agitation lv 3");
        }
        if (agitationLevel == 4)
        {
            catlv1.enabled = false;
            catlv2.enabled = false;
            catlv3.enabled = false;
            catlv4.enabled = true;

            sightRange = sightRangeBase * 3;
            //Debug.Log("agitation lv 4");
        }
    }

    void GetFill()
    {
        float fillAmount = (float)catAgitationCurrent / (float)catAgitationMax;
        maskCatAgitation.fillAmount = fillAmount;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
