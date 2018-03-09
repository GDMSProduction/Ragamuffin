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
    [SerializeField]
    float hardcodedz;
    [SerializeField]
    soundAffect sounds;
    [SerializeField]
    PlayerMovement player;
	// Use this for initialization
	void Start () {
     

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = hardcodedz;
        cursorPos.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            if(sounds!=null)
            sounds.PlaySound("laso");
           
             Vector3 zeropo= cursorPos.transform.position;
            GrappleTarget.transform.position = zeropo;
            grappleScript.StartGrapple();
            player.AreWeUsingthePet = false;
       
          
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

            grappleScript.ZoomIn(0);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            grappleScript.EndZoom();
        }
       transform.position = new Vector3(playerTrans.position.x,playerTrans.position.y+4, transform.position.z);
	}
}
