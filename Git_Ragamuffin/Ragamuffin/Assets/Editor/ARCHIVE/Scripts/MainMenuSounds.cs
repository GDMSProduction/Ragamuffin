using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainMenuSounds : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    soundAffect sound;
    public void OnPointerEnter(PointerEventData eventData)
    {
        sound.PlaySound("buttonSound");
    }
}
