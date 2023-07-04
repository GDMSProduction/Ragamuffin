using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Targets UI element in Scene.
    public TMP_Dropdown qualityDropdown; // Targets UI element in Scene.
    Resolution[] resolutions; // Creates array for screen resolutions.
    // the 3 keys below are used for playerprefs.
    string resKey = "ResIndex";
    string qualityKey = "QualityIndex";
    string firstTimeKey = "First";
    public Toggle tog; // UI element for Toggling the FullScreen mode on and off.

    void Start()
    {
     //Debug.Log(Screen.currentResolution); Debug.Log(resolutions.Length);Debug.Log(names[0]); 
     tog.isOn = Screen.fullScreen;
     resolutions = Screen.resolutions;
     resolutionDropdown.ClearOptions();
     List<string> options = new List<string>();
     int currentResolutionIndex = 0;
     string[] names = QualitySettings.names;
     List<string> optionz = new List<string>();
     qualityDropdown.ClearOptions();

        for(int i=0; i <names.Length; i++) // Creates drop down for Quality settings.
        {
            string option1 = names[i];
            optionz.Add(option1);
        }
        for(int i=0; i <resolutions.Length; i++) // Creates drop down for Resolution settings.
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
                if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)

                    {
                        currentResolutionIndex = i;
                    }
        }

        resolutionDropdown.AddOptions(options);
        qualityDropdown.AddOptions(optionz);

            if (PlayerPrefs.GetInt(firstTimeKey) == 0) // Checks for first time loading Options Scene.
            {
                PlayerPrefs.SetInt(resKey, resolutions.Length - 1);
                PlayerPrefs.SetInt(qualityKey, names.Length - 1);
                PlayerPrefs.SetInt(firstTimeKey, 1);
            }
        // The 4 lines below update the drop down boxes to reflect the current settings.
        resolutionDropdown.value = PlayerPrefs.GetInt(resKey);
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = PlayerPrefs.GetInt(qualityKey);
        qualityDropdown.RefreshShownValue();
    }

    public void QualityDropDownValue(int QualityIndex) // Gets called when user selects an option.
    {
        QualitySettings.SetQualityLevel(QualityIndex);
        PlayerPrefs.SetInt(qualityKey,qualityDropdown.value);
    }
    public void ResDropDownValue() // Gets called when user selects an option.
    {
        UpdateResolution(resolutions[resolutionDropdown.value]);
    }
    private void UpdateResolution(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt(resKey,resolutionDropdown.value);
    }
    public void FullScreenOnOff() // Gets called when user selects an option.
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
