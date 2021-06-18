using UnityEngine;

public class Pushable : MonoBehaviour
{
    public Rag rag;
    //public bool onOff = false;
    public Rigidbody pushPullObject;
    private void Start()
    {
        pushPullObject = GetComponent<Rigidbody>();
    }
    private void Update()
    {
    }
   
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "KinematicTag")
    //    {
    //        if (rag.child.transform.parent != null)
    //        {
    //            rag.child.transform.parent = null;
    //        }
    //        pushPullObject.isKinematic = false;
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == ("KinematicTag"))
    //    {
    //        pushPullObject.isKinematic = false;
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == ("Ground"))
    //    {
    //        pushPullObject.isKinematic = true;
    //    }
    //}
   
    private void FixedUpdate()
    {
        //transform.position += new Vector3(0, -test * Time.deltaTime, 0);
        //transform.position += new Vector3(test * Time.deltaTime, 0, 0);
        //transform.AddForce(Vector3.down * 5f * Time.deltaTime);
    }
}
