using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class disables the renderers of the object it's attached to and 
/// </summary>
public class RenderDisabler : MonoBehaviour
{
	[SerializeField] private bool scriptEnabled = true;

	[Tooltip("Disable renderers of child objects as well?")]
	[SerializeField] private bool includeChildren = false;

	private Renderer[] rends;

	void Start()
	{
		//this is here so the renderers will ALWAYS be turned off in builds, even if someone forgets to enable the render disabler before building 
#if UNITY_EDITOR
		if(!scriptEnabled)
		{
			enabled = false;
			return;
		}
#endif

		if (includeChildren)
		{
			rends = GetComponentsInChildren<Renderer>();
		}
		else
		{
			rends = GetComponents<Renderer>();
		}

		for (int i = 0; i < rends.Length; i++)
		{
			rends[i].enabled = false;
		}
	}
}
