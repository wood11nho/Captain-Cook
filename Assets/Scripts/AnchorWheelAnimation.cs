using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnchorWheelAnimation : MonoBehaviour, IUsable
{
    private RaycastHit hit;
    public UnityEvent OnUse => throw new System.NotImplementedException();
    [SerializeField]
    bool startGame = false;
    public Camera mainCamera;
    public Camera anchorCamera;
    public GameObject anchor;

    public void Use(GameObject player)
    {
        startGame = !startGame;
        Debug.Log(startGame);
        gameObject.GetComponent<Animator>().SetBool("startGame", startGame);

        // Trigger the coroutine for switching cameras with a delay
        StartCoroutine(SwitchCamerasWithDelay());

       

    }

    IEnumerator SwitchCamerasWithDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Deactivate cameras immediately
        deactivateAllCams();

        // Switch cameras after the delay
        mainCamera.enabled = !mainCamera.enabled;
        anchorCamera.enabled = !anchorCamera.enabled;

        // Anchor animation
        anchor.GetComponent<Animator>().SetBool("startGame", startGame);

        // Wait for another 3 seconds before switching back to the main camera
        yield return new WaitForSeconds(3f);

        // Switch back to the main camera
        deactivateAllCams();
        anchorCamera.enabled = !anchorCamera.enabled;
        mainCamera.enabled = !mainCamera.enabled;

        // Anchor animation
        anchor.GetComponent<Animator>().SetBool("startGame", startGame);
        gameObject.GetComponent<Animator>().SetBool("startGame", false);



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
