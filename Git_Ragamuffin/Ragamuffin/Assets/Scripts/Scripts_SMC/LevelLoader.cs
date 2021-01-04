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
    public void SetVolume(float num){//music volume
        PlayerPrefs.SetFloat("Volume", num);
        Debug.Log(PlayerPrefs.GetFloat("Volume") + " test");
    }
     public void SetSoundEffectsVolume(float num){
        PlayerPrefs.SetFloat("Sound", num);
        Debug.Log(PlayerPrefs.GetFloat("Sound") + " test");
    }
    public void ContinueGame(){
       string temp = PlayerPrefs.GetString("lastlevel");
        if(temp != ""){
            SceneManager.LoadScene(temp);
        }
    }
}
//Boolean Screen.fullScreen that you can set at runtime. False is windowed mode!