using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpiderStuff
{
	public class SpiderTriggerVolume : MonoBehaviour
	{
		[Tooltip(
			"What is the index of the position you want the spider to move to when the player enters this trigger?" +
			"\n-1 indicates an invalid index." +
			"\nThe array holding the positions can be found in the spider controller."
			)]
		//[SerializeField] private int positionIndexToMoveSpiderTo = -1;
		[SerializeField] private Transform targetPosition = null;

		public delegate void Dlg_TriggerEnetered(Rag_Movement rm, Vector3 pos);
		public event Dlg_TriggerEnetered Evt_TriggerEntered;

		private bool lookingForRag = true;

		private void OnTriggerEnter(Collider other)
		{
			if (targetPosition == null)
			{
				Debug.LogError(gameObject.name + ": SpidertriggerVolume: Target position transform not set! Cannot tell spider controller to move to position!");
				enabled = false;
				return;
			}

			Rag_Movement rm = other.GetComponent<Rag_Movement>();
			if (rm != null && lookingForRag)
			{
				for (int i = 0; i < Evt_TriggerEntered.GetInvocationList().Length; i++)
				{
					Evt_TriggerEntered.GetInvocationList()[i].DynamicInvoke(other.GetComponent<Rag_Movement>(), targetPosition.position);
					Debug.Log("Triggered event, sent " + targetPosition.position);
				}
				//Evt_TriggerEntered.Invoke(other.GetComponent<Rag_Movement>(), targetPosition.position);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			Rag_Movement rm = other.GetComponent<Rag_Movement>();
			if (rm != null)
			{
				lookingForRag = true;
			}
		}

		public void StopLookingForRag()
		{
			lookingForRag = false;
		}
	}
}
