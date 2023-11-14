using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Disposal : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool isOpened = false;
    bool duringUse = false;

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        GameObject pickedUpObject = playerItemPickupComponent.GetPickedUpObject();

        if (pickedUpObject == null)
        {
            Debug.Log("You have to grab an ingredient to dispose of it!");
        }
        else
        {
            pickedUpObject.transform.SetParent(null);
            playerItemPickupComponent.SetPickedUpObject(null);
            Destroy(pickedUpObject);

            if (!duringUse)
            {
                isOpened = !isOpened;
                gameObject.GetComponent<Animator>().SetBool("IsOpened", isOpened);
                StartCoroutine(UseRoutine());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(UseRoutine());
            }
        }
    }

    IEnumerator UseRoutine()
    {
        duringUse = true;
        yield return new WaitForSeconds(2.0f);
        isOpened = !isOpened;
        gameObject.GetComponent<Animator>().SetBool("IsOpened", isOpened);
        duringUse = false;
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
