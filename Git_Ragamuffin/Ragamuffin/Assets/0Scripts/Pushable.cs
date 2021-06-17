using UnityEngine;

public class Pushable : MonoBehaviour
{
    public Rag rag;
    public float test = 1f;
    public bool onOff = false;
    public bool onOff2 = false;
    private void Update()
    {
        if (!onOff )
        {
            test = 1f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        onOff = true;
        test = 0f;
    } 


    private void OnTriggerExit(Collider other)
    {
        onOff = false;
        test = 1f;
    }
    private void OnCollisionExit(Collision collision)
    {
        onOff = false;
        if (!onOff && rag.isGrounded == false)
        {
            test = 1f;
        }
            
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, -test * Time.deltaTime, 0);
        //transform.position += new Vector3(test * Time.deltaTime, 0, 0);
        //transform.AddForce(Vector3.down * 5f * Time.deltaTime);
    }
}
