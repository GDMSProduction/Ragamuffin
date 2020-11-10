using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private Menu menu;
    [SerializeField]
    private int num;
    [SerializeField]
    private string savePictureData;
     [SerializeField]
    private int pictureItemsArryNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            menu.UpdateCollection(num);
            gameObject.SetActive(false);
            menu.GetSaveData(savePictureData,pictureItemsArryNum);
        }
    }
}
