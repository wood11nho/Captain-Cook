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
    private GameObject cutIngredient;

    [SerializeField]
    private bool cookable;

    [SerializeField]
    private bool cuttable;

    [SerializeField]
    private bool cooked;

    [SerializeField]
    private bool burnt;

    [SerializeField]
    private bool cut;

    [SerializeField]
    private float burnTime;

    [SerializeField]
    private float cookTime;

    [SerializeField]
    private float cutTime;

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

    public bool GetCuttable()
    {
        return cuttable;
    }

    public bool GetCooked()
    {
        return cooked;
    }

    public bool GetCut()
    {
        return cut;
    }

    public float GetCookTime()
    {
        return cookTime;
    }

    public float GetCutTime()
    {
        return cutTime;
    }

    public void SetCooked(bool val)
    {
        cooked = val;
    }
    public void SetCut(bool val)
    {
        cut = val;
    }
    
    public bool GetBurnt()
    {
        return burnt;
    }

    public float GetBurnTime()
    {
        return burnTime;
    }

    public void SetBurnt(bool val)
    {
        burnt = val;
    }

    public GameObject GetBurntIngredient()
    {
        return burntIngredient;
    }

}
