using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<InputAction.CallbackContext> MovePerformed;
    public event Action<InputAction.CallbackContext> MoveStarted;
    public event Action<InputAction.CallbackContext> MoveCanceled;
    public event Action<InputAction.CallbackContext> InteractPerformed;
    public event Action<InputAction.CallbackContext> BodyShapeShiftStarted;
    public event Action<InputAction.CallbackContext> BodyShapeShiftCanceled;
    public event Action<InputAction.CallbackContext> BodyShapeShiftPerformed;
    public event Action<InputAction.CallbackContext> Time_spaceShiftPerformed;
    public event Action<InputAction.CallbackContext> JumpPerformed;
    public event Action<InputAction.CallbackContext> JumpCanceled;
    public event Action<InputAction.CallbackContext> JumpStarted;

    private PlayerInputs inputActions;
    private bool callbacksRegistered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new PlayerInputs();
    }

    private void Start()
    {
        if (Instance != this || inputActions == null)
        {
            return;
        }

        RegisterCallbacks();
        inputActions.PlayerControl.Enable();
    }
    /*
    private void OnEnable()
    {
        if (Instance != this || inputActions == null)
        {
            return;
        }

        RegisterCallbacks();
        inputActions.PlayerControl.Enable();
    }

    private void OnDisable()
    {
        if (inputActions == null)
        {
            return;
        }

        UnregisterCallbacks();
        inputActions.PlayerControl.Disable();
    }


    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        UnregisterCallbacks();
        inputActions?.Dispose();
        inputActions = null;
        Instance = null;
    }
    */
    private void RegisterCallbacks()
    {
        if (callbacksRegistered)
        {
            return;
        }

        inputActions.PlayerControl.Move.performed += HandleMovePerformed;
        inputActions.PlayerControl.Move.started += HandleMoveStarted;
        inputActions.PlayerControl.Move.canceled += HandleMoveCanceled;
        inputActions.PlayerControl.InterAct.performed += HandleInteractPerformed;
        inputActions.PlayerControl.BodyShapeShift.performed += HandleBodyShapeShiftPerformed;
        inputActions.PlayerControl.BodyShapeShift.canceled += HandleBodyShapeShiftCanceled;
        inputActions.PlayerControl.BodyShapeShift.performed += HandleBodyShapeShiftPerformed;
        inputActions.PlayerControl.TimespaceShift.performed += HandleTime_spaceShiftPerformed;
        inputActions.PlayerControl.Jump.performed += HandleJumpPerformed;
        inputActions.PlayerControl.Jump.canceled += HandleJumpCanceled;
        inputActions.PlayerControl.Jump.started += HandleJumpStarted;
        callbacksRegistered = true;
    }



    private void UnregisterCallbacks()
    {
        if (!callbacksRegistered || inputActions == null)
        {
            return;
        }

        inputActions.PlayerControl.Move.performed -= HandleMovePerformed;
        inputActions.PlayerControl.Move.canceled -= HandleMoveCanceled;
        inputActions.PlayerControl.InterAct.performed -= HandleInteractPerformed;
        inputActions.PlayerControl.BodyShapeShift.performed -= HandleBodyShapeShiftPerformed;
        inputActions.PlayerControl.BodyShapeShift.canceled -= HandleBodyShapeShiftCanceled;
        inputActions.PlayerControl.BodyShapeShift.performed -= HandleBodyShapeShiftPerformed;
        inputActions.PlayerControl.TimespaceShift.performed -= HandleTime_spaceShiftPerformed;
        inputActions.PlayerControl.Jump.performed -= HandleJumpPerformed;
        inputActions.PlayerControl.Jump.canceled -= HandleJumpCanceled;
        inputActions.PlayerControl.Jump.started -= HandleJumpStarted;
        callbacksRegistered = false;
    }

    private void HandleMovePerformed(InputAction.CallbackContext context)
    {
        MovePerformed?.Invoke(context);
    }
    private void HandleMoveStarted(InputAction.CallbackContext context)
    {
        MoveStarted?.Invoke(context);
    }
    private void HandleMoveCanceled(InputAction.CallbackContext context)
    {
        MoveCanceled?.Invoke(context);
    }

    private void HandleInteractPerformed(InputAction.CallbackContext context)
    {
        InteractPerformed?.Invoke(context);
    }

    private void HandleBodyShapeShiftCanceled(InputAction.CallbackContext context)
    {
        BodyShapeShiftCanceled?.Invoke(context);
    }
    private void HandleBodyShapeShiftPerformed(InputAction.CallbackContext context)
    {
        BodyShapeShiftPerformed?.Invoke(context);
    }

    private void HandleTime_spaceShiftPerformed(InputAction.CallbackContext context)
    {
        Time_spaceShiftPerformed?.Invoke(context);
    }

    private void HandleJumpPerformed(InputAction.CallbackContext context)
    {
        JumpPerformed?.Invoke(context);
    }

    private void HandleJumpCanceled(InputAction.CallbackContext context)
    {
        JumpCanceled?.Invoke(context);
    }

    private void HandleJumpStarted(InputAction.CallbackContext context)
    {
        JumpStarted?.Invoke(context);
    }
}
