using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class AnchorWheelAnimation : MonoBehaviour, IUsable
{
    private RaycastHit hit;
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool startGame = false;



    void Start()
    {
        
    }

    public void Use(GameObject player)
    {
        Debug.Log("Interact");
        startGame = !startGame;
        gameObject.GetComponent<Animator>().SetBool("isOpened", startGame);


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
