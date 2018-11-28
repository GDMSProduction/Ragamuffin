//--------------------------------------------------------------------------------------------------------------------------------------------------\\
//             Author: Cory Moultrie
//               Date: 
//            Purpose: 
// Associated Scripts: 
//--------------------------------------------------------------------------------------------------------------------------------------------------\\

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviorBase : MonoBehaviour
{
    #region Variables
    [Header("Which side the cat may use the node from")]
    [SerializeField] private byte activationSide;                   // 1 - Left, 2 - Right, 3 - Both

    byte currentActivationSide;
    #endregion

    #region Initialation
    private void Awake() { currentActivationSide = activationSide; }
    #endregion

    #region Accessors
    public byte GetActivationSide() { return currentActivationSide; }
    #endregion
}
