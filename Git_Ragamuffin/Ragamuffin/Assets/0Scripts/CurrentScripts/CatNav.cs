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

    [Header("Player & Masks")]
    public Transform playerLocation;
    public GameObject player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Image maskCatAgitation;
    
    [Header("Attack Stats")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    [Tooltip("Max agitation")]
    public float catAgitationMin = 0;
    [Tooltip("Min agitation")]
    public float catAgitationMax = 100;
    [Tooltip("Current agitation")]
    public float catAgitationCurrent = 0;
    [Tooltip("The max amount agitation can increase within a given time frame")]
    public float increaseAgitationThreshold = 25;  
    //working notes, add a current threshold
    [Tooltip("The max amount agitation can decrease within a given time frame")]
    public float decreaseAgitationThreshold = 45;
    [Tooltip("Timer before agitation can be changed again ")]
    public float raiseAgitationTimer = 5;
    [Tooltip("Current Agitation Level")]
    public float agitationLevel = 1;


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
    }

    // Update is called once per frame
    void Update()
    {
        GetFill();

        if(!spooked)
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
        if (catAgitationCurrent < catAgitationMax)
        {
            catAgitationCurrent = catAgitationCurrent += amount;
            if(catAgitationCurrent > catAgitationMax) { catAgitationCurrent = catAgitationMax; }
            Debug.Log("Agitation = " + catAgitationCurrent);
        }
    }
    public void reduceAgitation(float amount) // reduce agitation by float
    {
        if (catAgitationCurrent > catAgitationMin)
        {
            catAgitationCurrent = catAgitationCurrent -= amount;
            if(catAgitationCurrent < catAgitationMin) { catAgitationCurrent = catAgitationMin; }
            Debug.Log("Agitation = " + catAgitationCurrent);
        }
    }
    public void increaseAgitationLevel() 
    {

    }
    public void decreaseAgitationLevel() 
    {

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
