using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Transform CurrentCheckpoint;
    [SerializeField] bool Reusable;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject Effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (SpawnPoint != null)
                CurrentCheckpoint = SpawnPoint;
            else
                CurrentCheckpoint = transform;

            if (Effect != null && CurrentCheckpoint != SpawnPoint)
                Destroy(Instantiate(Effect, transform.position, transform.rotation), .5f);

            if (!Reusable)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (SpawnPoint != null)
                CurrentCheckpoint = SpawnPoint;
            else
                CurrentCheckpoint = transform;

            if (Effect != null && CurrentCheckpoint != SpawnPoint)
                Destroy(Instantiate(Effect, transform.position, transform.rotation), .5f);

            if (!Reusable)
                gameObject.SetActive(false);
        }
    }
}