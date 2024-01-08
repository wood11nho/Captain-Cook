using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OpenInstructions : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool displayInstructions = false;
    private AudioManager audioManager;
    private int ignoreRaycastLayerMaskInt;
    public GameObject book;

    [SerializeField]
    Text pickUpText;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        book.SetActive(displayInstructions);
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    IEnumerator StopPlayerAndRead(GameObject player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        PlayerLook playerLook = player.GetComponent<PlayerLook>();
        playerLook.enabled = false;
        controller.enabled = false;
        pickUpText.text = "To close.";
        while (displayInstructions)
        {
            yield return null;
        }
        StartCoroutine(StartPlayer(player));
    }

    IEnumerator StartPlayer(GameObject player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        PlayerLook playerLook = player.GetComponent<PlayerLook>();
        playerLook.enabled = true;
        controller.enabled = true;
        pickUpText.text = "To interact.";
        yield return null;
    }

    public void Use(GameObject player)
    {
        displayInstructions = !displayInstructions;
        book.SetActive(displayInstructions);

        StartCoroutine(StopPlayerAndRead(player));

    }

    
}
