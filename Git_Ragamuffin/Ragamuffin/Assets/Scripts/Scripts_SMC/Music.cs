using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
    public static Music Instance;
     AudioSource audioSource;
     void Awake(){
         audioSource = GetComponent<AudioSource>();
         if (Instance!=null){
             GameObject.Destroy(Instance);
     } 
        else {
            Instance = this;         
            DontDestroyOnLoad(this);
     }
    }
    void Update(){
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
    }
}
