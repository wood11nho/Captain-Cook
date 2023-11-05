using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutter : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    private GameObject objectOnCutter;
    private int ignoreRaycastLayerMaskInt;
    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        Transform cutterObjectPositionTranform = transform.GetChild(1);
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
                pickedUpObject.layer = ignoreRaycastLayerMaskInt;
                pickedUpObject.transform.SetParent(null);
                pickedUpObject.transform.position = cutterObjectPositionTranform.position;
                pickedUpObject.transform.rotation = cutterObjectPositionTranform.rotation;
                objectOnCutter = pickedUpObject;
                Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                /*
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                */
                playerItemPickupComponent.SetPickedUpObject(null);
                StartCoroutine(StopPlayer(player));
                
            }

        }
        return;
    }

    IEnumerator StopPlayer(GameObject player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        gameObject.layer = ignoreRaycastLayerMaskInt; 
        yield return new WaitForSeconds(5f);
        StartCoroutine(StartPlayer(player));
    }

    IEnumerator StartPlayer(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = true;
        objectOnCutter.layer = LayerMask.NameToLayer("Pickable");
        objectOnCutter.transform.SetParent(playerPickUpHand);
        objectOnCutter.transform.position = playerPickUpHand.position;
        objectOnCutter.transform.rotation = playerPickUpHand.rotation;
        Rigidbody rb = objectOnCutter.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        playerItemPickupComponent.SetPickedUpObject(objectOnCutter);
        objectOnCutter = null;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        yield return null;
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
}
