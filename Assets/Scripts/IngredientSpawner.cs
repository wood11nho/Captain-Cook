using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IngredientSpawner : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();

    [SerializeField] private GameObject spawnedIngredient;

    public void Use(GameObject player)
    {
        ItemPickup playerItemPickupComponent = player.GetComponent<ItemPickup>();
        Transform playerPickUpHand = playerItemPickupComponent.GetPickUpHand();
        GameObject newIngredient = Instantiate(spawnedIngredient, Vector3.zero, Quaternion.identity);
        newIngredient.transform.position = playerPickUpHand.position;
        newIngredient.transform.rotation = playerPickUpHand.rotation;
        Rigidbody rb = newIngredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        playerItemPickupComponent.SetPickedUpObject(newIngredient);
        newIngredient.transform.SetParent(playerPickUpHand);
        
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
