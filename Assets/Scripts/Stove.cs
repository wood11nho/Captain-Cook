using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Stove : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    private GameObject objectOnStove;
    private int ignoreRaycastLayerMaskInt;

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        Transform stoveObjectPositionTransform = transform.GetChild(0);
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();

        if (pickedUpObject == null && objectOnStove == null)
        {
            Debug.Log("You have to grab an ingredient to cook it!");
        }
        else
        {
            if (objectOnStove != null)
            {
                if (pickedUpObject != null)
                {
                    Debug.Log("There is already an ingredient on the stove!");
                }
                else
                {
                    objectOnStove.layer = LayerMask.NameToLayer("Pickable");
                    objectOnStove.transform.SetParent(playerPickUpHand);
                    objectOnStove.transform.position = playerPickUpHand.position;
                    objectOnStove.transform.rotation = playerPickUpHand.rotation;
                    objectOnStove.transform.localScale = Vector3.one;
                    Rigidbody rb = objectOnStove.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                    playerItemPickupComponent.SetPickedUpObject(objectOnStove);
                    objectOnStove = null;
                }
            }
            else
            {
                if (pickedUpObject.GetComponent<Ingredient>().GetCooked())
                {
                    Debug.Log("You can't cook a cooked ingredient!");
                    return;
                }
                else if (!pickedUpObject.GetComponent<Ingredient>().GetCookable())
                {
                    Debug.Log("You can't cook this ingredient!");
                    return;

                }
                else
                {
                    pickedUpObject.layer = ignoreRaycastLayerMaskInt;
                    pickedUpObject.transform.SetParent(null);
                    pickedUpObject.transform.position = stoveObjectPositionTransform.position;
                    //pickedUpObject.transform.rotation = stoveObjectPositionTransform.rotation;
                    objectOnStove = pickedUpObject;
                    objectOnStove.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                    /*
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    */
                    playerItemPickupComponent.SetPickedUpObject(null);
                    StartCoroutine(CookIngredient(objectOnStove));
                }
            }

        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CookIngredient(GameObject ingredient)
    {
        ingredient.layer = ignoreRaycastLayerMaskInt;
        yield return new WaitForSeconds(ingredient.GetComponent<Ingredient>().GetCookTime());
        if (objectOnStove == null)
        {
            yield break;
        }
        GameObject cookedIngredient = Instantiate(ingredient.GetComponent<Ingredient>().GetCookedIngredient(), objectOnStove.transform.position, objectOnStove.transform.rotation);
        cookedIngredient.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        cookedIngredient.layer = ignoreRaycastLayerMaskInt;
        Rigidbody rb = cookedIngredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        objectOnStove = null;
        Destroy(ingredient);
        objectOnStove = cookedIngredient;
    }

}