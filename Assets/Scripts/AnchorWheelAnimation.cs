using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnchorWheelAnimation : MonoBehaviour, IUsable
{
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool startGame = false;
    public Camera mainCamera;
    public Camera anchorCamera;
    public GameObject anchor;
    public AudioClip newTrack;
    private AudioManager audioManager;
    private int ignoreRaycastLayerMaskInt;

    [SerializeField]
    private GameObject gameManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        mainCamera.enabled = true;
        anchorCamera.enabled = false;
        ignoreRaycastLayerMaskInt = LayerMask.NameToLayer("IgnoreRaycast");
    }

    public void Use(GameObject player)
    {
        startGame = !startGame;
        Debug.Log(startGame);
        gameObject.GetComponent<Animator>().SetBool("isMoving", startGame);
        //change the background music
        audioManager.changeBackgroundMusic(newTrack);
        gameObject.layer = ignoreRaycastLayerMaskInt;

        // Trigger the coroutine for switching cameras with a delay
        StartCoroutine(SwitchCamerasWithDelay());

       

    }

    IEnumerator SwitchCamerasWithDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Animator>().SetBool("isMoving", false);

        // Deactivate cameras immediately
        //deactivateAllCams();

        // Switch cameras after the delay
        mainCamera.enabled = !mainCamera.enabled;
        anchorCamera.enabled = !anchorCamera.enabled;

        // Anchor animation
        anchor.GetComponent<Animator>().SetBool("anchorMoving", startGame);

        // Wait for another 3 seconds before switching back to the main camera
        yield return new WaitForSeconds(3f);

        // Switch back to the main camera
        //deactivateAllCams();
        anchorCamera.enabled = !anchorCamera.enabled;
        mainCamera.enabled = !mainCamera.enabled;

        // Anchor animation
        anchor.GetComponent<Animator>().SetBool("anchorMoving", false);
        yield return new WaitForSeconds(1f);
        gameManager.GetComponent<GameManager>().StartGame();


    }

    public void deactivateAllCams()
    {
        mainCamera.enabled = false;
        anchorCamera.enabled = false;
    }

   


    // Update is called once per frame
    void Update()
    {
        // Your other update logic here
    }
}
