using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterYeet : MonoBehaviour
{
    public Animator toasterAnim;


    // Start is called before the first frame update
    void Start()
    {
        //toasterAnim = gameObject.GetComponentInParent<Animator>();
        toasterAnim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        toasterAnim.SetBool("Yeeting", true);

        Invoke("Yeet", 0.5f);

        Debug.Log("yeet");
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

    }
 
}
