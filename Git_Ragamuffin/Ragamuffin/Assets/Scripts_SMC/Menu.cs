using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject collectables;
    public Image[] images;
    private bool menuOnOff = false;
    private bool collectablesOnOff = false;
    string[] picNames = {"1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"};

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            menuOnOff = true;
            menu.SetActive(true);
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.P)){
            for (int i = 0; i < 20; i++){
                Debug.Log("pictureItems " + PlayerPrefs.GetInt(picNames[i]));
            }
        }
        
    }
    public void GetSaveData(string numArry, int numBool){
        PlayerPrefs.GetInt(numArry, numBool);
        Debug.Log("pictureItems " + PlayerPrefs.GetInt(numArry));
    }
    // public void SetSaveData(int numArry, string numBool){
    //     PlayerPrefs.SetInt("zero", numBool);
    //     pictureItems[numArry] = PlayerPrefs.GetInt("zero");
    //     Debug.Log("pictureItems " + pictureItems[numArry]);
    // }
    public void UpdateCollection(int num){
        images[num].GetComponent<Image>().color = Color.white;
    }

    public void MenuOff(){
        menuOnOff = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CollectionOnOff(){
        collectablesOnOff = !collectablesOnOff;
        collectables.SetActive(collectablesOnOff);
    }
}
