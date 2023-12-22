using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject servingTable;

    [SerializeField]
    private GameObject activeRecipesUI; 
    
    [SerializeField]
    private GameObject pickupUI;


    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI timeLeftText;

    [SerializeField]
    private RawImage[] strikesImages;

    [SerializeField]
    private GameObject recipeGenerator;

    [SerializeField]
    private float gameDuration = 300.0f;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private TextMeshProUGUI cookingScoreText;

    [SerializeField]
    private TextMeshProUGUI survivedLevelText;

    [SerializeField]
    private TextMeshProUGUI strikesPenalizationText;

    [SerializeField]
    private TextMeshProUGUI noLeftoversBonusText;

    [SerializeField]
    private int survivedLevelScore = 300;

    [SerializeField]
    private int noLeftoversBonus = 150;

    [SerializeField]
    private TextMeshProUGUI totalScoreText;

    [SerializeField]
    private GameObject musicPlayer;

    [SerializeField]
    private AudioClip winMusic;

    [SerializeField]
    private AudioClip loseMusic;

    private float timeElapsed = 0.0f;

    private int score;

    private int strikes;

    private int strikesScore;

    private int totalScore;

    private bool gameStarted = false;

    private bool gameWon = false;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        strikes = 0;
        servingTable.layer = LayerMask.NameToLayer("IgnoreRaycast");
    }

    // Update is called once per frame
    void Update()
    {
        if(strikes >= 3)
        {
            Debug.Log("Game Over!");
            StopGame();
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= gameDuration)
            {
                Debug.Log("Game Over!");
                gameWon = true;
                StopGame();
            }
            else
            {
                if (gameStarted)
                {
                    scoreText.text = "Score: " + score;
                    timeLeftText.text = "Time left: " + (int)(gameDuration - timeElapsed);
                }
                
            }
        }
    }

    public void StartGame()
    {
        servingTable.layer = LayerMask.NameToLayer("Interactable");
        scoreText.gameObject.SetActive(true);
        timeLeftText.gameObject.SetActive(true);
        gameStarted = true;
        recipeGenerator.GetComponent<RecipeGenerator>().StartRecipeGenerator();
        timeElapsed = 0.0f;
    }

    public void StopGame()
    {

        //disable player movement
        Destroy(player.GetComponent<PlayerLook>());
        Destroy(player.GetComponent<PlayerMotor>());
        Destroy(player.GetComponent<ItemPickup>());
        player.GetComponent<CharacterController>().enabled = false;

        activeRecipesUI.SetActive(false);
        pickupUI.SetActive(false);

        for(int i = 0; i < strikesImages.Length; i++)
        {
            strikesImages[i].gameObject.SetActive(false);
        }
        scoreText.gameObject.SetActive(false);
        timeLeftText.gameObject.SetActive(false);
        recipeGenerator.GetComponent<RecipeGenerator>().StopRecipeGenerator();
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        strikesScore = strikes * 50;

        if (!gameWon)
        {
            //musicPlayer.GetComponent<AudioManager>().changeToLoseMusic();
            musicPlayer.GetComponent<AudioManager>().changeBackgroundMusic(loseMusic);
            strikesPenalizationText.text = "All";
            survivedLevelScore = 0;
        }
        else
        {
            //musicPlayer.GetComponent<AudioManager>().changeToWinMusic();
            musicPlayer.GetComponent<AudioManager>().changeBackgroundMusic(winMusic);
            gameOverText.text = "You Survived!";
            strikesPenalizationText.text = "-" + strikes.ToString();
        }

        if(LeftoversExist())
        {
            noLeftoversBonus = 0;
        }

        totalScore = score + survivedLevelScore + noLeftoversBonus  - strikesScore;
        cookingScoreText.text = score.ToString();
        survivedLevelText.text = survivedLevelScore.ToString();
        noLeftoversBonusText.text = noLeftoversBonus.ToString();
        totalScoreText.text = totalScore.ToString();

        gameOverPanel.SetActive(true);

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool LeftoversExist()
    {
        if(GameObject.FindGameObjectsWithTag("Ingredient").Length > 1)
        {
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("Ingredient").Length; i++)
            {
                Debug.Log(GameObject.FindGameObjectsWithTag("Ingredient")[i].name);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public int CalculateRecipeScore(Recipe recipe)
    {
        int recipeScore = 10 * recipe.GetIngredientNames().Count + 200 + (int)recipe.GetExpirationTime();
        return recipeScore;
    }

    public float GetGameDuration()
    {
        return gameDuration;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public void AddStrike()
    {
        if(strikes < 3)
        {
            strikes++;
            strikesImages[strikes - 1].gameObject.SetActive(true);
        }
        
    }

    public bool GetGameWon()
    {
        return gameWon;
    }

    public void SetGameWon(bool gameWon)
    {
        this.gameWon = gameWon;
    }

}
