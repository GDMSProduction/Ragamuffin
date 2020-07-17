//Changelog:
// 10/25/2019 Colby Peck: Added call to PlayerHealth.Init() to ReSpawn()

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnManager : MonoBehaviour
{
	[SerializeField] bool printLogs = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ReSpawn();
    }

    public void ReSpawn()
    {
		if (printLogs)
			Debug.Log("ReSpawnManager: Respawning object with name: " + gameObject.name);

        transform.position = Checkpoint.CurrentCheckpoint.position;
        transform.rotation = Checkpoint.CurrentCheckpoint.rotation;
		PlayerHealth.Init();
    }
}