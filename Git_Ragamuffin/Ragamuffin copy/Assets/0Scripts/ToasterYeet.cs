using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterYeet : MonoBehaviour
{
    public Animator toasterAnim;
    public Rigidbody rb;
    public float force;

    void Start()
    {
        toasterAnim = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        toasterAnim.SetBool("Yeeting", true);

        rb = other.GetComponent<Rigidbody>();

        Invoke("Yeet", 0.5f);
        Invoke("Base", 1.0f);
    }
    void OnTriggerExit()
    {
        toasterAnim.SetBool("Yeeting", false);
        toasterAnim.SetBool("Reset", true);
    }
    public void Base()
    {
        toasterAnim.SetBool("Reset", false);
    }
    public void Yeet()
    {
        rb.AddForce(1, force, 0, ForceMode.Impulse);

        Debug.Log("yeet");
    }
 
}
