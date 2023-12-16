using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    private string recipeName;
    private List<string> ingredientNames = new List<string>();
    private float expirationTime;

    public Recipe(string recipeName, List<string> ingredientNames, float expirationTime)
    {
        this.recipeName = recipeName;
        this.ingredientNames = ingredientNames;
        this.expirationTime = expirationTime;
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

    public void SetExpirationTime(float expirationTime)
    {
        this.expirationTime = expirationTime;
    }
}
