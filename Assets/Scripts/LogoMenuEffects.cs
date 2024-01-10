using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoMenuEffects : MonoBehaviour
{
    [SerializeField]
    GameObject logoImage;

    bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        StartCoroutine(MoveLogo());
    }


    // Update is called once per frame
    void Update()
    {
    
    }
    IEnumerator MoveLogo()
    {
        Vector3 targetPosition;

        while (true)
        {
            targetPosition = new Vector3(logoImage.transform.position.x, logoImage.transform.position.y + 10, logoImage.transform.position.z);
            yield return MoveLogoToPosition(targetPosition, 1f);

            targetPosition = new Vector3(logoImage.transform.position.x, logoImage.transform.position.y - 20, logoImage.transform.position.z);
            yield return MoveLogoToPosition(targetPosition, 1f);

            targetPosition = new Vector3(logoImage.transform.position.x, logoImage.transform.position.y + 10, logoImage.transform.position.z);
            yield return MoveLogoToPosition(targetPosition, 1f);
        }
    }

    IEnumerator MoveLogoToPosition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = logoImage.transform.position;

        while (elapsedTime < duration)
        {
            logoImage.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        logoImage.transform.position = targetPosition; // Ensure the logo reaches the exact target position
    }


}
