using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HidingBox : MonoBehaviour
{
    /// <summary>
    /// The canvas SafeCodeInput => (InputField (TMP)) has a character limit section.
    /// This controls how many characters will be in the input field.
    /// So the password (which is the variable called password) needs to be 
    /// exactly how many characters the character size limit is.
    /// Please update password in the inspector of the safe gameobject with this script on it.
    /// That way it keeps the variable dynamic and have potential for multiple passwords with
    /// only using one variable.
    /// </summary>
    
    public string password;  // Change password in inspector of gameObject.
    private OnOff onOff;
    public TMP_InputField safeCode;
    public Transform target; // Where you want Rag to teleport.
    public GameObject child; // Hook up Rag to this variable.

    /// <summary>
    /// The bools safe or box will be checked on the inspector if the gameobject 
    /// that this script is a box or safe for increased functionality.
    /// If a plant keep both safe and box unchecked.
    /// Only one bool should be active at a time or none at all.
    /// </summary>

    public bool safe;
    public bool box;

    private bool trueFalse = false; // is used solely to control the state of the safe.

    private void Start()
    {
        onOff = GetComponent<OnOff>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == ("Player"))
        {
            if (safe)
            {
                onOff.ObjectOnOff();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            if (safe)
            {
                if (trueFalse)
                {
                    LockingPlayerMovement();
                    trueFalse = !trueFalse;
                }     
            }
            if (box)
            {
                LockingPlayerMovement();
            }
            else { LockingPlayerMovement(); }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (safe)
        {
            safeCode.text = "";
            onOff.ObjectOnOff();
        }
    }

    private void LockingPlayerMovement() // Locks player movement.
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            child.transform.position = target.position;
            child.transform.SetParent(target.transform);
            child.GetComponent<Rag>().isHiding = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && child.GetComponent<Rag>().isHiding == true)
        {
            transform.position = transform.position;
            child.transform.parent = null;
            child.GetComponent<Rag>().isHiding = false;
        }
    }
    public void SafeFunction(string code)
    {
        if (code == "password")
        {
            LockingPlayerMovement();
            safeCode.text = "";
            onOff.ObjectOnOff();
            trueFalse = !trueFalse;
        }
    }
}

