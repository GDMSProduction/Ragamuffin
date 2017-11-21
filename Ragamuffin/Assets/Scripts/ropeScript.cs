using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(setNoAutoConfigure());
    }

    IEnumerator setNoAutoConfigure()
    {
        yield return new WaitForSeconds(0.1f);
        // can not reel into when this is true
        GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
    }
}
