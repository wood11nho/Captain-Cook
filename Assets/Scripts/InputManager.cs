using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.WalkActions walk;

    private PlayerMotor motor;
    private PlayerLook look;
    private ItemPickup itemPickup;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInput = new PlayerInput();
        walk = playerInput.Walk;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        itemPickup = GetComponent<ItemPickup>();

        walk.Interact.performed += ctx => itemPickup.Interact();
        walk.Drop.performed += ctx => itemPickup.Drop();

        walk.OpenRecipeBook.performed += ctx => ToggleRecipeBook();
    }

  
    private void OnEnable()
    {
        walk.Enable();
    }

    private void OnDisable()
    {
        walk.Disable();
    }

    private void Update()
    {
        motor.ProcessMove(walk.Movement.ReadValue<Vector2>());
        look.ProcessLook(walk.Look.ReadValue<Vector2>());

        if (motor.getRecipeBookOpened())
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && motor.getCurrentRecipeBookPage() > 0)
            {
                motor.setCurrentRecipeBookPage(motor.getCurrentRecipeBookPage() - 1);
                motor.activateCurrentRecipeBookPage(motor.getCurrentRecipeBookPage());
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && motor.getCurrentRecipeBookPage() < motor.getTotalRecipeBookPages() - 1)
            {
                motor.setCurrentRecipeBookPage(motor.getCurrentRecipeBookPage() + 1);
                motor.activateCurrentRecipeBookPage(motor.getCurrentRecipeBookPage());
            }
        }
    }

    private void ToggleRecipeBook()
    {
        if (motor.getRecipeBookOpened())
        {
            motor.CloseRecipeBook();
        }
        else
        {
            motor.OpenRecipeBook();
        }
    }
}
