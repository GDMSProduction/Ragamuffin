using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNodeBehavior : MonoBehaviour
{
    [Header("Height of cat's jump")]
    [SerializeField] private float verticalRepositionHeight;                   // The height the cat needs to jump to make it to the specified platform

    public float GetverticalRepositionHeight() { return verticalRepositionHeight; }
}