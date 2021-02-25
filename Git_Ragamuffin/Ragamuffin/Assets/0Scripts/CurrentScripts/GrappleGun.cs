using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    public LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cam;
    public float maxDistance = 100f;
    private SpringJoint joints;
    public GameObject player;
    public Transform playerTransform;
    public GameObject rag;
    
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopGrapple();   
        }
    }

    private void FixedUpdate()
    {
        float distanceFromPoint = Vector3.Distance(playerTransform.position, grapplePoint);
        if (distanceFromPoint > maxDistance)
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        if (rag.GetComponent<RagMovement>().isEquip == true)
        {
        }
        else
        {
            //Using spherecollider we get nearest object to connect spring too.
            Collider[] objectsInRange = Physics.OverlapSphere(playerTransform.position, maxDistance, whatIsGrappleable, QueryTriggerInteraction.Ignore);
            Collider closestObject = null;
            foreach (Collider _object in objectsInRange)
            {
                if (closestObject == null)
                {
                    closestObject = _object;
                }
                else
                {
                    if (Vector3.Distance(_object.transform.position, playerTransform.position) <= Vector3.Distance(closestObject.transform.position, playerTransform.position))
                    {
                        Debug.Log("New Closest Object!");
                        closestObject = _object;
                    }
                }
            }

            if (objectsInRange.Length > 0)
            {
                lr.enabled = true;
                //grapplePoint = object 0 in the array;
                grapplePoint = closestObject.transform.position;
                joints = player.gameObject.AddComponent<SpringJoint>();
                joints.autoConfigureConnectedAnchor = false;
                joints.connectedAnchor = grapplePoint;
                //Change these values to fit the game.
                joints.spring = 7f;
                joints.damper = 4f;
                joints.massScale = 5f;
                lr.positionCount = 2;
            }
        }
    }

    void DrawRope()
    {
        if (!joints) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {   
        lr.positionCount = 0;
        Destroy(joints);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, maxDistance);
    }
}
