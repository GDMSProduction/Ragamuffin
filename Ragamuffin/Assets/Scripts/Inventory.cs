using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Image[] itemImages = new Image[numItemSlots];
    public InVentroyObject[] items = new InVentroyObject[numItemSlots];
    public GameObject[] pos = new GameObject[3];
    public int x;
    public const int numItemSlots = 4;
    public float switchCooldown = 0.25f;
    float countdownProgress;
    [SerializeField]
    Color black;
    Color ogcolor;
    float timer = 0;
    
    private void Update()
    {
        timer += Time.deltaTime;
        black.r = 0;
        black.g = 0;
        black.b = 0;
        countdownProgress -= Time.deltaTime;
        if (countdownProgress <= 0 && (Mathf.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) > float.Epsilon)){
     
            timer = 0;
            countdownProgress = switchCooldown;
            Image previousimage = itemImages[numItemSlots - 1];
            InVentroyObject previousobject = items[numItemSlots - 1];
            for(int i = 0; i < numItemSlots; ++i)
            {
                    InVentroyObject temptitem = items[i];
                    Image temptImage = itemImages[i];
                    itemImages[i] = previousimage;
                    items[i] = previousobject;
                    previousimage = temptImage;
                    previousobject = temptitem;
            }
            Color aplhareduce;
            aplhareduce = itemImages[0].color;
            aplhareduce.a = 1;
            itemImages[0].color = aplhareduce;

            aplhareduce = itemImages[1].color;
            aplhareduce.a = 1;
            itemImages[1].color = aplhareduce;

            aplhareduce = itemImages[2].color;
            aplhareduce.a = 1;
            itemImages[2].color = aplhareduce;
            // itemImages[2].color = ogcolor;


        }
        if (timer > 4)
        {
            Color aplhareduce;
            aplhareduce = itemImages[0].color;
            aplhareduce.a -= (float)0.01;
            itemImages[0].color = aplhareduce;
            
            aplhareduce = itemImages[1].color;
            aplhareduce.a -= (float)0.01;
            itemImages[1].color = aplhareduce;
      
            aplhareduce = itemImages[2].color;
            aplhareduce.a -= (float)0.01;
            itemImages[2].color = aplhareduce;
        }
        
     //   ogcolor = itemImages[1].color;
      //  itemImages[1].color = black;
        itemImages[0].transform.position = pos[0].transform.position;
        itemImages[1].transform.position = pos[1].transform.position;
        itemImages[2].transform.position = pos[2].transform.position;
        Color outofsite = itemImages[3].color;
        outofsite.a = 0;
        itemImages[3].color = outofsite;
    }
    public void AddItem(InVentroyObject itemToAdd)
    {
        if (itemToAdd.sprite == null)
        {
            return; 
        }
        for(int i=0; i < items.Length; ++i)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                return;
                
            }
        }
    }
}
