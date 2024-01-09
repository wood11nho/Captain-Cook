using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void selectLevel1()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void selectLevel2()
    {
        SceneManager.LoadSceneAsync("Level 2");
    }

    public void selectLevel3()
    {
        SceneManager.LoadSceneAsync("Level 3");
    }

    public void backButton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
