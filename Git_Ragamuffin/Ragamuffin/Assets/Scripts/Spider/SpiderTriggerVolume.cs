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
		[SerializeField] private int positionIndexToMoveSpiderTo = -1;

		public delegate void Dlg_TriggerEnetered(Rag_Movement rm, int pos);
		public event Dlg_TriggerEnetered Evt_TriggerEntered;

		public bool lookingForRag = true;

		private void OnTriggerEnter(Collider other)
		{
			if (positionIndexToMoveSpiderTo == -1)
			{
				Debug.LogError(gameObject.name + ": SpidertriggerVolume: target position index not set! Cannot tell spider controller to move to a target position!");
				enabled = false;
				return;
			}

			Rag_Movement rm = other.GetComponent<Rag_Movement>();
			if (rm != null && lookingForRag)
			{
				for (int i = 0; i < Evt_TriggerEntered.GetInvocationList().Length; i++)
				{
					Evt_TriggerEntered.GetInvocationList()[i].DynamicInvoke(rm, positionIndexToMoveSpiderTo);
				}
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
	}
}
