using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    // Hint will show up when entering a is trigger Collider with this script 
    // on it and go away upon leaving that Collider.
    public GameObject hint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            hint.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            hint.SetActive(false);
        }
    }
}
