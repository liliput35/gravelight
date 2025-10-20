using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private bool holdToSprint = true;

    public bool SprintToggledOn { get; private set; }
    public InputSystem_Actions InputActions { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }

    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        JumpPressed = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // REMOVED - no longer using LookInput
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        JumpPressed = true;
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintToggledOn = holdToSprint || !SprintToggledOn;
        }
        else if (context.canceled)
        {
            SprintToggledOn = !holdToSprint && SprintToggledOn;
        }
    }
}
