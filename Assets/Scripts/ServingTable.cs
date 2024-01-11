using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ServingTable : MonoBehaviour, IUsable
{
    [SerializeField]
    private GameObject activeRecipesUI;

    [SerializeField]
    private GameObject recipeGenerator;

    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private AudioSource recipeMatchesAudioSource;

    [SerializeField]
    private AudioSource recipeDoesNotMatchAudioSource;
    public UnityEvent OnUse => throw new System.NotImplementedException();

    private List<Recipe> OpenedRecipes = new List<Recipe>();

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();

        List<string> inHandRecipe = new List<string>();

        if (pickedUpObject == null)
        {
            Debug.Log("You have to have a recipe in hand in order to submit it!");
        }
        else
        {
            int indexOfDoneRecipe = -1;
            bool recipeMatches = false;
            Recipe matchingRecipe = null;

            int nrOfIngredientsInHand = 1 + pickedUpObject.GetComponent<Ingredient>().GetNrOfIngredientChildren();

            string firstIngredientName = pickedUpObject.GetComponent<Ingredient>().GetIngredientName();
            inHandRecipe.Add(firstIngredientName);

            Debug.Log(firstIngredientName);

            for (int i = 0; i < pickedUpObject.transform.childCount; i++)
            {
                Ingredient childIngredient = pickedUpObject.transform.GetChild(i).GetComponent<Ingredient>();

                if (childIngredient != null)
                {
                    string childIngredientName = childIngredient.GetIngredientName();
                    inHandRecipe.Add(childIngredientName);
                    Debug.Log(childIngredientName);
                }

            }

            for (int i = 0; i < OpenedRecipes.Count; i++)
            {
                if (OpenedRecipes[i].GetIngredientNames().Count == nrOfIngredientsInHand)
                {
                    bool ingredientNamesMatch = true;
                    for (int j = 0; j < OpenedRecipes[i].GetIngredientNames().Count; j++)
                    {
                        if (OpenedRecipes[i].GetIngredientNames()[j] != inHandRecipe[j])
                        {
                            Debug.Log("Ingredient Reteta: " + OpenedRecipes[i].GetIngredientNames()[j] + " Ingredient in mana: " + inHandRecipe[j]);
                            ingredientNamesMatch = false;
                        }
                    }
                    if (ingredientNamesMatch)
                    {
                        recipeMatches = true;
                        matchingRecipe = OpenedRecipes[i];
                        indexOfDoneRecipe = i;
                        Debug.Log("Reteta care va fi scoasa din vector: " + OpenedRecipes[i].GetRecipeName() + " la indexul: " + i + " are " + OpenedRecipes[i].GetIngredientNames().Count + " ingrediente");
                        break;
                    }
                }
            }

            if(recipeMatches)
            {
                Debug.Log("Recipe matches!");
                recipeMatchesAudioSource.Play();
                RemoveRecipeFromOpened(indexOfDoneRecipe);
                gameManager.GetComponent<GameManager>().AddScore(gameManager.GetComponent<GameManager>().CalculateRecipeScore(matchingRecipe));
            }
            else
            {
                Debug.Log("Recipe does not match!");
                gameManager.GetComponent<GameManager>().AddStrike();
                recipeDoesNotMatchAudioSource.Play();
            }

            Destroy(pickedUpObject);
            playerItemPickupComponent.SetPickedUpObject(null);

            if (indexOfDoneRecipe != -1)
            {
                recipeGenerator.GetComponent<RecipeGenerator>().DecrementIndexLastRecipe();

                for (int nextRecipeInLineIndex = indexOfDoneRecipe + 1; nextRecipeInLineIndex <= OpenedRecipes.Count; nextRecipeInLineIndex++)
                {
                    RectTransform rectTransform = activeRecipesUI.transform.GetChild(nextRecipeInLineIndex).GetComponent<RectTransform>();
                    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - 200, rectTransform.localPosition.y, rectTransform.localPosition.z);
                }

                // Get the recipe text box and destroy it
                GameObject textBox = activeRecipesUI.transform.GetChild(indexOfDoneRecipe).gameObject;
                Destroy(textBox);
            }
        }

    }

    public void AddRecipeToOpened(Recipe recipe)
    {
        OpenedRecipes.Add(recipe);
    }

    public void RemoveRecipeFromOpened(int indexOfDoneRecipe)
    {
        OpenedRecipes.RemoveAt(indexOfDoneRecipe);
    }

    public void RemoveAllRecipesFromOpened()
    {
        for(int i = OpenedRecipes.Count - 1; i >= 0; i--)
        {
            Destroy(activeRecipesUI.transform.GetChild(i).gameObject);
            OpenedRecipes.RemoveAt(i);
        }
    }

    void Awake()
    {
        StartCoroutine(RemoveExpiredRecipes());
        //OpenedRecipes.Add(new Recipe("Sausage Sandwich", new List<string>{ "BreadSliceIngredient", "SausageSliceIngredient", "BreadSliceIngredient" }, 30f));
    }

    private IEnumerator RemoveExpiredRecipes()
    {
        // I want to make the verification every 1 second
        List<int> indexesToRemove = new List<int>();

        string indexesToRemoveString = "";

        for (int i = 0; i < OpenedRecipes.Count; i++)
        {
            Debug.Log("Recipe named " + OpenedRecipes[i].GetRecipeName() + " has " + OpenedRecipes[i].GetExpirationTime() + " seconds left");
            if (OpenedRecipes[i].GetExpirationTime() <= 0.0f)
            {
                recipeDoesNotMatchAudioSource.Play();
                gameManager.GetComponent<GameManager>().AddStrike();
                //indexesToRemove.Add(i);
                indexesToRemoveString += i + " ";

                RemoveRecipeFromOpened(i);
                recipeGenerator.GetComponent<RecipeGenerator>().DecrementIndexLastRecipe();
                Destroy(activeRecipesUI.transform.GetChild(i).gameObject);

                for (int nextRecipeInLineIndex = i + 1; nextRecipeInLineIndex <= OpenedRecipes.Count; nextRecipeInLineIndex++)
                {
                    RectTransform rectTransform = activeRecipesUI.transform.GetChild(nextRecipeInLineIndex).GetComponent<RectTransform>();
                    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - 200, rectTransform.localPosition.y, rectTransform.localPosition.z);
                }
            }
        }

        /*
        for (int i = 0; i < indexesToRemove.Count; i++)
        {

            RemoveRecipeFromOpened(OpenedRecipes[indexesToRemove[i]]);
            recipeGenerator.GetComponent<RecipeGenerator>().DecrementIndexLastRecipe();
            Destroy(activeRecipesUI.transform.GetChild(indexesToRemove[i]).gameObject);
        }
        */

        yield return new WaitForSeconds(1f);
        StartCoroutine(RemoveExpiredRecipes());

    }

    void Update()
    {
        for (int i = OpenedRecipes.Count - 1; i >= 0; i--)
        {
            OpenedRecipes[i].SetExpirationTime(OpenedRecipes[i].GetExpirationTime() - Time.deltaTime);
        }
    }
    void LateUpdate()
    {
        /*
        for (int i = OpenedRecipes.Count - 1; i >= 0; i--)
        {

            if (OpenedRecipes[i].GetExpirationTime() <= 0.0f)
            {
                gameManager.GetComponent<GameManager>().AddStrike();
                Debug.Log("Recipe expired index: " + i);
                RemoveRecipeFromOpened(OpenedRecipes[i]);
                recipeGenerator.GetComponent<RecipeGenerator>().DecrementIndexLastRecipe();
                Destroy(activeRecipesUI.transform.GetChild(i).gameObject);

                for (int nextRecipeInLineIndex = i + 1; nextRecipeInLineIndex <= OpenedRecipes.Count; nextRecipeInLineIndex++)
                {
                    RectTransform rectTransform = activeRecipesUI.transform.GetChild(nextRecipeInLineIndex).GetComponent<RectTransform>();
                    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - 200, rectTransform.localPosition.y, rectTransform.localPosition.z);
                }
            }
        }
        */
    }

}
