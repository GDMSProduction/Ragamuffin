using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ReSpawn();
    }

    public void ReSpawn()
    {
        transform.position = Checkpoint.CurrentCheckpoint.position;
        transform.rotation = Checkpoint.CurrentCheckpoint.rotation;
    }
}