using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CookingTable : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();

    [SerializeField]
    private float offset = 0.15f;

    private GameObject lastObjectOnTable;
    private GameObject objectOnTable;
    private int ignoreRaycastLayerMaskInt;

    private float heightOffset = 0.0f;

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        Transform tableObjectPositionTransform = transform.GetChild(0);
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();

        if(pickedUpObject == null && objectOnTable == null)
        {
            Debug.Log("You have to grab an ingredient to put on the table!");
        }
        else
        {
            if(pickedUpObject != null)
            {
                if(objectOnTable == null)
                {
                    pickedUpObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
                    pickedUpObject.transform.position = tableObjectPositionTransform.position;
                    pickedUpObject.transform.rotation = tableObjectPositionTransform.rotation;
                    pickedUpObject.transform.SetParent(tableObjectPositionTransform);
                    Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                    /*
                    if(rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    */
                    playerItemPickupComponent.SetPickedUpObject(null);
                    lastObjectOnTable = pickedUpObject;
                    objectOnTable = pickedUpObject;
                    heightOffset += tableObjectPositionTransform.localScale.y;
                }
                else
                {
                    pickedUpObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
                    pickedUpObject.transform.position = lastObjectOnTable.transform.position + new Vector3(0.0f, offset, 0.0f);
                    pickedUpObject.transform.rotation = tableObjectPositionTransform.rotation;
                    pickedUpObject.transform.SetParent(lastObjectOnTable.transform);
                    Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                    /*
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    */
                    playerItemPickupComponent.SetPickedUpObject(null);
                    lastObjectOnTable = pickedUpObject;
                    heightOffset += lastObjectOnTable.transform.localScale.y;
                }
            }
            else
            {
                objectOnTable.layer = LayerMask.NameToLayer("Pickable");
                objectOnTable.transform.SetParent(playerPickUpHand);
                objectOnTable.transform.position = playerPickUpHand.position;
                objectOnTable.transform.rotation = playerPickUpHand.rotation;
                Rigidbody rb = objectOnTable.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                playerItemPickupComponent.SetPickedUpObject(objectOnTable);
                objectOnTable = null;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(heightOffset);
    }
}
