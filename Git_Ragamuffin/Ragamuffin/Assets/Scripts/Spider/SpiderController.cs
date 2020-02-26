using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderStuff
{
	public class SpiderController : MonoBehaviour
	{
		[SerializeField] private SpiderTriggerVolume[] triggerVolumes = null;
		[SerializeField] private Transform[] targetTransforms = null; //MOVE THIS TO THE TRIGGER!!!!!
		[Tooltip("Rag is attached to this transform while moving and detached when done")]
		[SerializeField] private Transform ragParent = null; 

		[SerializeField] private Transform meshParent = null;
		[SerializeField] private float moveDistPerSecond = .3f;

		private float moveDistPerFixedUpdate;
		private int numPositions, targPositionIndex;

		private float stopDistance = .1f;
		#region Init/De-Init

		private void Start()
		{
			if(ReferencesAreSet() != true) //if any of our references are null, 
			{
				return; //trying to continue will throw NREs, get out of here! 
			}

			numPositions = targetTransforms.Length;
			moveDistPerFixedUpdate = 1 / 50 * moveDistPerSecond;
		}


		private void OnEnable()
		{
			StopListenening();
			StartListening();
		}

		private void OnDisable()
		{
			StopListenening();
		}
		#endregion

		#region Helper methods
		private bool ReferencesAreSet()
		{
			if (triggerVolumes == null || triggerVolumes.Length == 0)
			{
				Debug.LogWarning(preErrorHeader() + "SpiderController trigger volume not set! looking for one!");
				triggerVolumes = GetComponentsInChildren<SpiderTriggerVolume>();
			}

			if (triggerVolumes == null)
			{
				Debug.LogError(preErrorHeader() + "No trigger volume found on root object or in children!");
				enabled = false;
			}

			if (targetTransforms == null || targetTransforms.Length == 0)
			{
				Debug.LogError(preErrorHeader() + "Target positions null or empty!");
				enabled = false;
			}

			if (meshParent == null)
			{
				Debug.LogError(preErrorHeader() + "meshParent reference not set!");
				enabled = false;
			}

			if (ragParent == null)
			{
				Debug.LogError(preErrorHeader() + "Rag parent transform reference not set!");
				enabled = false;
			}
			return enabled;
		}

		private void TriggerListener(Rag_Movement rm, int posIndex)
		{
			StopAllCoroutines();
			targPositionIndex = posIndex;

			StartCoroutine(rut_MoveToTargPos(rm));
		}

		private string preErrorHeader()
		{
			return gameObject.name + ".SpiderController: ";
		}

		private void StopListenening()
		{
			if (triggerVolumes != null)
			{
				for (int i = 0; i < triggerVolumes.Length; i++)
				{
					triggerVolumes[i].Evt_TriggerEntered -= TriggerListener;
				}
			}
		}

		private void StartListening()
		{
			if (triggerVolumes != null)
			{
				for (int i = 0; i < triggerVolumes.Length; i++)
				{
					triggerVolumes[i].Evt_TriggerEntered += TriggerListener;
				}
			}
		}
		#endregion

		#region Coroutines

		private IEnumerator rut_MoveToTargPos(Rag_Movement ragMvmt)
		{
			Transform originalRagParent = ragMvmt.gameObject.transform.parent;
			ragMvmt.gameObject.transform.parent = ragParent;
			Vector3 moveDir = (targetTransforms[targPositionIndex].position - transform.position).normalized;

			while (Vector3.Distance(transform.position, targetTransforms[targPositionIndex].position) > stopDistance)
			{
				transform.position += moveDir * moveDistPerFixedUpdate;
				yield return new WaitForFixedUpdate();
			}
			transform.position = targetTransforms[targPositionIndex].position;

			ragMvmt.gameObject.transform.parent = originalRagParent;
		}

		#endregion



	}
}

