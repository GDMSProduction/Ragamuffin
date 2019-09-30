using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public int savedSceneIndex;

	public void StartGame()
    {
        //This will load the scene directly after the current scene. 
        //To add a scene to the scene manager or change the current order
        //go to FILE -> BUILD SETTINGS and modify the scenes how you see fit. 
        //The main menu should always be index 0.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void OptionsMenu()
    {
        PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("OptionsMenu");
    }
    public void ContinueGame()
    {
        //Continues the game when passed a specific scene based on a passed index.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + savedSceneIndex);
    }
    public void QuitGame()
    {
        //Ends the program. 
        Application.Quit();
    }

}
