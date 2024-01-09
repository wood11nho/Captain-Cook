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
        StartCoroutine(MoveLogo());
    }


    // Update is called once per frame
    void Update()
    {
    
    }

    IEnumerator MoveLogo()
    {
        while (true)
        {
            logoImage.transform.position = Vector3.MoveTowards(logoImage.transform.position, new Vector3(logoImage.transform.position.x, logoImage.transform.position.y + 100f, logoImage.transform.position.z), Time.deltaTime * 5f);

            yield return null; // Wait for the next frame

            yield return new WaitForSeconds(5f);

            logoImage.transform.position = Vector3.MoveTowards(logoImage.transform.position, new Vector3(logoImage.transform.position.x, logoImage.transform.position.y - 200f, logoImage.transform.position.z), Time.deltaTime * 5f);

            yield return null; // Wait for the next frame

            yield return new WaitForSeconds(5f);
        }
    }

}
