using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Robert Bauerle
//               Date: 7/9/2019
//            Purpose: A simple script used to create makeshift semi-solid colliders for 2D space
// Associated Scripts: None
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
public class ColliderEnabler : MonoBehaviour
{
    [SerializeField] bool startEnabled = false;
    List<Collider> colliders = new List<Collider>();
    [SerializeField] GameObject[] colliderObjects;

	void Start ()
    {
        for (int i = 0; i < colliderObjects.Length; i++)
        {
            foreach (Collider col in colliderObjects[i].GetComponents<Collider>())
            {
                colliders.Add(col);
            }
        }

        SetColliders(startEnabled);
	}


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            SetColliders(!startEnabled);
        }
    }

    void SetColliders(bool enabled)
    {
        if (colliders == null)
            return;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = enabled;
        }
    }
}
