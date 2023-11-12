using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 5f;
    public float ySensitivity = 5f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calculate the camera rotation for looking up and down

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        //apply it to camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
