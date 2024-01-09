using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    [SerializeField]
    private Button[] levelButtons;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Level2Unlocked") == 1)
        {
            levelButtons[1].interactable = true;
        }
        if (PlayerPrefs.GetInt("Level3Unlocked") == 1)
        {
            levelButtons[2].interactable = true;
        }
    }

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
