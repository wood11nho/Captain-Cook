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

    private RaycastHit hit;

    private void Start()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interact");
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }
    }

    private void Update()
    {
        if(hit.collider != null)
        {
            hit.collider.GetComponent<HighlightObject>()?.ToggleHighlght(false);
            pickUpUI.SetActive(false);
        }
        if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<HighlightObject>()?.ToggleHighlght(true);
            pickUpUI.SetActive(true);
        }
         
    }
}
