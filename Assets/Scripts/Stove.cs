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
        Transform stoveObjectPositionTransform = transform.GetChild(1);
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();

        if (pickedUpObject == null && objectOnStove == null)
        {
            Debug.Log("You have to grab an ingredient to cook it!");
        }
        else
        {
            if (objectOnStove != null)
            {
                if(pickedUpObject != null)
                {
                    Debug.Log("There is already an ingredient on the stove!");
                }
                else
                {
                    objectOnStove.layer = LayerMask.NameToLayer("Pickable");
                    objectOnStove.transform.SetParent(playerPickUpHand);
                    objectOnStove.transform.position = playerPickUpHand.position;
                    objectOnStove.transform.rotation = playerPickUpHand.rotation;
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
                pickedUpObject.layer = ignoreRaycastLayerMaskInt;
                pickedUpObject.transform.SetParent(null);
                pickedUpObject.transform.position = stoveObjectPositionTransform.position;
                pickedUpObject.transform.rotation = stoveObjectPositionTransform.rotation;
                objectOnStove = pickedUpObject;
                Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                /*
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                */
                playerItemPickupComponent.SetPickedUpObject(null);
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
}
