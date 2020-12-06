/*-------------------------------------------------------------------------------------------------------------------------------
 * Name: Steven Cole
 * Date: 1/24/2020
 * Purpose: Controlling guards
 * 
 *--------------------------------------------------------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    public GameObject[] agents;
    int selectedIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {   
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().SetHighlight(false);
            // increment the index
            selectedIndex++;
            // Always Bound check in case index goes higher then the objects
            // in the array and set to valid index
            if (selectedIndex >= agents.Length)
                selectedIndex = 0;

            // Output status
            Debug.Log("AgentManager selected Agent #" +
                selectedIndex + "" + agents[selectedIndex]);
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().SetHighlight(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().SetHighlight(false);
            // increment the index
            selectedIndex--;
            // Always Bound check in case index goes higher then the objects
            // in the array and set to valid index
            if (selectedIndex < 0)
                selectedIndex = 3;

            // Output status
            Debug.Log("AgentManager selected Agent #" +
                selectedIndex + "" + agents[selectedIndex]);
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().SetHighlight(true);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().Pause();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //this needs to be a swtich statement that makes it switch between forward and backwards
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().Reversing();

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            agents[selectedIndex].GetComponent<NavAgentStateMachine_Best>().ToAlarm();
        }
      
       
    }
}
