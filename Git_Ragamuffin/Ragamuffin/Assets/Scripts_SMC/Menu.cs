using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public Image[] images;
    private bool onOff = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            onOff = !onOff;
            menu.SetActive(onOff);
        }
    }
    public void UpdateCollection(int num){
        images[num].GetComponent<Image>().color = Color.white;
    }
}
