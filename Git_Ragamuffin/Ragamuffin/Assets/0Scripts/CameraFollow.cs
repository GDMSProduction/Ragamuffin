using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offsetBase; // base offset of camera
    public Vector3 offsetFOV; // offset for larger view area
    public bool baseView = true;
    public bool FOV = false;


    private void FixedUpdate()
    {
        if (baseView && !FOV) { Base(); };
        if (!baseView && FOV) { fov(); }
    }


    public void Base()
    {
        Vector3 desiredPosition = target.position + offsetBase;
        Vector3 smoothedPositon = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPositon; // Allows camera to follow the player.
    }

    public void fov()
    {
        Vector3 desiredPosition = target.position + offsetFOV;
        Vector3 smoothedPositon = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPositon; // Allows camera to follow the player.
    }

    public void ToggleView()
    {
        if(baseView == true)
        {
            baseView = false;
        }
        else
        {
            baseView = true;
        }
        if (FOV == false)
        {
            FOV = true;
        }
        else
        {
            FOV = false;
        }
    }

}
