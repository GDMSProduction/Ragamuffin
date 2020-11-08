using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    private bool onOff = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            onOff = !onOff;
            menu.SetActive(onOff);
        }
    }
}
