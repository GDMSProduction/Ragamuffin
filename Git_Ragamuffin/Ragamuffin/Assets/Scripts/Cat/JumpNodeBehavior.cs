//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNodeBehavior : NodeBehaviorBase
{
    #region Variables
    [Header("Cat's horizontal movement during jump")]
    [SerializeField] private float horizontalDistance;
    [Header("Height of cat's jump")]
    [SerializeField] private float verticalRepositionHeight;        // The height the cat needs to jump to make it to the specified platform
    #endregion

    #region Accessors
    public float GetHorizontalDistance()
	{
		return horizontalDistance;
	}
    public float GetverticalRepositionHeight()
	{
		return verticalRepositionHeight;
	}
    #endregion
}