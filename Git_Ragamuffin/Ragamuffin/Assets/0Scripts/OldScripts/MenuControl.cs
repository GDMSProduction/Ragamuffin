using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MenuControl : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
        if(Input.GetButtonDown("Cancel"))
        {
            PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("OptionsMenu");
        }
	}
}
