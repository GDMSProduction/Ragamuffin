using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryNeck : MonoBehaviour {
    [SerializeField]
    int counter = 0;
   
    [HideInInspector]
public GameObject Hook;
   
	// Use this for initialization
	void Start () {
        counter = 0;
     
    }
	
	// Update is called once per frame
	void Update () {

        if (Hook != null)
        {
            if ((Vector2.Distance(gameObject.transform.position, Hook.transform.position) < 1.5f))
            {
                counter++;

            }
        }   
        if (counter >= 2)
        {
            Destroy(gameObject);
        }
	}
   

}
