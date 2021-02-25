using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
