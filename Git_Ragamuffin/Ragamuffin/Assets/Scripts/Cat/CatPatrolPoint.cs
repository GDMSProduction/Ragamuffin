//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Colby Peck
//               Date: 10/07/2019 
//            Purpose: Make a checkpoint for the cat's patrol system, make the checkpoints visually intuitive and simple for the level designers to use 
// Associated Scripts: CatManager, CatState (patrol state)
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog: 
// 10/07/2019 ColbyPeck: Made the script 
// 10/07/2019 ColbyPeck: Added functionality for pointing to next/previous point
// 10/07/2019 ColbyPeck: Added a bunch of editor code to make debugging/implementation easier 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif

public class CatPatrolPoint : MonoBehaviour
{
	public CatPatrolPoint nextPoint;
	public Vector3 Position { get { return transform.position; } }
	[HideInInspector] public CatPatrolPoint previousPoint;

	//All this editor code is built to help make the checkpoints easier to use 
	//It's in bad need of documentation, but it can be safely disabled if it's causing problems. You'll need to set all the previous points manually (so remove the [HideInInspector] and the arrows pointing to the other checkpoints will be useless, but everything should work just fine 
	#region Editor Shenanigans

#if UNITY_EDITOR
	[SerializeField] private bool debugMode = false;
	[SerializeField] private GameObject forwardArrowPivot, backwardsArrowPivot;

	void ToggleMeshes(PlayModeStateChange change)
	{
		bool enable = true;
		switch (change)
		{
			case PlayModeStateChange.EnteredEditMode:
				enable = true;
				break;

			case PlayModeStateChange.EnteredPlayMode:
				if(!debugMode)
				{
					enable = false;
				}
				break;
		}

		Renderer[] rends = GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < rends.Length; i++)
		{
			rends[i].enabled = enable;
		}
	}

	private void Awake()
	{
		EditorApplication.playModeStateChanged -= ToggleMeshes;
		EditorApplication.playModeStateChanged += ToggleMeshes;
	}
	private void OnDisable()
	{
		EditorApplication.playModeStateChanged -= ToggleMeshes;
	}

	private void Update()
	{
		if(EditorApplication.isPlaying)
		{
			return;
		}

		if (nextPoint != null)
		{
			if (nextPoint == this)
			{
				Debug.LogWarning("CatPatrolPoint: A point's next point cannot be itself!");
				nextPoint = null;
				return;
			}
			if (nextPoint.previousPoint == null)
			{
				nextPoint.previousPoint = this;
			}
			else if (nextPoint.previousPoint != this)
			{
				nextPoint.previousPoint.nextPoint = null;
				nextPoint.previousPoint = this;
			}

			if (forwardArrowPivot)
			{
				forwardArrowPivot.SetActive(true);
				forwardArrowPivot.transform.LookAt(nextPoint.transform);
			}
		}
		else
		{
			forwardArrowPivot.SetActive(false);
		}


		if (previousPoint != null)
		{
			if (previousPoint == this)
			{
				previousPoint = null;
				return;
			}
			if (previousPoint.nextPoint != this)
			{
				previousPoint = null;
				return;
			}

			if (backwardsArrowPivot)
			{
				backwardsArrowPivot.SetActive(true);
				backwardsArrowPivot.transform.LookAt(previousPoint.transform);
			}
		}
		else
		{
			backwardsArrowPivot.SetActive(false);
		}

	}
#endif
	#endregion
}

