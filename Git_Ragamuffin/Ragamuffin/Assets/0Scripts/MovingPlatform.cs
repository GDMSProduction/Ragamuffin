using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private GameObject playerHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
        playerHolder = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
        playerHolder = null;
    }
}
