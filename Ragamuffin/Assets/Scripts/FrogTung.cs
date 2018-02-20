using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTung : MonoBehaviour {
    [SerializeField]
    GameObject hookPrefab;

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
    // for realing out
    [SerializeField]
    private float noadMax;
    [SerializeField]
    float damage;
  public  bool throwtung;

    // this is made not null in the start grapple funtion

    private GameObject curHook = null;


    // Use this for initialization
    void Start () {
        curHook = null;
    }

    void FixedUpdate()
    {
        if (throwtung == true)
        {
            StartGrapple();
            throwtung = false;
        }

    }
    // Update is called once per frame
    void Update () {
		
	}
    public void EndGrapple()
    {
        if (curHook != null)
            curHook.GetComponent<FrogTungExtend>().reelingIn = true;
    }
    public void StartGrapple()
    {

        if (curHook != null)
            DestroyGrapple();
        Vector3 destiny = grappleTarget.transform.position;
        curHook = (GameObject)Instantiate(hookPrefab, eyes.transform.position, Quaternion.identity);
        curHook.transform.LookAt(grappleTarget.transform);
        Debug.Log(curHook);
        FrogTungExtend hookComp = null;
        hookComp = curHook.GetComponent<FrogTungExtend>();
        Debug.Log(hookComp);
        hookComp.SetDestination(destiny);
        hookComp.SetTarget(grappleTarget);
        hookComp.SetMaxDistance(maxDistance);
        hookComp.player = gameObject;
        hookComp.SetEye(eyes);
        hookComp.SetDamage(damage);
        hookComp.SetPlayer(this.gameObject);



    }
    public void DestroyGrapple()
    {
        //delete rope
        curHook.GetComponent<FrogTungExtend>().DeleteNodes();

        Destroy(curHook);
        curHook = null;

        reelingIn = false;
    }
}
