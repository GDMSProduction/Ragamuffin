using UnityEngine;

public class OnOff : MonoBehaviour
{
bool objectOnOff = false;
public GameObject uiOnOff;
    public void ObjectOnOff(){
        objectOnOff = !objectOnOff;
        uiOnOff.SetActive(objectOnOff);
    }
}
