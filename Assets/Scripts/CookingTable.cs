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
    private GameObject firstObjectOnTable;
    private int ignoreRaycastLayerMaskInt;

    private float heightOffset = 0.1f;

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
                var nrOfChildren = pickedUpObject.GetComponent<Ingredient>().GetNrOfIngredientChildren();
                if (objectOnTable == null)
                {
                    pickedUpObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
                    pickedUpObject.transform.position = tableObjectPositionTransform.position;
                    pickedUpObject.transform.rotation = tableObjectPositionTransform.rotation;
                    Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                    /*
                    if(rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    */
                    pickedUpObject.transform.SetParent(firstObjectOnTable.transform);
                    playerItemPickupComponent.SetPickedUpObject(null);

                    if (nrOfChildren != 0)
                    {
                        firstObjectOnTable = pickedUpObject;
                        lastObjectOnTable = pickedUpObject.GetComponent<Ingredient>().GetLastIngredientChild();
                        objectOnTable = pickedUpObject;
                    }
                    else
                    {
                        lastObjectOnTable = pickedUpObject;
                        firstObjectOnTable = pickedUpObject;
                        objectOnTable = pickedUpObject;
                    }

                }
                else
                {
                    if(nrOfChildren != 0)
                    {
                        Debug.Log("You can't put a dish on the table!");
                        //return;
                    }
                    else
                    {
                        int nrOfChildrenFirstObject = firstObjectOnTable.GetComponent<Ingredient>().GetNrOfIngredientChildren();
                        pickedUpObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
                        pickedUpObject.transform.SetParent(firstObjectOnTable.transform);
                        pickedUpObject.transform.localPosition = Vector3.up * heightOffset * (nrOfChildrenFirstObject + 1);
                        pickedUpObject.transform.rotation = tableObjectPositionTransform.rotation;
                        Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
                        /*
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }
                        */
                        playerItemPickupComponent.SetPickedUpObject(null);
                        lastObjectOnTable = pickedUpObject;
                    }
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
                firstObjectOnTable = transform.GetChild(0).gameObject;
                lastObjectOnTable = transform.GetChild(0).gameObject;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        firstObjectOnTable = transform.GetChild(0).gameObject;
        lastObjectOnTable = transform.GetChild(0).gameObject;
        //Debug.Log("First object on table: " + firstObjectOnTable);
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(heightOffset);
        //Debug.Log("First object on table: " + lastObjectOnTable.transform.position.y);
    }
}
