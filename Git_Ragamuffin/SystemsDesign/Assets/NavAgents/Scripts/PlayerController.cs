/*-------------------------------------------------------------------------------------------------------------------------------
 * Name: Steven Cole
 * Date: 1/24/2020
 * Purpose: Controlling player movements and players collider
 * 
 *--------------------------------------------------------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update


    public Camera cam;
    public NavMeshAgent player;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

           if(Physics.Raycast(ray, out hit));
            {
                // Move the Agent
                player.SetDestination(hit.point);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NavAgentStateMachine_Best>())
        {
            other.GetComponent<NavAgentStateMachine_Best>().ChasePlayer(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NavAgentStateMachine_Best>())
        {
            other.GetComponent<NavAgentStateMachine_Best>().ChasePlayer(false);
        }
    }




}
