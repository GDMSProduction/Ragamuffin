using UnityEngine;
using UnityEngine.AI;

public class AlarmBotNav : MonoBehaviour
{

    // Remember to bake the agents

    [Header("Navigation")]
    [SerializeField]
    [Tooltip("Populate the array with nav points. (EX. empty game objects)")]
    Transform[] BotNavPoints;
    [Tooltip("What nav point the agent is on in the array.")]
    public int navpointIndex;
    [Tooltip("How close the nav point has to be before finding the next one.")]
    public float navDistance = 1;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public NavMeshAgent agent;

    public bool playerInSightRange = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInSightRange) { Patrolling(); };
    }

    private void Patrolling()
    {
        agent.SetDestination(BotNavPoints[navpointIndex].position); // setting destination to current nav point index
        if (Vector3.Distance(BotNavPoints[navpointIndex].position, gameObject.transform.position) < navDistance) // increasing the index when in navDistance
        {
            IncreaseIndex();
        }
    }

    void IncreaseIndex()
    {
        navpointIndex++; //increasing the index causes the agent to travel to the next point in the array
        if (navpointIndex >= BotNavPoints.Length)
        {
            navpointIndex = 0; // reseting nav point index so the agent will start again from the first point
        }
    }
}
