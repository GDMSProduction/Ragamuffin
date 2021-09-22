using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairTrigger : MonoBehaviour
{
    public Animator chairAnim;

    // Start is called before the first frame update
    void Start()
    {
        //chairAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chairAnim.SetTrigger("RagNear");
        }
    }
}
