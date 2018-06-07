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
    [SerializeField]
    GameObject Childmepls;
    [SerializeField]
    GameObject Childmepls2;
    [SerializeField]
    GameObject Childmepls3;
    
    GameObject OrginalParentone;

    GameObject OrginalParent2;
  
    GameObject OrginalParent3;
    GameObject Parent4;
    Vector3 startingPos;
    [SerializeField]
    Quaternion startingRotation;

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
    [SerializeField]
    float TimeWeCanRotate;
    bool youMayRotate;

    [SerializeField]
    GameObject HAPPYROTATE;
    bool lowerypls;

    // Use this for initialization
    void Start () {
        curHook = null;
        Parent4 = HAPPYROTATE.transform.parent.gameObject;
        startingPos = HAPPYROTATE.transform.position;
        startingRotation = HAPPYROTATE.transform.rotation;

    }

    void FixedUpdate()
    {


        if (curHook != null)
        {
            
            if (lowerypls == false)
            {
            

            }
            youMayRotate = true;
            StartCoroutine(RotatePls(TimeWeCanRotate));
            HAPPYROTATE.transform.parent = curHook.transform;
            HAPPYROTATE.transform.localPosition = new Vector3(0, 0, 0);



        }
        else
        {
            lowerypls = false;
        }
        if (youMayRotate == true)
        {
          //  HAPPYROTATE.transform.Rotate(new Vector3(0, 0, -5));
        }
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
    public void StartGrappleTest()
    {

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
        HAPPYROTATE.transform.parent = null;
        HAPPYROTATE.transform.parent = Parent4.transform;
        HAPPYROTATE.transform.position = startingPos;
        HAPPYROTATE.transform.rotation = startingRotation;
        youMayRotate = false;
        curHook.GetComponent<FrogTungExtend>().DeleteNodes();

        Destroy(curHook);
        curHook = null;

        reelingIn = false;
    }
    IEnumerator RotatePls(float _allowedRotation)
    {
        yield return new WaitForSeconds(_allowedRotation);
        youMayRotate = false;
    }
}
