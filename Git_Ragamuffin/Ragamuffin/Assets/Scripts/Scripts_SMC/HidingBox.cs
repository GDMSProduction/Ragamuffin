using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingBox : MonoBehaviour
{
    public Transform target;

    public GameObject child;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            transform.position = target.position;

            child.transform.SetParent(target.transform);

            target.GetComponent<SMC_move>().isHiding = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && target.GetComponent<SMC_move>().isHiding == true)
        {
            transform.position = transform.position;

            child.transform.parent = null;

            target.GetComponent<SMC_move>().isHiding = false;
        }

        
    }
}
