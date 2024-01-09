using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //PlayerPrefs.SetInt("Level2Locked", 1);
        //PlayerPrefs.SetInt("Level3Locked", 1);
    }

    public void playGame()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void quitGame()
    {
        Application.Quit();
    }


}
