using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpiderStuff
{
	public class SpiderTriggerVolume : MonoBehaviour
	{
		[Tooltip("Where should the spider take rag after rag steps in this trigger?")]
		[SerializeField] private Transform targetPosition = null;

		public delegate void Dlg_TriggerEnetered(Rag_Movement rm, Vector3 pos);
		public event Dlg_TriggerEnetered Evt_TriggerEntered;

		private bool lookingForRag = true;

		private void OnTriggerEnter(Collider other)
		{
			if (targetPosition == null) //If we don't know where to send the spider, 
			{
				//Yell at the console and arrest execution 
				Debug.LogError(gameObject.name + ": SpiderTriggerVolume: Target position transform not set! Cannot tell spider controller to move to position!");
				return;
			}

			Rag_Movement rm = other.GetComponent<Rag_Movement>();
			if (rm != null && lookingForRag)
			{
				//Look through our listeners, invoke any that aren't null 
				for (int i = 0; i < Evt_TriggerEntered.GetInvocationList().Length; i++)
				{
					if (Evt_TriggerEntered.GetInvocationList()[i] != null)
					{
						Evt_TriggerEntered.GetInvocationList()[i].DynamicInvoke(other.GetComponent<Rag_Movement>(), targetPosition.position);
					}
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

		public void SetLookingForRag(bool look)
		{
			lookingForRag = look;
		}
	}
}
