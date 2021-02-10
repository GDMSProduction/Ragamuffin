//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck 
//               Date: 10/25/2019 
//            Purpose: Create a class for managing player health; including damage and death 
// Associated Scripts: ReSpawnManager.cs 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
// 10/25/2019 Colby Peck: Created script: Added Awake, Init, Start, TakeDamage, and Die methods

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
	#region Fields/Properties

	#region Serialized
	[Tooltip("How much maximum health should Rag have?")]
	[SerializeField] private float maxHealth = 5;

	[Tooltip("Rag's current health")]
	[SerializeField] private float curHealth;
	#endregion

	private static PlayerHealth playerHealth = null; //Used for singleton pattern 

	#endregion

	#region Methods

	#region Initialization/De-Initialization

	private void Awake()
	{
		//Singleton pattern allows us to make static methods that can be called from anywhere without having to feed PlayerHealth instances all over the place 
		if (playerHealth == null)
		{
			playerHealth = this;
		}
		if (playerHealth != this)
		{
			Destroy(this);
		}
	}

	private void Start()
	{
		Init();
	}

	/// <summary>
	/// Initializes the player's health; sets the player's health to full
	/// </summary>
	public static void Init()
	{
		playerHealth.curHealth = playerHealth.maxHealth; //Set our health to full 
	}

	#endregion

	#region Public Static Methods
	/// <summary>
	/// Damages the player. If the player's health falls to 0 or less, the player will die
	/// </summary>
	/// <param name="damageAmount">How much damage are we dealing to the player?</param>
	public static void TakeDamage(float damageAmount)
	{
		playerHealth.curHealth -= damageAmount; //Decrease our health by the specified amount 
		if (playerHealth.curHealth <= 0) //If we're out of health, 
		{
			playerHealth.curHealth = 0; //Make sure we don't ever get negative health 
			Die(); //Die.
		}
	}

	/// <summary>
	/// Heals the player. Will not allow the player's health to exceed their maximum.
	/// </summary>
	/// <param name="healAmount">How much are we healing the player?</param>
	public static void Heal(float healAmount)
	{
		playerHealth.curHealth += Mathf.Clamp(playerHealth.curHealth + healAmount, 0, playerHealth.maxHealth); //Don't want to heal over max health 
	}

	/// <summary>
	/// Kills the player
	/// </summary>
	public static void Die()
	{
		Debug.Log("PlayerHealth.Die() called.\nThis method isn't fully implemented yet!"); //Death should probably bring up a screen or display a message to the player or something in the future 
		GameManager.Player.GetComponentInChildren<ReSpawnManager>().ReSpawn(); //Respawn the player 
	}
	#endregion

	#endregion
}
