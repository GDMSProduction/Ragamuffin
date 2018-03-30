using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Image[] itemImages = new Image[numItemSlots];
    public InVentroyObject[] items = new InVentroyObject[numItemSlots];
    public List<Image> TUrtle = new List<Image>();
    public GameObject[] pos = new GameObject[numItemSlots];
    public int x;
    public const int numItemSlots = 6;
    public float switchCooldown = 0.25f;
    float countdownProgress;
    [SerializeField]
    Color black;
    Color ogcolor;
    float timer = 0;
    int numberofitemsinhand = 0;
    float width;
    float height;
    bool addeditem;
    bool switching;
    int[] gontlet = new int[6];
    private void Start()
    {
        width = itemImages[0].rectTransform.sizeDelta.x;
        height = itemImages[0].rectTransform.sizeDelta.y;
        for (int i = 0; i < gontlet.Length; ++i)
        {
            gontlet[i] = -1;
        }

    }
    private void Update()
    {
        //for(int i=0; i < itemImages.Length; ++i)
        //{
        //    if (itemImages[i].sprite == null&&i+1!=items.Length&&itemImages[i+1].sprite!=null)
        //    {
        //        itemImages[i] = itemImages[i + 1];
        //      //  itemImages[i].sprite = itemImages[i + 1].sprite;
        //        itemImages[i + 1].sprite = null;
        //        items[i] = items[i + 1];
        //        items[i + 1] = null;
        //    }
        //}
        for (int i = 0; i < itemImages.Length; ++i)
        {
            if (itemImages[i].sprite == null && i + 1 < itemImages.Length && itemImages[i + 1].sprite != null)
            {
                Image tempt;
                tempt = itemImages[i];
                itemImages[i] = itemImages[i + 1];
                items[i] = items[i + 1];
                items[i + 1] = null;
                itemImages[i + 1] = tempt;

                switching = true;
            }
        }
        timer += Time.deltaTime;
        black.r = 0;
        black.g = 0;
        black.b = 0;
        countdownProgress -= Time.deltaTime;
        int counter = 0;
        for (int i = 0; i < gontlet.Length; ++i)
        {
            if (gontlet[i] != -1)
            {
              
                if (gontlet[i] == 0 || gontlet[i] == 1 || gontlet[i] == 2)
                {
                    counter++;
                }
            }
          
        }
        for(int i=0; i < gontlet.Length; ++i)
        {
            if (gontlet[i]!=-1)
            {
                Color aplhareduce;
                aplhareduce = itemImages[gontlet[i]].color;
                aplhareduce.a -= (float)0.01;
                itemImages[gontlet[i]].color = aplhareduce;
            }
            else if(counter>=2&&i<3)
            {
                Color aplhareduce;
                aplhareduce = itemImages[i].color;
                aplhareduce.a -= (float)0.01;
                itemImages[i].color = aplhareduce;
            }
        }
        if (countdownProgress <= 0 && ((Input.GetAxisRaw("Mouse ScrollWheel")) > float.Epsilon)||addeditem)
        {
            switching = false;
            addeditem = false;

            timer = 0;
            countdownProgress = switchCooldown;
            Image previousimage = itemImages[numberofitemsinhand - 1];
            InVentroyObject previousobject = items[numberofitemsinhand - 1];
            for (int i = 0; i < numberofitemsinhand; ++i)
            {
                itemImages[i].rectTransform.sizeDelta = new Vector2(width, height);

                InVentroyObject temptitem = items[i];
                Image temptImage = itemImages[i];
                itemImages[i] = previousimage;
                items[i] = previousobject;
                previousimage = temptImage;
                previousobject = temptitem;

            }
            for (int i = 0; i < 3; ++i)
            {
                if (itemImages[i].sprite != null)
                {
                    Color aplhareduce;
                    aplhareduce = itemImages[i].color;
                    aplhareduce.a = 1;
                    itemImages[i].color = aplhareduce;
                }
            }


        }
        else if (countdownProgress <= 0 && ((Input.GetAxisRaw("Mouse ScrollWheel")) < 0)||addeditem)
        {
            switching = false;
            addeditem = false;
            timer = 0;
            countdownProgress = switchCooldown;
            Image previousimage = itemImages[0];
            InVentroyObject previousobject = items[0];
            for (int i = numberofitemsinhand - 1; i >= 0; i--)
            {
                itemImages[i].rectTransform.sizeDelta = new Vector2(width, height);

                InVentroyObject temptitem = items[i];
                Image temptImage = itemImages[i];
                itemImages[i] = previousimage;
                items[i] = previousobject;
                previousimage = temptImage;
                previousobject = temptitem;
            }
            for (int i = 0; i < 3; ++i)
            {
                if (itemImages[i].sprite != null)
                {
                    Color aplhareduce;
                    aplhareduce = itemImages[i].color;
                    aplhareduce.a = 1;
                    itemImages[i].color = aplhareduce;
                }
            }

            //aplhareduce = itemImages[1].color;
            //aplhareduce.a = 1;
            //itemImages[1].color = aplhareduce;

            //aplhareduce = itemImages[2].color;
            //aplhareduce.a = 1;
            //itemImages[2].color = aplhareduce;
        }
     
        if (itemImages[1].sprite != null&&switching==false)
        {
            itemImages[1].rectTransform.sizeDelta = new Vector2(100, 100);
        } 
        else if(switching==false)
        {
            itemImages[0].rectTransform.sizeDelta = new Vector2(100, 100);
        }
        if (timer > 4&&switching==false)
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

            aplhareduce = itemImages[3].color;
            aplhareduce.a -= (float)0.01;
            itemImages[3].color = aplhareduce;

            aplhareduce = itemImages[4].color;
            aplhareduce.a -= (float)0.01;
            itemImages[4].color = aplhareduce;
        }
      
        if (itemImages[2].sprite != null)
        {
            for(int i=3; i < itemImages.Length; ++i)
            {
                Color tempt =  itemImages[i].color;
                tempt.a = 0;
                itemImages[i].color = tempt;
            }
        }

        //   ogcolor = itemImages[1].color;
        //  itemImages[1].color = black;
        if (itemImages[1].sprite != null&&switching==false)
        {
            itemImages[0].transform.position = pos[0].transform.position;
            itemImages[1].transform.position = pos[1].transform.position;
            itemImages[2].transform.position = pos[2].transform.position;
        }
        else if(switching==false)
        {
            itemImages[0].transform.position = pos[1].transform.position;
        }
        if (switching == true)
        {
            if (itemImages[1].sprite != null)
            {
                itemImages[0].transform.position = pos[0].transform.position;
                itemImages[1].transform.position = pos[1].transform.position;
                itemImages[2].transform.position = pos[2].transform.position;
            }
            else 
            {
                itemImages[0].transform.position = pos[1].transform.position;
            }
            if (itemImages[1].sprite != null )
            {
                itemImages[1].rectTransform.sizeDelta = new Vector2(100, 100);
            }
            else 
            {
                itemImages[0].rectTransform.sizeDelta = new Vector2(100, 100);
            }
            for (int i=0; i < 3; ++i)
            {
             //   Debug.Break();
                Color aplhareduce;
                aplhareduce = itemImages[i].color;
                aplhareduce.a += (float)0.01;
                itemImages[i].color = aplhareduce;
            }
            if (itemImages[0].color.a == 1)
            {
                switching = false;
            }

        }

          for(int i=0;i<itemImages.Length;++i)
        {
            if (itemImages[i].sprite == null)
            {
                Color outofsite = itemImages[i].color;
                outofsite.a = 0;
                itemImages[i].color = outofsite;
            }
        }
          
    }
    public void AddItem(InVentroyObject itemToAdd)
    {
        addeditem = true;
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
        
    public void RemoveItem(InVentroyObject itemToRemove)
    {
        
        for (int i = 0; i < items.Length; ++i)
        {
           
            if (items[i] == itemToRemove)
            {
                gontlet[i] = i;
            StartCoroutine(RemoveItemPause());


            }

        }
    }
    IEnumerator RemoveItemPause()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < gontlet.Length; ++i)
        {
            if (gontlet[i] != -1)
            {
                items[gontlet[i]] = null;
                itemImages[gontlet[i]].sprite = null;
                if (gontlet[i] + 1 != items.Length)
                {
                   // itemImages[gontlet[i]].sprite = itemImages[gontlet[i] + 1].sprite;
                  //  items[gontlet[i]] = items[gontlet[i] + 1];
                }
                --numberofitemsinhand;
                gontlet[i] = -1;

            }
        }
    }
    public InVentroyObject GetItem()
    {
        return items[1];
    }
    public List<InVentroyObject> GetKeys()
    {
        List<InVentroyObject> listofkeys = new List<InVentroyObject>();
        for(int i=0; i < numberofitemsinhand; ++i)
        {
            if (items[i].GetComponent<Key>() != null)
            {
                listofkeys.Add(items[i]);
            }
        }
        return listofkeys;
    }
}
