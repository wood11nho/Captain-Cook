using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField]
    GameObject destination;

    [SerializeField]
    GameObject recipeGenerator;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npcDestinationArea"))
        {
            StopCoroutine(recipeGenerator.GetComponent<RecipeGenerator>().getLeaveNpcsCoroutine());
            Debug.Log("NPC reached destination");
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            gameObject.SetActive(false);
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

    public void GoToDestination()
    {
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(destination.transform.position);
    }

}
