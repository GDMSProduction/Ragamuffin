using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPLayer : MonoBehaviour {
    [SerializeField]
    GrappleScript grappleScript;
    [SerializeField]
    float grapplingRange;
    [SerializeField]
    GameObject GrappleTarget;
    [SerializeField]
    GameObject cursorPos;
    Vector3 mouseInput;
    [SerializeField]
    GameObject bottomclamp;
    [SerializeField]
    GameObject topclamp;
    [SerializeField]
    Transform playerTrans;
   
	// Use this for initialization
	void Start () {
     

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 9.75f; 
        cursorPos.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
           
             Vector3 zeropo= cursorPos.transform.position;
            zeropo.z = 0;
            GrappleTarget.transform.position = zeropo;
            grappleScript.StartGrapple();
       
          
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            grappleScript.ZoomIn(0);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            grappleScript.EndZoom();
        }
     //   transform.position = new Vector3(playerTrans.position.x,transform.position.y, transform.position.z);
	}
}
