using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private GameObject servingTable;

    [SerializeField]
    private int maxNrOfRecipes = 5;

    [SerializeField]
    private float easyRecipeProbability = 0.5f;

    [SerializeField]
    private float mediumRecipeProbability = 0.3f;

    [SerializeField]
    private float hardRecipeProbability = 0.2f;

    [SerializeField]
    private float minTimeBetweenRecipes = 15.0f;

    [SerializeField]
    private float maxTimeBetweenRecipes = 30.0f;

    private float timeModifier; 

    private List<Recipe> recipes = new List<Recipe>();

    private IEnumerator generateRecipesCoroutine;

    private bool gameStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        timeModifier = easyRecipeProbability / (gameManager.GetComponent<GameManager>().GetGameDuration() - maxTimeBetweenRecipes);

        Recipe sausageSandwich = new Recipe("Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe steakSandwich = new Recipe("Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe fishFilletSandwich = new Recipe("Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe mediumSteakSandwich = new Recipe("Medium Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability);
        Recipe mediumSausageSandwich = new Recipe("Medium Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability); 
        Recipe mediumFishFilletSandwich = new Recipe("Medium Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability);
        Recipe hardSteakSausageSandwich = new Recipe("Hard Steak Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);
        Recipe hardFishFilletSausageSandwich = new Recipe("Hard Salmon Sausage Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);
        Recipe meatLoverSandwich = new Recipe("Meat Lover Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "FishFilletCookedIngredient","SausageSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);

        recipes.Add(sausageSandwich);
        recipes.Add(steakSandwich);
        recipes.Add(fishFilletSandwich);
        recipes.Add(mediumSteakSandwich);
        recipes.Add(mediumSausageSandwich);
        recipes.Add(mediumFishFilletSandwich);
        recipes.Add(hardSteakSausageSandwich);
        recipes.Add(hardFishFilletSausageSandwich);
        recipes.Add(meatLoverSandwich);

    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.GetComponent<GameManager>().GetGameDuration() - gameManager.GetComponent<GameManager>().GetTimeElapsed() <= maxTimeBetweenRecipes)
        {
            StopAllCoroutines();
        }
        else
        {
            if (gameStarted)
            {
                easyRecipeProbability -= timeModifier * Time.deltaTime;
                mediumRecipeProbability += timeModifier * 0.33f * Time.deltaTime;
                hardRecipeProbability += timeModifier * 0.66f * Time.deltaTime;
            }
            
        }
        //Debug.Log("Easy: " + easyRecipeProbability + " Medium: " + mediumRecipeProbability + " Hard: " + hardRecipeProbability);
    }

    public void StartRecipeGenerator()
    {
        gameStarted = true;
        StartCoroutine(GenerateRecipesCoroutine(minTimeBetweenRecipes, maxTimeBetweenRecipes));
    }

    Recipe GenerateRecipe()
    {
        Recipe generatedRecipe = null;
        float randomNr = Random.Range(0.0f, 1.0f);
        if(randomNr <= easyRecipeProbability)
        {
            generatedRecipe = GenerateEasyRecipe(0, 2);
        }
        else if(randomNr <= easyRecipeProbability + mediumRecipeProbability)
        {
            generatedRecipe = GenerateMediumRecipe(3, 5);
        }
        else
        {
            generatedRecipe = GenerateHardRecipe(6, 8);
        }

        Debug.Log("Generated recipe: " + generatedRecipe.GetRecipeName());
        return generatedRecipe;
    }

    Recipe GenerateEasyRecipe(int startPosition, int endPosition)
    {
        Recipe easyRecipe = recipes[Random.Range(startPosition, endPosition)];
        return easyRecipe;
    }

    Recipe GenerateMediumRecipe(int startPosition, int endPosition)
    {
        Recipe mediumRecipe = recipes[Random.Range(startPosition, endPosition)];
        return mediumRecipe;
    }

    Recipe GenerateHardRecipe(int startPosition, int endPosition)
    {
        Recipe hardRecipe = recipes[Random.Range(startPosition, endPosition)];
        return hardRecipe;
    }

    IEnumerator GenerateRecipesCoroutine(float minTimeBetween, float maxTimeBetween)
    {
        servingTable.GetComponent<ServingTable>().AddRecipeToOpened(GenerateRecipe());
        yield return new WaitForSeconds(Random.Range(minTimeBetween, maxTimeBetween));
        StartCoroutine(GenerateRecipesCoroutine(minTimeBetween, maxTimeBetween));
    }
    

}
