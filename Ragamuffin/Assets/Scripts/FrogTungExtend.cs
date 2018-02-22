using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTungExtend : MonoBehaviour {
    
    [SerializeField]
    BoxCollider2D destoryhook;
    // the destination of the hookC:\Users\panda12\Desktop\fixedone\Codebase\WorkingTitle\Assets\Scripts\Managers\CoilManger.cs
   private  Vector2 destiny;
    // the speed
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    float slowspeed;
    [SerializeField]
    private float zoominSpeed = 0.1f;
    [SerializeField]
    private float distance = 0.5f;
    [SerializeField]
    private float maxDistance = 10f;
    [SerializeField]
     GameObject nodePrefab;
    float damage;
    
    public GameObject player;
    [SerializeField]
   private GameObject secondNode;
    [SerializeField]
    // last node is the cloest rope 2 the player
    private GameObject lastNode;

    private GameObject grappleTarget;
    private LineRenderer lr;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    AudioClip hitSound;



    // this changes based on the rope length 
    public int vertexCount = 1;
    [SerializeField]
    private List<GameObject> Nodes = new List<GameObject>();


    // is true and it never turns false
    private bool done = false;

    [HideInInspector]
    public bool reelingIn = false;
    bool collision;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    FixedJoint2D connectedJoint = null;
    [SerializeField]
    Rigidbody2D connectedRigidbody = null;

    [SerializeField]
    Collider mainCollider;
    [SerializeField]

    GameObject eye;
 
    [SerializeField]
   
    bool hit;
    bool delete = true;

    [SerializeField]
    float hardcodesz;


    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        secondNode = null;
        lastNode = transform.gameObject;

        Nodes.Add(transform.gameObject);

        transform.LookAt(grappleTarget.transform.position);
        transform.Rotate(Vector2.up * -90);
        done = false;
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 20);
        if (reelingIn)
        {

            done = true;
            
       

            /*          if (mainCollider.enabled)
                        {
                            mainCollider.enabled = false;
                            return;
                        }*/

            transform.position = Vector3.MoveTowards(transform.position, eye.transform.position, speed *1.3f);
            transform.position = new Vector3(transform.position.x, transform.position.y, hardcodesz);
            if (Vector2.Distance(eye.transform.position, transform.position) < 1)
            {

                player.GetComponent<FrogTung>().DestroyGrapple();

            }

            /*            if (secondNode != null)
                        {
                            secondNode.GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
                        }
                        else
                        {
                            GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
                        }

                        if ((secondNode.transform.position - lastNode.transform.position).sqrMagnitude < 0.025f)
                        {
                            DeleteSecond();
                        }*/
         


        for (int i =0; i < vertexCount; ++i)
            {
                if(Vector2.Distance(Nodes[i].transform.position,eye.transform.position) < 2)
                {
                  

                    DeleteSecond();
                    i--;


                }
            }
        }
        else if (!done)
        {
            transform.position = Vector2.MoveTowards(transform.position, destiny, speed);

            if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }

        if (!done && Vector2.Distance(eye.transform.position, transform.position) > maxDistance)
            reelingIn = true;
        if (collision == true )
        {
            reelingIn = true;
          
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, hardcodesz);
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        
        if(other.gameObject.tag== "spider")
        {
            
            other.gameObject.GetComponent<spider>().HurtPlayer(damage);
            collision = true;
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "grapple")
        {

          
            collision = true;
        }
    }
    // Update is called once per frame
    void Update () {
        RenderLine();
        transform.position = new Vector3(transform.position.x, transform.position.y, hardcodesz);
    }
    void RenderLine()
    {
        lr.positionCount = vertexCount;

        int i;
        for (i = 0; i < vertexCount - 1; i++)
        {
            if (Nodes[i] == null)
                Nodes[i] = null;
            else lr.SetPosition(i, Nodes[i].transform.position);
        }

        lr.SetPosition(i, eye.transform.position);
    }
    public void CreateNode()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, hardcodesz);

        // puting the rope in last node in front of the players
        Vector3 pos2Create = eye.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= ((eye.transform.position - lastNode.transform.position).magnitude < distance ? (eye.transform.position - lastNode.transform.position).magnitude : distance);
        pos2Create += lastNode.transform.position;
        Debug.Log(pos2Create);
        pos2Create.z = 20;
        GameObject go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity);
        // sets the node 2 the transform of the parent
        go.transform.SetParent(transform);
        // replaces the hingje joint of the last node to the newsest last node
        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        // the last node because the second node
        secondNode = lastNode;
        // the new node becomes the last node
        lastNode = go;
        // adde the newest last node 2 the list
        Nodes.Add(lastNode);
        // incracment the vertex counter
        vertexCount++;



    }
    public void DeleteNodes()
    {

        vertexCount = 2;
        foreach (GameObject obj in Nodes)
        {
            if (obj != transform.gameObject)
            {
                Destroy(obj);
            }
        }
        List<GameObject> tempNodes = new List<GameObject>();
        tempNodes.Add(transform.gameObject);
        lastNode = transform.gameObject;
        secondNode = null;
        Nodes = tempNodes;
        Destroy(connectedJoint);
        Destroy(connectedRigidbody);
        Destroy(this);
        Destroy(gameObject);

    }
    // accessors
    #region 
    public bool GetGrappleHookDone()
    {
        return done;
    }
    public float GetNodesCount()
    {
        return Nodes.Count;
    }
    public GameObject GetSecondNode()
    {
        return secondNode;
    }
    public GameObject GetLastNode()
    {
        return lastNode;
    }
 
    #endregion
    // Setters
    #region
    public void SetTarget(GameObject _target)
    {
        grappleTarget = _target;
    }
    public void SetEye(GameObject _eye)
    {
        eye = _eye;
    }
    public void SetDestination(Vector2 _destination)
    {
        destiny = _destination;
    }
    public void SetMaxDistance(float _maxDistance)
    {
        maxDistance = _maxDistance;
    }
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    public void SetPlayer(GameObject Toad)
    {
        player = Toad;
    }
    public void DeleteSecond()
    {

        int i = Nodes.IndexOf(secondNode);
        --i;
        Destroy(secondNode);
        Nodes.Remove(secondNode);
        --vertexCount;
        secondNode = Nodes[i];
        secondNode.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();

    }
    #endregion
}
