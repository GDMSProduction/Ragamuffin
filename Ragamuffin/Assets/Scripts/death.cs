using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour {
    [SerializeField]
    GameObject respawn;
    [SerializeField]
    PlayerHeath heath;
  public  bool delaydeath;
    [SerializeField]
    float lives = 2;
  
  public  GameObject mainSpanpoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < -20||heath.GetHeath() <=0&&delaydeath==false&&lives!=0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(respawn!=null)
            transform.position = respawn.transform.position;
            else
            {
                transform.position = mainSpanpoint.transform.position;
            }
            heath.ResetHeath();
            lives -= 1;
        }
        else if(delaydeath)
        {
            StartCoroutine(catdeath());
        }
        else if (lives == 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            lives = 3;

            transform.position = mainSpanpoint.transform.position;
        }

    }
    IEnumerator catdeath()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = respawn.transform.position;
        heath.ResetHeath();
        lives -= 1;
        StopAllCoroutines();
    }
    public void setRespawn(GameObject _respawn)
    {
        respawn = _respawn;
    }
}
