/*-------------------------------------------------------------------------------------------------------------------------------
 * Name: Steven Cole
 * Date: 1/24/2020
 * Purpose: Make a functioning Nav Agent state machine.
 * 
 *--------------------------------------------------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEditor;


public class NavAgentStateMachine_Best : FSGDN.StateMachine.MachineBehaviour
{
             
    [SerializeField] NavPoint[] myNavPoints;
    int navIndex = 0;
    [SerializeField] GameObject Highlight;
    [SerializeField] Transform playerLocation;
    [SerializeField] GameObject alarm;


    public override void AddStates()
    {
        AddState<PatrolState_Best>();
        AddState<IdleState_Best>();
        AddState<PauseState>();
        AddState<AngryState>();
        AddState<PursueState>();

        SetInitialState<PatrolState_Best>();
    }




    // this needs to be a dual function that makes
    // both forward and reverse
    bool isReverse = false;

    public void pickNextNavPoint()
    {
        if (!isReverse)
        {


            ++navIndex;
            if (navIndex >= myNavPoints.Length)
            {
                navIndex = 0;
            }
        }
        else
        {
            
            --navIndex;
            if (navIndex < 0)
            {

                navIndex = myNavPoints.Length -1;
            }
        }

    }


    public void FindDestination()
    { 
        GetComponent<NavMeshAgent>().SetDestination(myNavPoints[navIndex].transform.position);
    }

   

    public void ChasePlayer(bool isChasing)
    {

        if (isChasing)
        {
        ChangeState<PursueState>();
        }
        GetComponent<NavMeshAgent>().SetDestination(playerLocation.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<NavPoint>())
            ChangeState<IdleState_Best>();
    }

    // Helper function for setting the object color
    public void SetMainColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    public void Reversing()
    {
        isReverse = !isReverse;

        if (isReverse)
        {
            SetMainColor(Color.blue);
        }
        else
        {
            SetMainColor(Color.green);
        }
        pickNextNavPoint();
        FindDestination();
    }

    bool alarmed = false;

    public void ToAlarm()
    {
        // toggle paused value
        alarmed = !alarmed;

        if (alarmed)
        {
            lastState = currentState;

            ChangeState<AngryState>();
            GetComponent<NavMeshAgent>().SetDestination(alarm.transform.position);
        }
        else
        {
            ChangeState<PatrolState_Best>();
        }
    }

    bool paused = false;
    FSGDN.StateMachine.State lastState = null;
    Color lastColor = Color.green;
    public void Pause()
    {
        // toggle paused value
        paused = !paused;

        if (paused)
        {
            // store current state for use when unpausing
            lastState = currentState;

            // change state to Pause
            ChangeState<PauseState>();
            GetComponent<NavMeshAgent>().isStopped = true;
        }
        else
        {
            // restore stored state when pausing earlier
            ChangeState(lastState.GetType());
            GetComponent<NavMeshAgent>().isStopped = false;
            SetMainColor(lastColor);
        }
    }


    public void SetHighlight(bool highlightOn)
    {
        if (highlightOn)
        {
            Highlight.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            Highlight.GetComponent<Renderer>().material.color = Color.grey;
        }
       
    }


    public override void Update()
    {
        // since this overrides the state machine’s Update()
        // it is very important to call parent class’ Update()
        // because that is where the state machine does it’s work for us
        base.Update();
    }
}

// New base class for NavAgent states that gives us some utility
// functions to help clean things up even more
public class NavAgentState : FSGDN.StateMachine.State
{
    // Nice accessor for getting our state machine script reference
    protected NavAgentStateMachine_Best NavAgentStateMachine()
    {
        return ((NavAgentStateMachine_Best)machine);
    }
}

// NOTE: now inherits from NavAgentState
public class PatrolState_Best : NavAgentState
{
    public override void Enter()
    {
        base.Enter();
        
        NavAgentStateMachine().FindDestination();
    }
}

// NOTE: now inherits from NavAgentState
public class IdleState_Best : NavAgentState
{
    float timer = 0;

    public override void Enter()
    {
        base.Enter();
        timer = 0;
        NavAgentStateMachine().SetMainColor(new Color(0.0f, 0.5f, 0.0f));
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= 2.0f)
        {
            machine.ChangeState<PatrolState_Best>();
            NavAgentStateMachine().pickNextNavPoint();
            
        }
    }
}

public class PauseState : NavAgentState
{
    public override void Enter()
    {
        base.Enter();
        NavAgentStateMachine().SetMainColor(Color.grey);
    }
}


public class AngryState : NavAgentState
{
    public override void Enter()
    {
        base.Enter();
        NavAgentStateMachine().SetMainColor(Color.red);
    }

}

public class PursueState : NavAgentState
{
    
    public override void Enter()
    {
        base.Enter();
        NavAgentStateMachine().SetMainColor(Color.magenta);
    }

    public override void Execute()
    {
        NavAgentStateMachine().ChasePlayer(false);
    }
}
