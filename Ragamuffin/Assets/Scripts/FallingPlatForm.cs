using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatForm : MonoBehaviour {
    bool fall;
    [SerializeField]
    float falldownseconds;
    Vector3 startspot;

	// Use this for initialization
	void Start () {
        startspot = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        if (fall == true)
        {
            GetComponent<Rigidbody2D>().gravityScale = 4;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    IEnumerator falldown()
    {
        yield return new WaitForSeconds(falldownseconds);
        fall = true;
        StartCoroutine(Reset());
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);
        fall = false;
        transform.position = startspot;
            GetComponent<Rigidbody2D>().gravityScale = 0;

        StopAllCoroutines();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Break();
            StartCoroutine(falldown());
        }
    }
}
