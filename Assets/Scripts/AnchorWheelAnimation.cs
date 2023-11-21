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



   

    public void Use(GameObject player)
    {
        startGame = !startGame;
        Debug.Log(startGame);
        gameObject.GetComponent<Animator>().SetBool("startGame", startGame);
        //change camera position
        


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
