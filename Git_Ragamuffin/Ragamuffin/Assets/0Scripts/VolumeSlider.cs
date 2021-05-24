using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundEffectsSlider;
    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Volume"); // Slider for game music.
        soundEffectsSlider.value = PlayerPrefs.GetFloat("Sound"); // Slider for sound effects.
    }
}
