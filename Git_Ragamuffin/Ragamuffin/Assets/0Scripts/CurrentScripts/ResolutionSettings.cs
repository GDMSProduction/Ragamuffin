using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionSettings : MonoBehaviour
{
public TMP_Dropdown resolutionDropdown;
Resolution[] resolutions;
public TMP_Dropdown qualityDropdown;
string resKey = "ResIndex";
string qualityKey = "QualityIndex";
public static float brightness = 1f;
public Toggle tog;
void Start()
 {
        
     tog.isOn = Screen.fullScreen;
     resolutions = Screen.resolutions;
     resolutionDropdown.ClearOptions();
     List<string> options = new List<string>();
     int currentResolutionIndex = 0;
     int currentQualityIndex = 0;
    string[] names = QualitySettings.names;
    Debug.Log(names[0]);
    List<string> optionz = new List<string>();
    qualityDropdown.ClearOptions();
 for(int i=0; i <names.Length; i++)
     {
         string option1 = names[i];
         optionz.Add(option1);
     }
     for(int i=0; i <resolutions.Length; i++)
     {
         string option = resolutions[i].width + " x " + resolutions[i].height;
         options.Add(option);
         if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
         {
             currentResolutionIndex = i;
         }
     }
     resolutionDropdown.AddOptions(options);
     resolutionDropdown.value = PlayerPrefs.GetInt(resKey);
     resolutionDropdown.RefreshShownValue();
     //ResDropDownValue();
     qualityDropdown.AddOptions(optionz);
     qualityDropdown.value = PlayerPrefs.GetInt(qualityKey);
     qualityDropdown.RefreshShownValue();
 }

public void QualityDropDownValue(int QualityIndex){
    QualitySettings.SetQualityLevel(QualityIndex);
    PlayerPrefs.SetInt(qualityKey,qualityDropdown.value);
 }

 public void ResDropDownValue(){
    UpdateResolution(resolutions[resolutionDropdown.value]);
 }
 private void UpdateResolution(Resolution res){
     Screen.SetResolution(res.width, res.height, Screen.fullScreen);
     PlayerPrefs.SetInt(resKey,resolutionDropdown.value);
 }
 public void FullScreenOnOff(){
     Screen.fullScreen = !Screen.fullScreen;
 }
 
}
