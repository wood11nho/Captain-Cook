using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private GameObject pickUpUI;

    [SerializeField]
    [Min(1)]
    private float hitRange = 3f;

    [SerializeField]
    private Transform pickUpHand;

    [SerializeField]
    private GameObject pickedUpObject;

    private RaycastHit hit;

    private void Start()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interact");
        if(hit.collider != null && pickedUpObject == null)
        {
            Debug.Log(hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (hit.collider.GetComponent<Ingredient>())
            {
                Debug.Log("Ingredient");
                pickedUpObject = hit.collider.gameObject;
                pickedUpObject.transform.position = pickUpHand.position;
                pickedUpObject.transform.rotation = pickUpHand.rotation;
                pickedUpObject.transform.SetParent(pickUpHand);
                if(rb != null)
                {
                    rb.isKinematic = true;
                }
                return;
            }

        }
    }

    public void Drop()
    {
        if(pickedUpObject != null)
        {
            pickedUpObject.transform.SetParent(null);
            Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
            }
            pickedUpObject = null;
            
        }
    }

    private void Update()
    {
        if(hit.collider != null)
        {
            hit.collider.GetComponent<HighlightObject>()?.ToggleHighlght(false);
            pickUpUI.SetActive(false);
        }

        if(pickedUpObject != null)
        {
            return;
        }

        if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<HighlightObject>()?.ToggleHighlght(true);
            pickUpUI.SetActive(true);
        }
         
    }
}
