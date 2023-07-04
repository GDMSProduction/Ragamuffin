using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{   /*
     void OnGUI()  //This is used to reset playerprefs. When testing saving function turn this function back on.
         {
             //Delete all of the PlayerPrefs settings by pressing this Button
             if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
             {
                 PlayerPrefs.DeleteAll();
             }
    }*/
    public void DeletePlayerPrefs(){
        PlayerPrefs.DeleteAll();
    }
    public void LoadLevel(string newScene){
        Time.timeScale = 1; // This makes sure timescale is set to 1 when going to a new scene in case player is in the menu screen where timescale can be = to 0;
        SceneManager.LoadScene(newScene);
    }
    public void Exit(){
        Application.Quit(); // Closes out the game / executable.
    }
    public void SetVolume(float num){//music volume
        PlayerPrefs.SetFloat("Volume", num);
        //Debug.Log(PlayerPrefs.GetFloat("Volume") + " test");
    }
     public void SetSoundEffectsVolume(float num){ // sound effects volume
        PlayerPrefs.SetFloat("Sound", num);
        //Debug.Log(PlayerPrefs.GetFloat("Sound") + " test");
    }
    public void ContinueGame(){ // Continue from last level played. Default is first level.
       string temp = PlayerPrefs.GetString("lastlevel");
        if(temp != ""){
            SceneManager.LoadScene(temp);
        }
    }
}
//Boolean Screen.fullScreen that you can set at runtime. False is windowed mode!