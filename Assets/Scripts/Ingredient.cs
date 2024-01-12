using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField]
    private string ingredientName;
    
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

    public GameObject GetCutIngredient()
    {
        return cutIngredient;
    }

    public int GetNrOfIngredientChildren()
    {
        int count = 0;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.CompareTag("Ingredient"))
            {
                count++;
            }
        }

        return count;

    }

    public GameObject GetLastIngredientChild()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            if(transform.GetChild(i).gameObject.CompareTag("Ingredient"))
            {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public string GetIngredientName()
    {
        return ingredientName;
    }

}
