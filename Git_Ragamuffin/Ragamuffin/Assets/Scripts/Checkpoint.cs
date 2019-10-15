//Changelog 
// 10/15/2019 Colby Peck: Made SetCurrentCheckpoint method, added CheckpointActivated event, added event invokation to SetCurrentCheckpoint 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public static Transform CurrentCheckpoint;
	[SerializeField] bool Reusable;
	[SerializeField] Transform SpawnPoint;
	[SerializeField] GameObject Effect;

	/// <summary>
	/// This event is called any time a checkpoint is activated; it passes the checkpoint that calls the event as an argument 
	/// </summary>
	public static System.Action<Checkpoint> CheckpointActivated;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			SetCurrentCheckpoint(this);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			SetCurrentCheckpoint(this);
		}
	}

	public static void SetCurrentCheckpoint(Checkpoint point)
	{
		if (point.SpawnPoint != null)
			CurrentCheckpoint = point.SpawnPoint;
		else
			CurrentCheckpoint = point.transform;

		if (point.Effect != null && CurrentCheckpoint != point.SpawnPoint)
			Destroy(Instantiate(point.Effect, point.transform.position, point.transform.rotation), .5f);

		CheckpointActivated.Invoke(point);

		if (!point.Reusable)
			point.gameObject.SetActive(false);
	}
}