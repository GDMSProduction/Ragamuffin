using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public int recipe = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GoodFood>())
        {
            addFood();
            Destroy(other.gameObject);
        }
    }
    private void addFood()
    {
        recipe++;
        if(recipe == 3)
        {
            solved();
        }
    }
    private void solved()
    {
        Debug.Log("Cooked!");
    }
}
