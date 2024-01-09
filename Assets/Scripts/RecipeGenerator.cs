using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
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

    [SerializeField]
    private GameObject nPCsObject;

    private float timeModifier; 

    private List<Recipe> recipes = new List<Recipe>();

    private GameObject[] npcs;

    private IEnumerator generateRecipesCoroutine;

    private bool gameStarted = false;

    private int indexLastRecipe = 0;

    private RawImage failImage;

    private int currentNpcIndex = 0;

    private int totalChildrenNumberOfNPCsObject;

    // Start is called before the first frame update
    void Awake()
    {
        timeModifier = easyRecipeProbability / (gameManager.GetComponent<GameManager>().GetGameDuration() - maxTimeBetweenRecipes);

        Recipe sausageSandwich = new("Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "BreadSliceIngredient" }, 60.0f, easyRecipeProbability);
        Recipe steakSandwich = new("Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "BreadSliceIngredient" }, 60.0f, easyRecipeProbability);
        Recipe fishFilletSandwich = new("Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "BreadSliceIngredient" }, 60.0f, easyRecipeProbability);
        Recipe mediumSteakSandwich = new("Medium Steak Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 85.0f, mediumRecipeProbability);
        Recipe mediumSausageSandwich = new("Medium Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 85.0f, mediumRecipeProbability); 
        Recipe mediumFishFilletSandwich = new("Medium Salmon Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 85.0f, mediumRecipeProbability);
        Recipe hardSteakSausageSandwich = new("Hard Steak Sausage Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 90.0f, hardRecipeProbability);
        Recipe hardFishFilletSausageSandwich = new("Hard Salmon Sausage Sandwich", new List<string> { "BreadSliceIngredient", "FishFilletCookedIngredient", "SausageSliceIngredient", "PotatoCutCookedIngredient", "OnionSliceIngredient", "BreadSliceIngredient" }, 90.0f, hardRecipeProbability);
        Recipe meatLoverSandwich = new("Meat Lover Sandwich", new List<string> { "BreadSliceIngredient", "SteakCookedIngredient", "FishFilletCookedIngredient","SausageSliceIngredient", "BreadSliceIngredient" }, 90.0f, hardRecipeProbability);

        recipes.Add(sausageSandwich);
        recipes.Add(steakSandwich);
        recipes.Add(fishFilletSandwich);
        recipes.Add(mediumSteakSandwich);
        recipes.Add(mediumSausageSandwich);
        recipes.Add(mediumFishFilletSandwich);
        recipes.Add(hardSteakSausageSandwich);
        recipes.Add(hardFishFilletSausageSandwich);
        recipes.Add(meatLoverSandwich);

        npcs = new GameObject[nPCsObject.transform.childCount];
        for (int i = 0; i < nPCsObject.transform.childCount; i++)
        {
            npcs[i] = nPCsObject.transform.GetChild(i).gameObject;
        }

        totalChildrenNumberOfNPCsObject = nPCsObject.transform.childCount;

    }
    public void DecrementIndexLastRecipe()
    {
        npcs[0].SetActive(false);
        // delete first element of vector npcs
        for (int i = 0; i < totalChildrenNumberOfNPCsObject - 1; i++)
        {
            npcs[i] = npcs[i + 1];
        }
        totalChildrenNumberOfNPCsObject--;
        indexLastRecipe--;
        currentNpcIndex--;
        StartCoroutine(MoveNPCs(currentNpcIndex));
    }

    IEnumerator MoveNPCs(int currentNpcIndex)
    {
        for (int i = 0; i < currentNpcIndex + 1; i++)
        {
            if (npcs[i].active == true)
            {
                Vector3 destination = npcs[i].transform.position + Vector3.forward;
                npcs[i].GetComponent<Animator>().SetBool("isWalking", true);

                while (Vector3.Distance(npcs[i].transform.position, destination) > 0.1f)
                {
                    npcs[i].transform.position = Vector3.MoveTowards(npcs[i].transform.position, destination, 0.5f * Time.deltaTime);
                    yield return null;
                }

                npcs[i].GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
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
                easyRecipeProbability -= timeModifier * Time.deltaTime * 0.5f;
                mediumRecipeProbability += timeModifier * Time.deltaTime * 0.33f;
                hardRecipeProbability += timeModifier * Time.deltaTime * 0.17f;
            }
            
        }
        //Debug.Log("Easy: " + easyRecipeProbability + " Medium: " + mediumRecipeProbability + " Hard: " + hardRecipeProbability);
    }

    public void StartRecipeGenerator()
    {
        gameStarted = true;
        StartCoroutine(GenerateRecipesCoroutine(minTimeBetweenRecipes, maxTimeBetweenRecipes));
    }

    public void StopRecipeGenerator()
    {
        gameStarted = false;
        servingTable.GetComponent<ServingTable>().RemoveAllRecipesFromOpened();
        StopAllCoroutines();
    }

    public void AddTextBoxToPanel(int indexLastRecipe, string recipeName, float expirationTime)
    {
        GameObject textBox = Instantiate(textBoxPrefab, activeRecipesUI.transform);

        RectTransform rectTransform = textBox.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(200 * indexLastRecipe - 800, 0, 0);

        // Get first child of textBox, which is the slider
        UnityEngine.UI.Slider sliderTextBox = textBox.transform.GetChild(0).GetComponent<UnityEngine.UI.Slider>();
        sliderTextBox.maxValue = expirationTime;
        sliderTextBox.value = 0;
        GameObject fill = sliderTextBox.transform.GetChild(1).GetChild(0).gameObject;
        fill.GetComponent<UnityEngine.UI.Image>().color = Color.green;

        failImage = textBox.transform.GetChild(1).GetComponent<RawImage>();

        StartCoroutine(UpdateSlider(sliderTextBox, expirationTime));

        textBox.GetComponent<TextMeshProUGUI>().text = recipeName;
        textBox.SetActive(true);
    }

    IEnumerator UpdateSlider(UnityEngine.UI.Slider slider, float expirationTime)
    {
        while(slider.value < expirationTime)
        {
            slider.value += Time.deltaTime;
            // Get Slider/Fill Area/Fill child of slider
            GameObject fill = slider.transform.GetChild(1).GetChild(0).gameObject;
            
            if (slider.value >= expirationTime / 2.0f)
            {
                fill.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            }
            if (slider.value >= expirationTime / 4.0f * 3.0f)
            {
                fill.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            /*if (slider.value >= expirationTime)
            {
                failImage.gameObject.SetActive(true);
            }*/
            yield return null;
        }
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
        AddTextBoxToPanel(indexLastRecipe, generatedRecipe.GetRecipeName(), generatedRecipe.GetExpirationTime());
        if(currentNpcIndex >= 1)
            npcs[currentNpcIndex].transform.position = new Vector3(npcs[currentNpcIndex - 1].transform.position.x, npcs[currentNpcIndex - 1].transform.position.y, npcs[currentNpcIndex - 1].transform.position.z - 1);
        npcs[currentNpcIndex].SetActive(true);
        currentNpcIndex++;
        indexLastRecipe++;
        return generatedRecipe;
    }

    Recipe GenerateEasyRecipe(int startPosition, int endPosition)
    {
        Recipe easyRecipeChoice = recipes[Random.Range(startPosition, endPosition)];
        Recipe easyRecipe = new Recipe(easyRecipeChoice.GetRecipeName(), easyRecipeChoice.GetIngredientNames(), easyRecipeChoice.GetExpirationTime(), easyRecipeChoice.GetProbability());
        return easyRecipe;
    }

    Recipe GenerateMediumRecipe(int startPosition, int endPosition)
    {
        Recipe mediumRecipeChoice = recipes[Random.Range(startPosition, endPosition)];
        Recipe mediumRecipe = new Recipe(mediumRecipeChoice.GetRecipeName(), mediumRecipeChoice.GetIngredientNames(), mediumRecipeChoice.GetExpirationTime(), mediumRecipeChoice.GetProbability());
        return mediumRecipe;
    }

    Recipe GenerateHardRecipe(int startPosition, int endPosition)
    {
        Recipe hardRecipeChoice = recipes[Random.Range(startPosition, endPosition)];
        Recipe hardRecipe = new Recipe(hardRecipeChoice.GetRecipeName(), hardRecipeChoice.GetIngredientNames(), hardRecipeChoice.GetExpirationTime(), hardRecipeChoice.GetProbability());
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
