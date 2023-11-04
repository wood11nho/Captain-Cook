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
    }
}
