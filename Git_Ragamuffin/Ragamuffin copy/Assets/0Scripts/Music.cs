using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
    // Instance is used to make sure there is only one audiosource.
    public static Music Instance = null;
    AudioSource audioSource;
    void Awake(){
         audioSource = GetComponent<AudioSource>();
         DontDestroyOnLoad(this);
    }
    void Update(){
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Instance = this;
            Destroy(gameObject);
        }
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
    }
}
