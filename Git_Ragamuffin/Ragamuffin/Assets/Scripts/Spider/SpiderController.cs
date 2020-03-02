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
		private const float stopDistance = .1f; //how close do we have to get to our target position before we consider ourselves to be at the position? (arbitrarily small)  
		private const float movingToRagModifier = 3f; //Move speed is multiplied by this when moving towards rag and not carrying him. 

		//variables for storing rag's state when grabbing 
		private Transform originalRagParent;
		private bool originalUseGravity;
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

			//see if casts are uneeded later
			moveDistPerFixedUpdate = 1.0f / 60.0f * moveDistPerSecond; //calculate how far we should move per tick of FixedUpdate 
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

		private void SetTriggersLooking(bool look)
		{
			for (int i = 0; i < triggerVolumes.Length; i++)
			{
				triggerVolumes[i].SetLookingForRag(look);
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
			Vector3 vecToTarget;
			//grab references to rag's original state 
			originalUseGravity = ragMvmt.useGravity;
			originalRagParent = ragMvmt.gameObject.transform.parent;

			SetRagEnabled(ragMvmt, false); //Disable rag 

			while (Vector3.Distance(ragMvmt.transform.position, transform.position) > stopDistance)
			{
				//move towards rag 
				vecToTarget = (ragMvmt.transform.position - transform.position);
				transform.position += vecToTarget.normalized * moveDistPerFixedUpdate * vecToTarget.magnitude;
				yield return new WaitForFixedUpdate();
			}
			//once we're really close to rag, 
			ragMvmt.transform.position = transform.position; //Snap rag to us 
			ragMvmt.gameObject.transform.parent = ragParent; //Parent rag to us 

			while (Vector3.Distance(transform.position, targetPos) > stopDistance) //While we aren't super close to our target position, 
			{
				vecToTarget = (targetPos - transform.position); //We should now be moving to the target position 
				transform.position += vecToTarget.normalized * moveDistPerFixedUpdate;  //move towards it 
				yield return new WaitForFixedUpdate(); //wait for the next tick of FixedUpdate 
			}
			//Once we're super close to our target, 
			transform.position = targetPos; //snap to the correct position 
			SetRagEnabled(ragMvmt, true); //re-enable rag 
			ragMvmt.gameObject.transform.parent = originalRagParent; //reset rag's parent back to whatever the hell it was before we touched it 

			//Rag is likely to step off the spider into a trigger, tell them to stop looking for him for a bit 
			SetTriggersLooking(false);
			yield return new WaitForSeconds(.2f);
			SetTriggersLooking(true);
		}

		private void SetRagEnabled(Rag_Movement ragMvmt, bool enabled)
		{
			if (!enabled)
			{
				ragMvmt.useGravity = false;
				ragMvmt.SetVelocity(Vector3.zero); //Make sure rag doesn't float off 
			}
			else
			{
				ragMvmt.gameObject.transform.parent = originalRagParent;
				ragMvmt.useGravity = originalUseGravity;
			}

			ragMvmt.disableControls = !enabled;
		}
		#endregion
		#endregion

	}
}

