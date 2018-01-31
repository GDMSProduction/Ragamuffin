using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatClaw : MonoBehaviour {
    [SerializeField]
    float damage;
    [SerializeField]
    float knockback;
    [SerializeField]
    catSearch catmovement;
    bool firstattc;
    [SerializeField]
    float charge;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Player"&&catmovement.getattac()==false)
        {
            catmovement.SetAttac(true);
          
            charge += 10;
            if (charge >= 100)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
                if (other.gameObject.transform.position.x > transform.position.x)
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
                else
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
                }
                charge = 0;
            }
            if (other.gameObject.GetComponent<PlayerMovement>().GetHeath() <= 0+damage)
            {
                other.gameObject.GetComponent<PlayerMovement>().CatFalty();
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().takeDamage(damage);
            }
          
        }
    }

}
