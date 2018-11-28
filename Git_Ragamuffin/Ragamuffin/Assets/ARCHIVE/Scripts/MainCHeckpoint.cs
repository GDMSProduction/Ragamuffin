using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCHeckpoint : MonoBehaviour {
    [SerializeField]
    GameObject[] CheckPoints = new GameObject[10];
    [SerializeField]
    GameObject[] ObjectsToMove = new GameObject[10];
    [SerializeField]
    GameObject[] MainCheckPointLocation = new GameObject[10];

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<death>().checkpoints = CheckPoints;
            other.GetComponent<death>().ObjectstoReset = ObjectsToMove;
            other.GetComponent<death>().zeroLiveResetPoint = MainCheckPointLocation;
        }
    }
}
