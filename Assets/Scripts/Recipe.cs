using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    private string recipeName;
    private List<string> ingredientNames = new List<string>();
    private float expirationTime;
    private float probability;

    public Recipe(string recipeName, List<string> ingredientNames, float expirationTime, float probability)
    {
        this.recipeName = recipeName;
        this.ingredientNames = ingredientNames;
        this.expirationTime = expirationTime;
        this.probability = probability;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetRecipeName()
    {
        return recipeName;
    }

    public List<string> GetIngredientNames()
    {
        return ingredientNames;
    }

    public float GetExpirationTime()
    {
        return expirationTime;
    }

    public float GetProbability()
    {
        return probability;
    }

    public void SetExpirationTime(float expirationTime)
    {
        this.expirationTime = expirationTime;
    }
}
