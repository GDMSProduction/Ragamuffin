using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPositon = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPositon; // Allows camera to follow the player.
    }
}
