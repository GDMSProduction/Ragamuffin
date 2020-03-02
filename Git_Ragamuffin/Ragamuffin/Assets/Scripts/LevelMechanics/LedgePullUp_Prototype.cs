//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: 11/20/18
//            Purpose: A super-quick prototype of climbing up ledges for the "Proof Of Concept" build review.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using UnityEngine;

public class LedgePullUp_Prototype : MonoBehaviour
{
	Collider col;
	[SerializeField] Transform spawnLoc;

	void Awake()
	{
		col = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player") && collision.transform.position.z != spawnLoc.transform.position.z)
		{
            
			collision.gameObject.transform.position = spawnLoc.position;
            collision.GetComponent<RB_Grapple>().EndGrapple();
		}
	}
}
