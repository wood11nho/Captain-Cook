using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject recipeGenerator;

    [SerializeField]
    private float gameDuration = 300.0f;

    private float timeElapsed = 0.0f;

    private int score;

    private int strikes;

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
        }
    }

    public void StartGame()
    {
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

}
