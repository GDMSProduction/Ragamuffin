using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    [SerializeField]
     GameObject hookPrefab;
    GameObject grappleStart;
    [SerializeField]
     GameObject grappleTarget;

    [SerializeField]
    GameObject eyes;



    private bool reelingIn;
    [SerializeField]
    private bool realout;
    // hard codes for the grapple hook
    [SerializeField]
    private float maxDistance = 2f;
    [SerializeField]
    private float speed = 0.8f;
    // how close you want them2 be while reeing in
    private float shortDist = 1.2f;
    [SerializeField]
    private float noadMax;


    [SerializeField]
    Material electricMat;
  
    // this is made not null in the start grapple funtion
    [HideInInspector]
    public GameObject curHook = null;

    // Use this for initialization
    void Start()
    {
        curHook = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // rells in the player
        if (reelingIn && curHook != null && (curHook.transform.position - eyes.transform.position).sqrMagnitude >= shortDist)
        {
            GrappleHook hook = curHook.GetComponent<GrappleHook>();

            if (hook.GetSecondNode() != null)
            {
                // this code here reels the player in dont make the stupid high unles you wanan break the rope
                hook.GetSecondNode().GetComponent<HingeJoint2D>().connectedAnchor *= speed;
            }
            if (hook.GetNodesCount() > 3 && (hook.GetSecondNode().transform.position - eyes.transform.position).sqrMagnitude < 0.25f)
            {
                hook.DeleteSecond();
            }

        }
        if (realout&&curHook.GetComponent<GrappleHook>().GetNodesCount()<noadMax)
        {
            
            GrappleHook hook = curHook.GetComponent<GrappleHook>();
            if (realout == false)
            {
                return;
            }
            hook.GetSecondNode().GetComponent<HingeJoint2D>().connectedAnchor *= speed;
            if (realout == false)
            {
                return;
            }
            //  hook.CreateNode1();
            hook.MakeRope1();
            Debug.Log("Dog");

        }
     
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)&& curHook != null && curHook.GetComponent<GrappleHook>().GetNodesCount() < noadMax )
        {
            realout = true;
        }
        if (Input.GetKeyUp(KeyCode.B) && curHook != null)
        {
            realout = false;
            
        }
    }
    // SHOTS THE GRAPPLE HOOK
    public void StartGrapple()
    {
        if (curHook != null)
            DestroyGrapple();
        Vector2 destiny = grappleTarget.transform.position;
        curHook = (GameObject)Instantiate(hookPrefab,eyes.transform.position, Quaternion.identity);
    
       //ISSUE: if player is moving the transform position it instantiates is old by the time the particle plays
        curHook.transform.LookAt(grappleTarget.transform);
        Debug.Log(curHook);
        GrappleHook hookComp = null;
        hookComp = curHook.GetComponent<GrappleHook>();
        Debug.Log(hookComp);
        // TODO: Set up the hook to use the proper tag when needed
      
        hookComp.SetDestination(destiny);
        hookComp.SetTarget(grappleTarget);
        hookComp.SetMaxDistance(maxDistance);
        hookComp.player = gameObject;
        hookComp.SetEye(eyes);

      
    }

    public void EndGrapple()
    {
        if (curHook != null)
            curHook.GetComponent<GrappleHook>().reelingIn = true;
    }

    public void DestroyGrapple()
    {
        //delete rope
        curHook.GetComponent<GrappleHook>().DeleteNodes();
        Destroy(curHook);
        curHook = null;

        reelingIn = false;
    }

    public void ZoomIn(float speed)
    {
        if (!curHook)
            return;
        //GrappleHook hookComp = curHook.GetComponent<GrappleHook>();
        //rb2d.velocity += (Vector2)(hookComp.lastNode.transform.position - transform.position).normalized * speed;
        //if (Vector2.Distance(transform.position, hookComp.lastNode.transform.position) < 3.2f)
        //hookComp.DeleteLastNode();
        // TODO: Start the player zooming towards the hook head, remove the rope, and display a line renderer straight from the player to the hook head
        //curHook.GetComponent<GrappleHook>().DeleteNodes();
        //this.speed = speed;
        reelingIn = true;
        //      if(curHook.GetComponent<GrappleHook>().secondNode != null)
        //          curHook.GetComponent<GrappleHook>().secondNode.GetComponent<HingeJoint2D>().connectedBody = null;
    }
    // when the player reaches the top of the grapple hook
    public void EndZoom()
    {
        reelingIn = false;
        if (curHook == null) return;
        // TODO: End the player zooming towards the hook head, and add in a new rope and end the old line renderer
        //curHook.GetComponent<GrappleHook>().MakeRope();
        if (curHook.GetComponent<GrappleHook>().GetSecondNode() != null &&
            curHook.GetComponent<GrappleHook>().GetLastNode() != null)
            curHook.GetComponent<GrappleHook>().GetSecondNode().GetComponent<HingeJoint2D>().connectedBody =
                curHook.GetComponent<GrappleHook>().GetLastNode().GetComponent<Rigidbody2D>();
    }
}