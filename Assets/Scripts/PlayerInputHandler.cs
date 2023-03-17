using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 2f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float webglLookSensitivityMultiplier = 0.25f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }
    public bool run { get; set; }
    public bool jump { get; set; }
    public bool crouch { get; set; }
    public bool hasAnalog { get; private set; }

    private void Awake() {
        InputSystem.onDeviceChange += (device, change) => {
            switch (change) {
                case InputDeviceChange.Added:
                    break;
                case InputDeviceChange.Removed:
                    InputSystem.RemoveDevice(device);
                    break;
            }
        };
    }

    private void Update() {
        var gamepad = Gamepad.current;
        hasAnalog = gamepad is not null;

        if (run && move.sqrMagnitude < 0.01f)
            run = false;
    }

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        look = context.ReadValue<Vector2>();

        Vector2 l = look;
        l.x *= sensitivityX;
        l.y *= sensitivityY;

        if (invertX) l.x *= -1;
        if (invertY) l.y *= -1;

        l *= 0.1f;

#if UNITY_WEBGL
            l *= webglLookSensitivityMultiplier;
#endif

        look = l;
    }

    public void OnRun(InputAction.CallbackContext context) {
        run = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context) {
        jump = context.performed;
    }

    public void OnCrouch(InputAction.CallbackContext context) {
        crouch = context.performed;
    }
}

