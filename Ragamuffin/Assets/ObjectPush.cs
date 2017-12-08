using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour {
    [SerializeField]
    float force;
    [SerializeField]
    float maxSpeed;
	void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "PushAbleObject")
        {

            //   other.gameObject.GetComponent<Sliide>().SetSwap(true);
            if (transform.position.x < other.gameObject.transform.position.x)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0));
                if (other.gameObject.GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
                {
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, 0);
                }
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-force, 0));
                if (other.gameObject.GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
                {
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, 0);
                    //Debug.Break();
                }

            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PushAbleObject")
        {
          //  other.gameObject.GetComponent<Sliide>().SetSwap(false);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }


}
