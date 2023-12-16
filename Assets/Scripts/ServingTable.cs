using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServingTable : MonoBehaviour, IUsable
{
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
            bool recipeMatches = true;

            int nrOfIngredientsInHand = 1 + pickedUpObject.GetComponent<Ingredient>().GetNrOfIngredientChildren();

            string firstIngredientName = pickedUpObject.GetComponent<Ingredient>().GetIngredientName();
            inHandRecipe.Add(firstIngredientName);

            for (int i = 0; i < pickedUpObject.transform.childCount; i++)
            {
                Ingredient childIngredient = pickedUpObject.transform.GetChild(i).GetComponent<Ingredient>();

                if (childIngredient != null)
                {
                    string childIngredientName = childIngredient.GetIngredientName();
                    inHandRecipe.Add(childIngredientName);
                }

            }

            for (int i = 0; i < OpenedRecipes.Count; i++)
            {
                if (OpenedRecipes[i].GetIngredientNames().Count == nrOfIngredientsInHand)
                {
                    for (int j = 0; j < OpenedRecipes[i].GetIngredientNames().Count; j++)
                    {
                        if (OpenedRecipes[i].GetIngredientNames()[j] != inHandRecipe[j])
                        {
                            recipeMatches = false;
                            break;
                        }
                    }
                }
                else
                {
                    recipeMatches = false;
                }
            }

            if(recipeMatches)
            {
                Debug.Log("Recipe matches!");
            }
            else
            {
                Debug.Log("Recipe does not match!");
            }
            Destroy(pickedUpObject);
            playerItemPickupComponent.SetPickedUpObject(null);

        }

    }

    public void AddRecipeToOpened(Recipe recipe)
    {
        OpenedRecipes.Add(recipe);
    }

    void Awake()
    {
        OpenedRecipes.Add(new Recipe("Sausage Sandwich", new List<string>{ "BreadSliceIngredient", "SausageSliceIngredient", "BreadSliceIngredient" }, 30f));
    }

    void Update()
    {
        //Debug.Log(OpenedRecipes[0].GetRecipeName());
    }
}
