using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private Menu menu;
    private int savePictureData = 1;    // When OnTriggerEnter is called this will change playerprefs int to 1 which means
                                        // player has collected item and will be turned on / displayed when that 
                                        // specific menu opens.
                                        
     [SerializeField]
    private int pictureItemsArryNum;    //number neeeds to match in array in menu script. string[] picNames. 
                                        //If picture is collectable #1 then this number should be 0 in the inspector. 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            gameObject.SetActive(false);
            menu.SetSaveData(pictureItemsArryNum,savePictureData);
        }
    }
}
