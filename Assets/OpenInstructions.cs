using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OpenInstructions : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool displayInstructions = false;
    private AudioManager audioManager;
    private int ignoreRaycastLayerMaskInt;
    public GameObject book;

    [SerializeField]
    private GameObject promtText;




    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        book.SetActive(displayInstructions);
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    public void Use(GameObject player)
    {
        displayInstructions = !displayInstructions;
        book.SetActive(displayInstructions);
        promtText.SetActive(!displayInstructions);
        
        


    }

    
}
