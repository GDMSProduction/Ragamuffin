using UnityEngine;
using TMPro;

public class Safe : MonoBehaviour
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
    public Transform target1;// Where you want Rag to teleport inside object.
    public Transform target2;// Where you want Rag to teleport outside object.
    public GameObject child; // Hook up Rag to this variable.
    private bool binary;


    private void Start()
    {
        onOff = GetComponent<OnOff>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            if (safeCode == null)
            {
                return;
            }
            if (safeCode)
            {
                onOff.ObjectOnOff();
                binary = !binary;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            if (Input.GetKeyDown(KeyCode.R) && child.GetComponent<Rag>().isHiding == true)
            {
                child.transform.position = target2.position;
                transform.position = transform.position;
                child.transform.parent = null;
                child.GetComponent<Rag>().isHiding = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!binary)
        {
            return;
        }
        if (binary)
        {
            onOff.ObjectOnOff();
            safeCode.text = "";
            binary = !binary;
        }
    }
    private void LockingPlayerMovement() // Locks player movement.
    {
            child.transform.position = target1.position;
            child.transform.SetParent(target1.transform);
            child.GetComponent<Rag>().isHiding = true;
    }
    public void SafeFunction(string code)
    {
        if (code == password)
        {
            LockingPlayerMovement();
            onOff.ObjectOnOff();
            safeCode.text = "";
            binary = !binary;
        }
    }
}
