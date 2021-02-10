using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class OptionsSettings : MonoBehaviour {

    public AudioMixer volumeMixer;

    public void BackButton()
    {
        string sceneName = PlayerPrefs.GetString("lastScene");
        SceneManager.LoadScene(sceneName);
    }
    public void SetMasterVolume(float volume)
    {
        volumeMixer.SetFloat("Master", volume);
    }
    public void SetBrightness(float brightness)
    {

    }
    public void SetFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
