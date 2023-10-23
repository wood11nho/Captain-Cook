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
    // Start is called before the first frame update
    private void Awake()
    {
        playerInput = new PlayerInput();
        walk = playerInput.Walk;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
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
