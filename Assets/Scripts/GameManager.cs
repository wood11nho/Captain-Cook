using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    private float timeElapsed = 0.0f;

    private int score;

    private int strikes;

    private bool gameStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        strikes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(strikes >= 3)
        {
            Debug.Log("Game Over!");
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= gameDuration)
            {
                Debug.Log("Game Over!");
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
        scoreText.gameObject.SetActive(true);
        timeLeftText.gameObject.SetActive(true);
        gameStarted = true;
        recipeGenerator.GetComponent<RecipeGenerator>().StartRecipeGenerator();
        timeElapsed = 0.0f;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public int CalculateRecipeScore(Recipe recipe)
    {
        int recipeScore = 10 * recipe.GetIngredientNames().Count + 200 - (int)recipe.GetExpirationTime();
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

}
