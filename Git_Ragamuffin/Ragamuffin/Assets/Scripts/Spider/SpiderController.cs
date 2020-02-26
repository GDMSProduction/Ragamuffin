using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderStuff
{
	public class SpiderController : MonoBehaviour
	{
		#region Variables
		#region Serialized
		[Tooltip("Put all the triggers you want this spider to use here!")]
		[SerializeField] private SpiderTriggerVolume[] triggerVolumes = null;
		[Tooltip("How many units per second should the spider move?")]
		[SerializeField] private float moveDistPerSecond = .3f;
		[Tooltip("Rag is attached to this transform while moving and detached when done")]
		[SerializeField] private Transform ragParent = null;
		#endregion

		private float moveDistPerFixedUpdate = 1; //stores the distance we move per fixed update, calculated from our moveDistPerSecond
		private const float stopDistance = .03f; //how close do we have to get to our target position before we consider ourselves to be at the position? (arbitrarily small)  
		#endregion

		#region Methods

		#region Init/De-Init
		private void Start()
		{
			if (NoReferencesAreNull() == false) //if any of our references are null, 
			{
				return; //trying to continue will throw NREs, get out of here! 
			}

			//We don't want our triggers to move with us, but we want them to be under us in the level hierarchy during design. So we un-parent them upon initialization. 
			for (int i = 0; i < triggerVolumes.Length; i++)
			{
				triggerVolumes[i].transform.parent = null;
			}

			moveDistPerFixedUpdate = (float)((float)((float)1f / (float)50f) * moveDistPerSecond); //calculate how far we should move per tick of FixedUpdate 
		}

		private void OnEnable()
		{
			StopListenening(); //always desubscribe before you suscribe to prevent double subscriptions 
			StartListening(); 
		}

		private void OnDisable()
		{
			StopListenening();
		}
		#endregion

		#region Helper methods
		/// <summary>Checks serialized values for null</summary>
		private bool NoReferencesAreNull() 
		{
			if (triggerVolumes == null || triggerVolumes.Length == 0)
			{
				Debug.LogWarning(PreErrorHeader() + "SpiderController trigger volume not set! looking for one!");
				triggerVolumes = GetComponentsInChildren<SpiderTriggerVolume>();
			}

			if (triggerVolumes == null)
			{
				Debug.LogError(PreErrorHeader() + "No trigger volume found on root object or in children!");
				enabled = false;
			}

			if (ragParent == null)
			{
				Debug.LogWarning(PreErrorHeader() + "Rag parent transform reference not set!\nSetting it to " + gameObject.name);
				ragParent = transform;
			}

			return enabled; //we set enabled to false if we can't continue, return that value so our start method can arrest its execution if this check fails 
		}

		/// <summary>Returns the name of our object + ".SpiderController" </summary>
		private string PreErrorHeader() { return gameObject.name + ".SpiderController: "; } 

		/// <summary>Desubscribes from our trigger volumes' events </summary>
		private void StopListenening()
		{
			if (triggerVolumes != null)
			{
				for (int i = 0; i < triggerVolumes.Length; i++)
				{
					triggerVolumes[i].Evt_TriggerEntered -= MoveToPosition;
				}
			}
		}

		/// <summary>Subscribes to our trigger volumes' events </summary>
		private void StartListening()
		{
			if (triggerVolumes != null)
			{
				for (int i = 0; i < triggerVolumes.Length; i++)
				{
					triggerVolumes[i].Evt_TriggerEntered += MoveToPosition;
				}
			}
		}

		private void StopTriggersLooking() //rename to something better!!! 
		{
			for (int i = 0; i < triggerVolumes.Length; i++)
			{
				triggerVolumes[i].StopLookingForRag();
			}
		}
		#endregion

		#region Movement
		private void MoveToPosition(Rag_Movement ragMvmt, Vector3 targetPos)
		{
			StopCoroutine("Rut_MoveToTargPos");
			StartCoroutine(Rut_MoveToPosition(ragMvmt, targetPos));
		}

		private IEnumerator Rut_MoveToPosition(Rag_Movement ragMvmt, Vector3 targetPos)
		{
			Debug.Log("STARTETD");
			//disable rag movement
			ragMvmt.transform.position = ragParent.position;
			ragMvmt.disableControls = true; 
			bool originalUseGravity = ragMvmt.useGravity;
			ragMvmt.useGravity = false;
			Transform originalRagParent = ragMvmt.gameObject.transform.parent; //Grab a reference to rag's current parent 
			ragMvmt.gameObject.transform.parent = ragParent; //Set rag's parent so he'll move along with us 
			//ragMvmt.enabled = false;
			ragMvmt.SetVelocity(Vector3.zero);
			Vector3 moveDir = (targetPos - transform.position).normalized; //calculate the direction we should be moving 

			while (Vector3.Distance(transform.position, targetPos) > stopDistance) //While we aren't super close to our target position, 
			{
				//transform.position += moveDir * moveDistPerFixedUpdate; //move towards it 
				transform.Translate(moveDir * moveDistPerFixedUpdate);
				//Debug.Log("TICFK");
				yield return new WaitForFixedUpdate(); //wait for the next tick of FixedUpdate 

			}

			//Once we're super close to our target, 
			transform.position = targetPos; //snap to the correct position 

			//re-enable rag's movement 
			//ragMvmt.gameObject.transform.parent = originalRagParent; //set rag's parent back to whatever the hell it was before we touched it 
			ragMvmt.disableControls = false; //re-enable rag's movement 
			ragMvmt.useGravity = originalUseGravity;
			//ragMvmt.enabled = true;
			//Debug.Log("STOPPED");
			StopTriggersLooking(); //Rag is probably going to step off the spider into a trigger, tell them to stop looking for him until he leaves the trigger 
		}

		private void SetRagEnabled(bool enabled)
		{
			if(!enabled)
			{

			}
			else
			{

			}
		}
		#endregion 
		#endregion

	}
}

