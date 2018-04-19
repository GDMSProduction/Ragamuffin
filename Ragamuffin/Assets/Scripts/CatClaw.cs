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
    void OnTriggerStay2D(Collider2D other) {

        if (other.tag == "Player"&&catmovement.getattac()==false)
        {
            catmovement.SetAttac(true);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            charge += 10;
            if (charge >= 100&& catmovement.getattac() == true)
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
            else if( catmovement.getattac() == true)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
                if (other.gameObject.transform.position.x > transform.position.x)
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
                else
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
                }

            }
            if (other.gameObject.GetComponent<PlayerMovement>().GetHeath() <= 0+damage && catmovement.getattac() == true)
            {
                other.gameObject.GetComponent<PlayerMovement>().CatFalty();
            }
            else if( catmovement.getattac() == true)
            {
                other.gameObject.GetComponent<PlayerMovement>().takeDamage(damage);
            }
          
        }
    }

}
