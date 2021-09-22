using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeSpinner : MonoBehaviour
{
    public bool OnHolderLevel = false;
    private GameObject playerHolder = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && OnHolderLevel)
        {
            transform.rotation *= Quaternion.AngleAxis(180, transform.up);
            if(playerHolder != null)
            {
                playerHolder.SendMessage("InvertControls");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnHolderLevel = true;
            playerHolder = other.gameObject;
        }
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            OnHolderLevel = false;
            playerHolder = null;
        }
        other.transform.parent = null;
    }
}
