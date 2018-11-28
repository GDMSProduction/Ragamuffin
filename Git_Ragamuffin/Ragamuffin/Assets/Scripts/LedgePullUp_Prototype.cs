//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Dylan D. Rader
//               Date: 11/20/18
//            Purpose: A super-quick prototype of climbing up ledges for the "Proof Of Concept" build review.
// Associated Scripts: None.
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using UnityEngine;

public class LedgePullUp_Prototype : MonoBehaviour
{
	Collider2D col;
	[SerializeField] Transform spawnLoc;

	void Awake()
	{
		col = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.transform.position = spawnLoc.position;
		}
	}
}
