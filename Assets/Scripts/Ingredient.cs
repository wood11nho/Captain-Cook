using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField]
    private GameObject cookedIngredient;
    
    [SerializeField]
    private GameObject burntIngredient;

    [SerializeField]
    private bool cookable;

    [SerializeField]
    private bool cooked;

    [SerializeField]
    private bool burnt;

    [SerializeField]
    private float burnTime;

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
    
    public bool GetBurnt()
    {
        return burnt;
    }

    public float GetBurnTime()
    {
        return burnTime;
    }
    public GameObject GetBurntIngredient()
    {
        return burntIngredient;
    }

}
