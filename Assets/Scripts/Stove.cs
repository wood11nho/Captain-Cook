using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stove : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    private GameObject objectOnStove;

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        Transform stoveSlotTransform = transform.GetChild(0);
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();
        if (pickedUpObject == null)
        {
            Debug.Log("You have to grab an ingredient in order to cook it!");
        }
        else
        {
            if (objectOnStove != null)
            {
                Debug.Log("There is already an ingredient on the stove!");
                return;
            }
            else
            {
                pickedUpObject.transform.SetParent(null);
                pickedUpObject.transform.position = stoveSlotTransform.position;
                pickedUpObject.transform.rotation = stoveSlotTransform.rotation;
                objectOnStove = pickedUpObject;
                Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                pickedUpObject = null;
            }
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
