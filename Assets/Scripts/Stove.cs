using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Stove : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    private GameObject objectOnStove;
    private int ignoreRaycastLayerMaskInt;
    public Slider cookingTimeSlider;
    public GameObject loadingScreen;

    private ParticleSystem smokeParticleSystem;
    private AudioSource fryAudioSource;
    private float fadeOutDuration = 1.0f;

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
                    objectOnStove.transform.localScale = objectOnStove.transform.localScale * 1.33f;
                    Rigidbody rb = objectOnStove.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                    playerItemPickupComponent.SetPickedUpObject(objectOnStove);
                    objectOnStove = null;
                    loadingScreen.SetActive(false);
                    smokeParticleSystem.Stop();
                    StartCoroutine(FadeOutRoutine());
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
                    objectOnStove.transform.localScale = objectOnStove.transform.localScale * 0.75f;
                    Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                    /*
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    */
                    playerItemPickupComponent.SetPickedUpObject(null);
                    StopAllCoroutines();
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
        smokeParticleSystem = GetComponentInChildren<ParticleSystem>();
        fryAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectOnStove != null)
        {
            Vector3 stoveScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 sliderPosition = new Vector3(stoveScreenPosition.x, stoveScreenPosition.y + 100f, stoveScreenPosition.z);
            cookingTimeSlider.transform.position = sliderPosition;
        }
    }

    IEnumerator CookIngredient(GameObject ingredient)
    {
        smokeParticleSystem.Play();
        fryAudioSource.Play();
        ingredient.layer = ignoreRaycastLayerMaskInt;
        //yield return new WaitForSeconds(ingredient.GetComponent<Ingredient>().GetCookTime());

        float cookTime = ingredient.GetComponent<Ingredient>().GetCookTime();

        loadingScreen.SetActive(true);
        cookingTimeSlider.value = 0f;
        while (cookingTimeSlider.value < 1f)
        {
            cookingTimeSlider.value += Time.deltaTime / cookTime;
            yield return null;
        }
        loadingScreen.SetActive(false);

        if (objectOnStove != null && objectOnStove == ingredient)
        {
            GameObject cookedIngredient = Instantiate(ingredient.GetComponent<Ingredient>().GetCookedIngredient(), objectOnStove.transform.position, objectOnStove.transform.rotation);
            cookedIngredient.transform.localScale = cookedIngredient.transform.localScale * 0.75f;
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
        else
        {
            yield break;
        }
        StartCoroutine(BurnIngredient(objectOnStove));
        
    }

    IEnumerator BurnIngredient(GameObject ingredient)
    {
        ingredient.layer = ignoreRaycastLayerMaskInt;

        float burnTime = ingredient.GetComponent<Ingredient>().GetBurnTime();
        yield return new WaitForSeconds(burnTime);

        if (objectOnStove != null && objectOnStove == ingredient)
        {
            GameObject burntIngredient = Instantiate(ingredient.GetComponent<Ingredient>().GetBurntIngredient(), objectOnStove.transform.position, objectOnStove.transform.rotation);
            burntIngredient.transform.localScale = ingredient.transform.localScale;
            burntIngredient.layer = ignoreRaycastLayerMaskInt;
            Rigidbody rb = burntIngredient.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            objectOnStove = null;
            Destroy(ingredient);
            objectOnStove = burntIngredient;
        }
        else
        {
            yield break;
        }


    }

    IEnumerator FadeOutRoutine()
    {
        float startVolume = fryAudioSource.volume;

        while (fryAudioSource.volume > 0)
        {
            fryAudioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;

            yield return null;
        }

        fryAudioSource.Stop();
        fryAudioSource.volume = startVolume;
    }

}