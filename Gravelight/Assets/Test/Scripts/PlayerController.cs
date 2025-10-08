using UnityEngine;
using UnityEngine.Playables;


[DefaultExecutionOrder(-1)]
public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [Header("Base Movement")]
    //public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    //public float drag = 0.1f;
    public float gravity = 25f;
    public float jumpSpeed = 1f;
    public float movingThreshold = 0.01f;
    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;

    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    private float _verticalVelocity = 0f;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        UpdateMovementState();
        HandleVerticalMovement();
        HandleLateralMovement();
    }

    private void UpdateMovementState()
    {
        bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();
        bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;
        bool isGrounded = IsGrounded();

        PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                            isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

        _playerState.SetPlayerMovementState(lateralState);

        if (!isGrounded && _characterController.velocity.y >= 0)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
        }
        else if (!isGrounded && _characterController.velocity.y < 0f)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
        }
    }

    private void HandleVerticalMovement()
    {
        bool isGrounded = _playerState.InGroundedState();

        if (isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = 0f;
        }

        _verticalVelocity -= gravity * Time.deltaTime;

        if (_playerLocomotionInput.JumpPressed && isGrounded)
        {
            _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
        }
    }

    /*private void HandleLateralMovement()
    {
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isGrounded = _playerState.InGroundedState();

        float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

        // ? Use world axes (no camera influence)
        Vector3 movementDirection =
            new Vector3(_playerLocomotionInput.MovementInput.x, 0f, _playerLocomotionInput.MovementInput.y);

        Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
        newVelocity.y += _verticalVelocity;

        _characterController.Move(newVelocity * Time.deltaTime);
    }*/

    private void HandleLateralMovement()
    {
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        float speed = isSprinting ? sprintSpeed : runSpeed;

        // Read movement input (normalized so diagonal movement isn’t faster)
        Vector3 inputDir = new Vector3(_playerLocomotionInput.MovementInput.x, 0f, _playerLocomotionInput.MovementInput.y);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        // Convert input to world movement
        Vector3 movement = inputDir * speed;

        // Apply gravity to vertical velocity
        movement.y = _verticalVelocity;

        // Move the character
        _characterController.Move(movement * Time.deltaTime);
    }


    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
        return lateralVelocity.magnitude > movingThreshold;
    }

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }
}

