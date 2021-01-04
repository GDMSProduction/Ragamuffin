using UnityEngine;

public class OnOff2 : MonoBehaviour
{
    public GameObject goOnOff;
    public GameObject otherOnOff;
    public void ObjectOnOff()
    {
        goOnOff.SetActive(false);
        otherOnOff.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ObjectOnOff();
    }
}
