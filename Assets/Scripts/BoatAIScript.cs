using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatAIScript : MonoBehaviour
{

    [SerializeField]
    List<GameObject> boats;

    [SerializeField]
    GameObject destination;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BoatAIScript Start");
        StartAI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAI()
    {
        for(int i = 0; i < boats.Count; i++)
        {
            boats[i].GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);
        }
    }

}
