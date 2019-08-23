//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//Changelog:
// 08/22/2019 Colby Peck: Replaced methods for getting horizontalDistance and verticalPositionHeight with properties 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNodeBehavior : NodeBehaviorBase
{
	#region Variables
	[Header("Cat's horizontal movement during jump")]
	[SerializeField] private float horizontalDistance;
	public float HorizontalDistance { get { return horizontalDistance; } }

	[Header("Height of cat's jump")]
	[SerializeField] private float verticalRepositionHeight;        // The height the cat needs to jump to make it to the specified platform
	public float VerticalRepositionHeight { get { return verticalRepositionHeight; } }
	#endregion
}