using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Doors : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool isOpened = false;

    public void Use(GameObject player)
    {
        isOpened = !isOpened;
        gameObject.GetComponent<Animator>().SetBool("isOpened", isOpened);
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
