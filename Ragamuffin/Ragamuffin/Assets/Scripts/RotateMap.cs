﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMap : MonoBehaviour {
    [SerializeField]
    FazeOutScreen fazeOut;
    bool hasmapRotated;
    [SerializeField]
    GameObject map;
    GameObject Camera;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Vector3 playernewposition;
    [SerializeField]
    Vector3 mapnewPosition;
    [SerializeField]
    Vector3 cameranewpositon;
    [SerializeField]
    Quaternion playernewrotaiton;
    [SerializeField]
    Quaternion mapnewROation;
    [SerializeField]
    Quaternion cameranewration;
    [SerializeField]
    float fadeTimer=3;
    
    private void Start()
    {
       // mapnewROation = map.transform.rotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (hasmapRotated == false)
            {
                fazeOut.Fade(true, fadeTimer);
                hasmapRotated = true;
                StartCoroutine(SceneNewLocations());
            }
        }
    }
    IEnumerator SceneNewLocations()
    {
        yield return new WaitForSeconds(fadeTimer);

        // Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        PlayerMovement.DisableMovement = true;
        Player.transform.position = playernewposition;
      //  Camera.transform.position = cameranewpositon;
        map.transform.position = mapnewPosition;
        map.transform.rotation = mapnewROation;
            SwitchingZ.SetMapChangeTrue();
        yield return new WaitForSeconds(fadeTimer);
        fazeOut.Fade(false, fadeTimer);
        yield return new WaitForSeconds(fadeTimer);
        PlayerMovement.DisableMovement = false;



    }
}
