using System.Collections;
using UnityEngine;


//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Robert Bauerle
//               Date: 5/16/2019
//            Purpose: A simple script used to elevate the ladder on the fire truck
// Associated Scripts: None
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
public class FireTruck_Button : MonoBehaviour
{
    [Header("Ladder transform")]
    [Tooltip("Transform of the ladder object that will be rotated")]
    [SerializeField] Transform ladder;
    [Tooltip("Amount of time it will take for the ladder to reach it's end rotation, in seconds.")]
    [Range(0.1f, 200)]
    [SerializeField] float rotationTime = 2;
    [Range(-360f, 360f)]
    [Tooltip("Angle at which the ladder will be at when it ends rotation")]
    [SerializeField] float endRotationAngle = 0;

    bool isHere = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        isHere = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        isHere = false;
    }

    private void Update()
    {
        if (isHere && Input.GetKeyUp(KeyCode.E) && !ladderUp)
            StartCoroutine(LadderUp());
    }

    bool ladderUp = false;
    private IEnumerator LadderUp()
    {
        ladderUp = true;
        float t = 0;
        Quaternion baseRot = ladder.localRotation, endRot = baseRot * Quaternion.AngleAxis(endRotationAngle - ladder.localRotation.eulerAngles.z, Vector3.forward);

        do
        {
            t += Time.deltaTime / rotationTime;
            ladder.localRotation = Quaternion.Lerp(baseRot, endRot, t);
            yield return new WaitForEndOfFrame();
        } while (t < 1);
        
    }
}
