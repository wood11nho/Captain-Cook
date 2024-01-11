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

    [SerializeField]
    private AudioSource winAudioSource;

    [SerializeField]
    private AudioSource loseAudioSource;

    [SerializeField]
    private float targetDensity = 0.2f;

    [SerializeField]
    private float duration = 10f;

    [SerializeField]
    private GameObject steakGeneratorObject;

    [SerializeField]
    private GameObject sausageGeneratorObject;

    [SerializeField]
    private GameObject salmonGeneratorObject;

    private float timeElapsed = 0.0f;

    private int score;

    private int strikes;

    private int strikesScore;

    private int totalScore;

    private bool gameStarted = false;

    private bool gameWon = false;

    private bool gameOver = false;

    private KeyCode[] cheatCodeSteak = { KeyCode.S, KeyCode.T, KeyCode.E, KeyCode.A, KeyCode.K };

    private int currentIndexOfSteakCheatCode = 0;

    private KeyCode[] cheatCodeSausage = { KeyCode.S, KeyCode.A, KeyCode.U, KeyCode.S, KeyCode.A, KeyCode.G, KeyCode.E };

    private int currentIndexOfSausageCheatCode = 0;

    private KeyCode[] cheatCodeSalmon = { KeyCode.S, KeyCode.A, KeyCode.L, KeyCode.M, KeyCode.O, KeyCode.N };

    private int currentIndexOfSalmonCheatCode = 0;

    private KeyCode[] cheatMaxPlayerSpeed = {KeyCode.M, KeyCode.A, KeyCode.X, KeyCode.S, KeyCode.P, KeyCode.E, KeyCode.E, KeyCode.D };

    private int currentIndexOfMaxPlayerSpeedCheatCode = 0;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        strikes = 0;
        servingTable.layer = LayerMask.NameToLayer("IgnoreRaycast");

        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            Debug.Log("Level 3");
            StartCoroutine(AddFog());
        }
    }

    IEnumerator AddFog()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 60));
            StartCoroutine(FadeFog(true));

            float randomTimeForFog = Random.Range(30, 60);
            yield return new WaitForSeconds(randomTimeForFog);

            StartCoroutine(FadeFog(false));
            yield return new WaitForSeconds(Random.Range(30, 60));
        }
    }

    IEnumerator FadeFog(bool fadeIn)
    {
        float targetDensityCalculator = fadeIn ? this.targetDensity : 0f;
        float currentDensity = RenderSettings.fogDensity;

        float elapsedTime = 0f;

        Debug.Log("Starting to fade fog");

        while (elapsedTime < duration)
        {
            // Gradually change fog density over time
            RenderSettings.fogDensity = Mathf.Lerp(currentDensity, targetDensityCalculator, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Finished fading fog");
        
        RenderSettings.fogDensity = targetDensityCalculator;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (strikes >= 3)
            {
                gameOver = true;
                Debug.Log("Game Over!");
                StopGame();
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= gameDuration && gameStarted)
                {
                    gameOver = true;
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

        if (player.GetComponent<CharacterController>().enabled)
            LookForCheatCodes();
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

        for (int i = 0; i < strikesImages.Length; i++)
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
            //musicPlayer.GetComponent<AudioManager>().changeBackgroundMusic(loseMusic);
            //musicPlayer.GetComponent<AudioSource>().loop = false;
            Debug.Log("AI PIERDUT");
            musicPlayer.GetComponent<AudioManager>().stopBackgroundMusic();
            StartCoroutine(playGameOverSound(false));
            //loseAudioSource.Play();
            strikesPenalizationText.text = "ALL(3) x 50";
            survivedLevelScore = 0;
        }
        else
        {
            //musicPlayer.GetComponent<AudioManager>().changeToWinMusic();
            //musicPlayer.GetComponent<AudioManager>().changeBackgroundMusic(winMusic);
            //musicPlayer.GetComponent<AudioSource>().loop = false;
            Debug.Log("AI CASTIGAT");
            musicPlayer.GetComponent<AudioManager>().stopBackgroundMusic();
            StartCoroutine(playGameOverSound(true));
            //winAudioSource.Play();
            gameOverText.text = "You Win!";
            strikesPenalizationText.text = "-" + strikes.ToString() + " x 50";
            if(SceneManager.GetActiveScene().name == "Level 1")
            {
                PlayerPrefs.SetInt("Level2Unlocked", 1);
            }
            else if(SceneManager.GetActiveScene().name == "Level 2")
            {
                PlayerPrefs.SetInt("Level3Unlocked", 1);
            }

        }

        IEnumerator playGameOverSound(bool won)
        {
            yield return new WaitForSeconds(2.0f);
            if (won)
            {
                winAudioSource.Play();
            }
            else
            {
                loseAudioSource.Play();
            }
        }

        if (LeftoversExist())
        {
            noLeftoversBonus = 0;
        }

        totalScore = score + survivedLevelScore + noLeftoversBonus - strikesScore;
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

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name != "Level 3" || !gameWon)
            SceneManager.LoadScene("LevelSelect");
        else
            SceneManager.LoadScene("EndScreen");

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

    public int GetStrikes()
    {
        return strikes;
    }

    public void SetStrikes(int strikes)
    {
        this.strikes = strikes;
    }

    public bool GetGameWon()
    {
        return gameWon;
    }

    public void SetGameWon(bool gameWon)
    {
        this.gameWon = gameWon;
    }

    public void LookForCheatCodes()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCodeSteak[currentIndexOfSteakCheatCode]))
            {
                currentIndexOfSteakCheatCode++;

                if (currentIndexOfSteakCheatCode == cheatCodeSteak.Length)
                {
                    steakGeneratorObject.GetComponent<IngredientSpawner>().Use(player);
                    currentIndexOfSteakCheatCode = 0;
                }
            }
            else
            {
                currentIndexOfSteakCheatCode = 0;
            }
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCodeSausage[currentIndexOfSausageCheatCode]))
            {
                currentIndexOfSausageCheatCode++;

                if (currentIndexOfSausageCheatCode == cheatCodeSausage.Length)
                {
                    sausageGeneratorObject.GetComponent<IngredientSpawner>().Use(player);
                    currentIndexOfSausageCheatCode = 0;
                }
            }
            else
            {
                currentIndexOfSausageCheatCode = 0;
            }
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCodeSalmon[currentIndexOfSalmonCheatCode]))
            {
                currentIndexOfSalmonCheatCode++;

                if (currentIndexOfSalmonCheatCode == cheatCodeSalmon.Length)
                {
                    salmonGeneratorObject.GetComponent<IngredientSpawner>().Use(player);
                    currentIndexOfSalmonCheatCode = 0;
                }
            }
            else
            {
                currentIndexOfSalmonCheatCode = 0;
            }
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatMaxPlayerSpeed[currentIndexOfMaxPlayerSpeedCheatCode]))
            {
                currentIndexOfMaxPlayerSpeedCheatCode++;

                if (currentIndexOfMaxPlayerSpeedCheatCode == cheatMaxPlayerSpeed.Length)
                {
                    player.GetComponent<PlayerMotor>().SetMaxSpeedFor10Seconds();
                    currentIndexOfMaxPlayerSpeedCheatCode = 0;
                }
            }
            else
            {
                currentIndexOfMaxPlayerSpeedCheatCode = 0;
            }
        }
    }
}
