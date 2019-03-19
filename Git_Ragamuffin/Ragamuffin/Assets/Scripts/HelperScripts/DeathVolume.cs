using UnityEngine;

//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Robert Bauerle
//               Date: 2/28/2019
//            Purpose: A simple script to call the respawn function for when the player falls off of the map
// Associated Scripts: ReSpawnManager
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

[RequireComponent(typeof(Collider2D))]
public class DeathVolume : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<ReSpawnManager>().ReSpawn();
        }
    }
}
