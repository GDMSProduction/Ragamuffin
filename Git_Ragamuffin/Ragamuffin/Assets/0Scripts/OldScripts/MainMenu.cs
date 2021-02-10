using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("=============================================================================================================================")]
    [Header("Warning window")]
    [SerializeField] GameObject warningWindow;
    [Tooltip("The Warning Window Title Text Object")]
    [SerializeField] TMP_Text text;
    [SerializeField] Button yesButton, noButton;
    [Header("=============================================================================================================================")]

    public int savedSceneIndex;

    public void OptionsMenu()
    {
        LoadScene("OptionsMenu");
    }

    public void ContinueGame()
    {
        //Continues the game when passed a specific scene based on a passed index.
        LoadScene(savedSceneIndex);
    }

    public void QuitGame(string quitMessageTitle)
    {
        //Make sure player meant to do this first here
        EnableWarningWindow(quitMessageTitle, ExitApplication);
    }

    /// <summary>
    /// WARNING: THIS FUNCTION IMMEDIATELY EXITS THE APPLICATION!!
    /// </summary>
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        //Ends the program.
        Application.Quit();
    }


    //TEMPORARY - WILL BE CHANGED TO ASYNC LATER
    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadScene(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void EnableWarningWindow(string windowTitle, UnityEngine.Events.UnityAction yesClicked, UnityEngine.Events.UnityAction noClicked = null, string yesButtonOverride = "Yes", string noButtonOverride = "No")
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        text.text = windowTitle;
        yesButton.onClick.AddListener(yesClicked);
        yesButton.onClick.AddListener(WarningWindowClose);
        yesButton.GetComponentInChildren<TMP_Text>().text = yesButtonOverride;
        if (noClicked != null)
            noButton.onClick.AddListener(noClicked);
        noButton.onClick.AddListener(WarningWindowClose);
        noButton.GetComponentInChildren<TMP_Text>().text = noButtonOverride;
        warningWindow.SetActive(true);
    }

    public void WarningWindowClose()
    {
        warningWindow.SetActive(false);
    }

    
}
