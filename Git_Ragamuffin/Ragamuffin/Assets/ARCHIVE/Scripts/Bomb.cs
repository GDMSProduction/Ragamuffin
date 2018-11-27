using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    bool dealdamage;
    [SerializeField]
    BoxCollider2D mycolider;
    [SerializeField]
    PlayerHeath player;
    [SerializeField]
    float explosionTime = 2;
    bool alreadystartedcontines;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(dealdamage == false&&alreadystartedcontines==false)
        {
            StartCoroutine(BlowUp());
            alreadystartedcontines = true;
        }
        else if(dealdamage)
        {
            player.takeDamage(10);
        }
    }
    IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(explosionTime);
        mycolider.enabled = true;
        dealdamage = true;
        StartCoroutine(DestoryBomb());

    }
    IEnumerator DestoryBomb()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
