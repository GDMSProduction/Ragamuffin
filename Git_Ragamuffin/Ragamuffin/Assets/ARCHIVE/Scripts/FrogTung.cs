using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTung : MonoBehaviour {
    [SerializeField]
    bool LeftSide;

    [SerializeField]
    GameObject hookPrefab;
    [SerializeField]
    GameObject NeckCompoents;
    [SerializeField]
    GameObject grappleTarget;

   
    List<GameObject> NeckNodes = new List<GameObject>();
    [SerializeField]
    GameObject eyes;
  
    GameObject Parent4;
    Vector3 startingPos;
   
    Quaternion startingRotation;

    private bool reelingIn;
   
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
    float damage;
    
  public  bool throwtung;
    [SerializeField]
    GameObject Neck;
    Quaternion quaternionNeck;

    // this is made not null in the start grapple funtion

    private GameObject curHook = null;
    [SerializeField]
    float TimeWeCanRotate;
    bool youMayRotate;

    [SerializeField]
    GameObject HAPPYROTATE;
    bool lowerypls;
    GameObject lastNode;
    [HideInInspector]
    bool makeNeck;

    float gravity;

    // Use this for initialization
    void Start () {
        curHook = null;
        Parent4 = HAPPYROTATE.transform.parent.gameObject;
        startingPos = HAPPYROTATE.transform.position;
        startingRotation = HAPPYROTATE.transform.rotation;
        gravity = GetComponent<Rigidbody2D>().gravityScale;

    }

    void FixedUpdate()
    {


        if (curHook != null)
        {

            FrogTungExtend nodeAccues = curHook.GetComponent<FrogTungExtend>();
            List<GameObject> nodes = nodeAccues.GetNodes();
            lastNode = Neck;
            if (curHook.GetComponent<FrogTungExtend>().reelingIn == true)
            {
                GetComponent<Rigidbody2D>().gravityScale = gravity;
              
                makeNeck = false;
                for (int i = 0; i < NeckNodes.Count; ++i)
                {
                    if (NeckNodes[i] != null)
                    {
                        if (LeftSide == false)
                        {
                            if (NeckNodes[i].transform.position.x < Neck.transform.position.x + 1)
                            {
                                Destroy(NeckNodes[i]);
                            }
                        }
                        else
                        {
                            if (NeckNodes[i].transform.position.x > Neck.transform.position.x + 1)
                            {
                                Destroy(NeckNodes[i]);
                            }
                        }
                    }
                }

            }
            else
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
          
            if (makeNeck == true)
            {

                for (int i = 0; i < nodeAccues.GetNodesCount(); ++i)
                {
                    Vector3 pos2Create = Neck.transform.position - lastNode.transform.position;
                    pos2Create.Normalize();
                    pos2Create *= ((Neck.transform.position - lastNode.transform.position).magnitude < 0.5f ? (Neck.transform.position - lastNode.transform.position).magnitude : 0.5f);
                    pos2Create += lastNode.transform.position;
                    lastNode = Instantiate(NeckCompoents, pos2Create, Neck.transform.rotation);
                   
                    lastNode.GetComponent<DestoryNeck>().Hook = curHook;
                    NeckNodes.Add(lastNode);
                }
            }
            
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
            HAPPYROTATE.transform.LookAt(grappleTarget.transform);

            //  Neck.transform.rotation =  new Quaternion(0, 0, 0.5495332f, 0.8354719f);
            Neck.transform.rotation = quaternionNeck;

        }
        if (throwtung == true)
        {
            StartGrapple();
            throwtung = false;
          
        }

    }
    IEnumerator DelayNeckCreation()
    {
        yield return new WaitForSeconds(0.3f);
        makeNeck = false;
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
    public void SetNeckRotation(Quaternion _SetNeckRotation)
    {
        quaternionNeck = _SetNeckRotation;
    }
    public void StartGrapple()
    {
        makeNeck = true;

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
        for(int i=0; i < NeckNodes.Count; ++i)
        {
            Destroy(NeckNodes[i]);
        }
        NeckNodes.Clear();
        //delete rope
        HAPPYROTATE.transform.parent = null;
        HAPPYROTATE.transform.parent = Parent4.transform;
        HAPPYROTATE.transform.localPosition = new Vector3(0, 4, 0);
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
