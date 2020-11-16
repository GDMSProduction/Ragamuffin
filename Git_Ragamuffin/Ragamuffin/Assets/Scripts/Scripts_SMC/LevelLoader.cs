using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string newScene){
        Time.timeScale = 1;
        SceneManager.LoadScene(newScene);
    }
    public void Exit(){
        Application.Quit();
    }
    public void SetVolume(float num){
        PlayerPrefs.SetFloat("Volume", num);
        Debug.Log(PlayerPrefs.GetFloat("Volume") + " test");
    }
    public void ContinueGame(){
       string temp = PlayerPrefs.GetString("lastlevel");
        if(temp != ""){
            SceneManager.LoadScene(temp);
        }
    }
}
//Boolean Screen.fullScreen that you can set at runtime. False is windowed mode!