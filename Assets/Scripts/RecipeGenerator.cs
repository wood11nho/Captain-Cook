using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    [SerializeField]
    private GameObject activeRecipesUI;

    [SerializeField]
    private GameObject textBoxPrefab;

    private float timeModifier; 

    private List<Recipe> recipes = new List<Recipe>();

    private IEnumerator generateRecipesCoroutine;

    private bool gameStarted = false;

    private int indexLastRecipe = 0;

    // Start is called before the first frame update
    void Awake()
    {
        timeModifier = easyRecipeProbability / (gameManager.GetComponent<GameManager>().GetGameDuration() - maxTimeBetweenRecipes);

        Recipe sausageSandwich = new("Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe steakSandwich = new("Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe fishFilletSandwich = new("Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "BreadSliceIngredient" }, 30.0f, easyRecipeProbability);
        Recipe mediumSteakSandwich = new("Medium Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability);
        Recipe mediumSausageSandwich = new("Medium Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability); 
        Recipe mediumFishFilletSandwich = new("Medium Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 45.0f, mediumRecipeProbability);
        Recipe hardSteakSausageSandwich = new("Hard Steak Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);
        Recipe hardFishFilletSausageSandwich = new("Hard Salmon Sausage Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);
        Recipe meatLoverSandwich = new("Meat Lover Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "FishFilletCookedIngredient","SausageSliceIngredient", "BreadSliceIngredient" }, 60.0f, hardRecipeProbability);

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
    public void DecrementIndexLastRecipe()
    {
        indexLastRecipe--;
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

    public void AddTextBoxToPanel(int indexLastRecipe, string recipeName)
    {
        GameObject textBox = Instantiate(textBoxPrefab, activeRecipesUI.transform);

        RectTransform rectTransform = textBox.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(200 * indexLastRecipe - 800, 0, 0);

        textBox.GetComponent<TextMeshProUGUI>().text = recipeName;
        textBox.SetActive(true);
    }

    Recipe GenerateRecipe()
    {
        Recipe generatedRecipe = null;
        float randomNr = Random.Range(0.0f, 1.0f);
        if (indexLastRecipe >= maxNrOfRecipes)
        {
            return null;
        }
        if(randomNr <= easyRecipeProbability)
        {
            generatedRecipe = GenerateEasyRecipe(0, 3);
        }
        else if(randomNr <= easyRecipeProbability + mediumRecipeProbability)
        {
            generatedRecipe = GenerateMediumRecipe(3, 6);
        }
        else
        {
            generatedRecipe = GenerateHardRecipe(6, 9);
        }
        Debug.Log("Generated recipe: " + generatedRecipe.GetRecipeName());
        AddTextBoxToPanel(indexLastRecipe, generatedRecipe.GetRecipeName());
        indexLastRecipe++;
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
        Recipe generatedRecipe = GenerateRecipe();

        if (generatedRecipe.GetRecipeName() != null)
        {
            servingTable.GetComponent<ServingTable>().AddRecipeToOpened(generatedRecipe);
        }

        yield return new WaitForSeconds(Random.Range(minTimeBetween, maxTimeBetween));

        StartCoroutine(GenerateRecipesCoroutine(minTimeBetween, maxTimeBetween));
    }

}
