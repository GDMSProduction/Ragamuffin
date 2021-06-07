using UnityEngine;

public class HidingBox : MonoBehaviour
{
    public Transform target1;// Where you want Rag to teleport inside object.
    public Transform target2;// Where you want Rag to teleport outside object.
    public GameObject child; // Hook up Rag to this variable.

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
                LockingPlayerMovement();
        }
    }
    private void LockingPlayerMovement() // Locks player movement.
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            child.transform.position = target1.position;
            child.transform.SetParent(target1.transform);
            child.GetComponent<Rag>().isHiding = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && child.GetComponent<Rag>().isHiding == true)
        {
            child.transform.position = target2.position;
            transform.position = transform.position;
            child.transform.parent = null;
            child.GetComponent<Rag>().isHiding = false;
        }
    }
}
