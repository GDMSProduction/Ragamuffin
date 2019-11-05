//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck 
//               Date: 10/25/2019 
//            Purpose: Create a damage volume to damage Rag over time 
// Associated Scripts: PlayerHealth.cs 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
// 10/25/2019 Colby Peck: Created script: Added OnTriggerEnter/Exit methods, StartDamageTicks method, and DamageRoutine coroutine 
// 11/05/2019 Colby Peck: Repaired some logic errors in trigger enter/exit and StartDamageTicks() 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class HeatDamageVolume : MonoBehaviour
{
	#region Fields/Properties
	#region Serialized
	[Header("How much time (seconds) should there be between damage ticks?")]
	[SerializeField] private float damageTickTime = .1f;

	[Header("How much damage should we apply each tick?")]
	[SerializeField] private float damageAmount = .1f;

	[Tooltip("Print debug logs?")]
	[SerializeField] private bool printLogs = false;
	#endregion

	#region Private
	private Collider col;
	private bool damageTicksActive = false;
	#endregion
	#endregion

	#region Methods
	#region Initialization/De-Initialization
	void Start()
	{
		if (GetComponents<MeshRenderer>().Length < 0) //If we have any renderers attached, 
		{
			//turn them off 
			for (int i = 0; i < GetComponents<MeshRenderer>().Length; i++)
			{
				GetComponents<MeshRenderer>()[i].enabled = false;
			}
		}

		if (col == null) //If we don't have a reference to our collider, 
		{
			col = GetComponent<Collider>(); //Get a reference to it 
		}

		col.isTrigger = true; //Our collider needs to be a trigger 
	}

	private void OnDestroy()
	{
		StopAllCoroutines(); //Don't want any stray coroutines running after we're destroyed 
	}
	#endregion

	#region Damage Dealing Methods
	private IEnumerator DamageRoutine()
	{
		while (damageTicksActive) //While we're meant to damage the player, 
		{
			yield return new WaitForSeconds(damageTickTime); //Wait for our specified amount of time, (this is before the damage so the player won't be immediately damaged upon entering the volume) 

			if (damageTicksActive) //This check is here so we don't damage the player if they leave the volume before our timer ticks over 
			{
				PlayerHealth.TakeDamage(damageAmount); //Damage the player 
			}
		}
	}

	private void StartDamageTicks()
	{
		if (!damageTicksActive) //If we aren't already damaging the player, 
		{
			damageTicksActive = true; //set our 'damaging the player' flag to true 
			StartCoroutine(DamageRoutine()); //Start our damage coroutine 
		}
	}
	#endregion

	#region Trigger Enter/Exit
	private void OnTriggerEnter(Collider other) //When something enters our trigger, 
	{
		if (printLogs)
			Debug.Log("Object \"" + other.gameObject.name + "\' entered damage volume: " + gameObject.name);

			if (other.gameObject == GameManager.Player) //If the other thing is the player, 
		{
			if (printLogs)
				Debug.Log("Player entered damage volume: " + gameObject.name);

			StartDamageTicks(); //Damage the player 
		}
	}

	private void OnTriggerExit(Collider other) //When something leaves our trigger, 
	{
		if (printLogs)
			Debug.Log("Object \"" + other.gameObject.name + "\' exited damage volume: " + gameObject.name);

		if (other.gameObject == GameManager.Player) //If the other thing is the player, 
		{
			if (printLogs)
				Debug.Log("Player exited damage volume: " + gameObject.name);

			damageTicksActive = false; //Tell our coroutine to stop damaging the player 
		}
	}
	#endregion
	#endregion
}
