using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    [SerializeField]
     GameObject hookPrefab;
  
    [SerializeField]
     GameObject grappleTarget;

    [SerializeField]
    GameObject eyes;
    [SerializeField]
    soundAffect sound;


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
    // for realing out
    [SerializeField]
    private float noadMax;


    // this is made not null in the start grapple funtion
    
    private GameObject curHook = null;

    // Use this for initialization
    void Start()
    {
        curHook = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (curHook != null)
        {
          //  GetComponent<PlayerMovement>().ResetGravity();
        }
        // rells in the player
        if (reelingIn && curHook != null && (curHook.transform.position - eyes.transform.position).sqrMagnitude >= shortDist)
        {
            GrappleHook hook = curHook.GetComponent<GrappleHook>();
            if (hook.GetSecondNode() != null)
            {
                // this code here reels the player in  make the stupid high unles you wanan break the rope
                hook.GetSecondNode().GetComponent<HingeJoint2D>().connectedAnchor *= speed;
            }
            if (hook.GetNodesCount() > 3 && (hook.GetSecondNode().transform.position - eyes.transform.position).sqrMagnitude < 0.25f)
            {
                
                hook.DeleteSecond();
            }   
        }
        // if the player noads are not larger then the max for reeling out
        if (curHook!=null&&realout&&Vector2.Distance( curHook.transform.position,transform.position) <noadMax)
        {
            GrappleHook hook = curHook.GetComponent<GrappleHook>();
            if (hook != null)
            {
                if (realout == false)
                {
                    return;
                }
                hook.GetSecondNode().GetComponent<HingeJoint2D>().connectedAnchor *= speed;
                if (realout == false)
                {
                    return;
                }
                hook.MakeRope1();
            }   
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && curHook != null && (curHook.transform.position - eyes.transform.position).sqrMagnitude <= noadMax)
        {
            Debug.Log((curHook.transform.position - eyes.transform.position).sqrMagnitude);
            // Debug.Break();
            realout = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            realout = false;
        }
        if (curHook != null &&(curHook.transform.position - eyes.transform.position).sqrMagnitude >= noadMax)
        {
            realout = false;
        }
    }
    // SHOTS THE GRAPPLE HOOK
    public void StartGrapple()
    {
     
        if (curHook != null)
            DestroyGrapple();
        Vector3 destiny = grappleTarget.transform.position;
        curHook = (GameObject)Instantiate(hookPrefab,eyes.transform.position, Quaternion.identity);    
        curHook.transform.LookAt(grappleTarget.transform);
        Debug.Log(curHook);
        GrappleHook hookComp = null;
        hookComp = curHook.GetComponent<GrappleHook>();
        Debug.Log(hookComp); 
        hookComp.SetDestination(destiny);
        hookComp.SetTarget(grappleTarget);
        hookComp.SetMaxDistance(maxDistance);
        hookComp.player = gameObject;
        hookComp.SetEye(eyes);
        if(sound!=null)
        hookComp.sound = sound;
     
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
        reelingIn = true;

    }
    // when the player reaches the top of the grapple hook
    public void EndZoom()
    {
        reelingIn = false;
        if (curHook == null) return; 
        if (curHook.GetComponent<GrappleHook>().GetSecondNode() != null &&
            curHook.GetComponent<GrappleHook>().GetLastNode() != null)
            curHook.GetComponent<GrappleHook>().GetSecondNode().GetComponent<HingeJoint2D>().connectedBody =
                curHook.GetComponent<GrappleHook>().GetLastNode().GetComponent<Rigidbody2D>();
    }
    // assecors
    #region
    public GameObject GetCurHook()
    {
        return curHook;
    }
    #endregion
}
