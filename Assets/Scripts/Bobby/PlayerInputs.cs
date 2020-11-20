using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Inputs")]
    public InputAction dirInput;
    public InputAction runInput;
    public InputAction jumpInput;
    public InputAction throwInput;
    public InputAction interactInput;
    public InputAction attackInput;

    void OnEnable()
    {
        dirInput.Enable();
        runInput.Enable();
        jumpInput.Enable();
        throwInput.Enable();
        interactInput.Enable();
        attackInput.Enable();

    }
    void OnDisable()
    {
        dirInput.Disable();
        runInput.Disable();
        jumpInput.Disable();
        throwInput.Disable();
        interactInput.Disable();
        attackInput.Disable();
    }
}
