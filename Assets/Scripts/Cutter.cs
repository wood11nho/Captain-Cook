using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutter : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    private GameObject objectOnCutter;
    private int ignoreRaycastLayerMaskInt;
    private AudioSource cutterAudioSource;

    private Animator cutterAnimator;
    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();
        if (pickedUpObject == null && objectOnCutter == null)
        {
            Debug.Log("You have to grab an ingredient to cut it!");
        }
        else
        {
            if (objectOnCutter != null)
            {
                Debug.Log("There is already an ingredient on the cutter!");
            }
            else
            {
                if (!pickedUpObject.GetComponent<Ingredient>().GetCuttable())
                {
                    Debug.Log("You can't cut this ingredient!");
                }
                else
                {
                    if (pickedUpObject.GetComponent<Ingredient>().GetCut())
                    {
                        Debug.Log("This ingredient is already cut!");
                    }
                    else
                    {
                        StartCoroutine(StopPlayerAndCut(player));
                    }
                }
                
                
            }

        }
        return;
    }

    IEnumerator StopPlayerAndCut(GameObject player)
    {
        cutterAnimator.SetBool("isCutting", true);
        cutterAudioSource.Play();
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform cutterObjectPositionTranform = transform.GetChild(0);
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();
        pickedUpObject.layer = ignoreRaycastLayerMaskInt;
        pickedUpObject.transform.SetParent(null);
        pickedUpObject.transform.position = cutterObjectPositionTranform.position;
        pickedUpObject.transform.rotation = cutterObjectPositionTranform.rotation;
        objectOnCutter = pickedUpObject;
        Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
        playerItemPickupComponent.SetPickedUpObject(null);
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        gameObject.layer = ignoreRaycastLayerMaskInt; 
        playerItemPickupComponent.enabled = false;
        yield return new WaitForSeconds(objectOnCutter.GetComponent<Ingredient>().GetCutTime());
        StartCoroutine(StartPlayer(player));
    }

    IEnumerator StartPlayer(GameObject player)
    {
        cutterAnimator.SetBool("isCutting", false);
        cutterAudioSource.Stop();
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = true;
        playerItemPickupComponent.enabled = true;
        GameObject cutObject = Instantiate(objectOnCutter.GetComponent<Ingredient>().GetCutIngredient(), playerPickUpHand.position, playerPickUpHand.rotation);
        cutObject.layer = LayerMask.NameToLayer("Pickable");
        Rigidbody rb = cutObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        playerItemPickupComponent.SetPickedUpObject(cutObject);
        cutObject.transform.SetParent(playerPickUpHand);
        Destroy(objectOnCutter);
        objectOnCutter = null;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
        cutterAnimator = GetComponent<Animator>();
        cutterAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
