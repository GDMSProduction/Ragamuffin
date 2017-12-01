using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour {
    [SerializeField]
    float force;
	void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "PushAbleObject")
        {
           
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(force,0));
        }
    }
}
