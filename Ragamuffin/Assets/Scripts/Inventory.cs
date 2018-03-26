using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Image[] itemImages = new Image[numItemSlots];
    public InVentroyObject[] items = new InVentroyObject[numItemSlots];
    public List<Image> TUrtle = new List<Image>();
    public GameObject[] pos = new GameObject[3];
    public int x;
    public const int numItemSlots = 5;
    public float switchCooldown = 0.25f;
    float countdownProgress;
    [SerializeField]
    Color black;
    Color ogcolor;
    float timer = 0;
    int numberofitemsinhand = 0;
    float width;
    float height;
    private void Start()
    {
        width = itemImages[0].rectTransform.sizeDelta.x;
        height = itemImages[0].rectTransform.sizeDelta.y;


    }
    private void Update()
    {
        timer += Time.deltaTime;
        black.r = 0;
        black.g = 0;
        black.b = 0;
        countdownProgress -= Time.deltaTime;
        if (countdownProgress <= 0 && ((Input.GetAxisRaw("Mouse ScrollWheel")) > float.Epsilon)){
     
            timer = 0;
            countdownProgress = switchCooldown;
            Image previousimage = itemImages[numberofitemsinhand - 1];
            InVentroyObject previousobject = items[numberofitemsinhand - 1];
            for(int i = 0; i < numberofitemsinhand; ++i)
            {
                itemImages[i].rectTransform.sizeDelta = new Vector2(width, height);

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
        else if(countdownProgress <= 0 && ((Input.GetAxisRaw("Mouse ScrollWheel")) <0))
        {

            timer = 0;
            countdownProgress = switchCooldown;
            Image previousimage = itemImages[0];
            InVentroyObject previousobject = items[0];
            for (int i = numberofitemsinhand-1; i >=0 ; i--)
            {
                itemImages[i].rectTransform.sizeDelta = new Vector2(width, height);

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
        }
        itemImages[1].rectTransform.sizeDelta = new Vector2(150, 150);
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
        numberofitemsinhand++;
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
    public InVentroyObject GetItem()
    {
        return items[1];
    }
}
