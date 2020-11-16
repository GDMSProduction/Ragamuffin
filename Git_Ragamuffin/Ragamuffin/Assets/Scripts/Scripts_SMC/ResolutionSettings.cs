using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionSettings : MonoBehaviour
{
public TMP_Dropdown resolutionDropdown;
Resolution[] resolutions;
string resKey = "ResIndex";
void Start()
 {
    //  int currentResIndex = PlayerPrefs.GetInt(resKey,0);
     resolutions = Screen.resolutions;
     resolutionDropdown.ClearOptions();
     List<string> options = new List<string>();
     int currentResolutionIndex = 0;
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
    Debug.Log("test");
    //Debug.Log(currentResolutionIndex);
     ResDropDownValue();
 }
 public void ResDropDownValue(){
     Debug.Log("test2");
    //  int currentResolutionIndex = 0;
    
    Debug.Log(resolutionDropdown.value);
    UpdateResolution(resolutions[resolutionDropdown.value]);
 }
 private void UpdateResolution(Resolution res){
     Screen.SetResolution(res.width, res.height, Screen.fullScreen);
     PlayerPrefs.SetInt(resKey,resolutionDropdown.value);
 }
}
