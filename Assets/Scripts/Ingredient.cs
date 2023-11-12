using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField]
    private GameObject cookedIngredient;

    [SerializeField]
    private bool cookable;

    [SerializeField]
    private bool cooked;

    [SerializeField]
    private float cookTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetCookedIngredient()
    {
        return cookedIngredient;
    }

    public bool GetCookable()
    {
        return cookable;
    }

    public bool GetCooked()
    {
        return cooked;
    }

    public float GetCookTime()
    {
        return cookTime;
    }

    public void SetCooked(bool val)
    {
        cooked = val;
    } 

}
