using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour {
    [SerializeField]
    string level1;

public void LoadLevelOne()
    {
        SceneManager.LoadScene(level1);
    }   
}
