using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour {
    [SerializeField]
    GameObject respawn;
    [SerializeField]
    PlayerHeath heath;
  public  bool delaydeath;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < -20||heath.GetHeath() <=0&&delaydeath==false)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = respawn.transform.position;
            heath.ResetHeath();
        }
        else if(delaydeath)
        {
            StartCoroutine(catdeath());
        }

    }
    IEnumerator catdeath()
    {
        yield return new WaitForSeconds(2);
        transform.position = respawn.transform.position;
        heath.ResetHeath();
        StopAllCoroutines();
    }
}
