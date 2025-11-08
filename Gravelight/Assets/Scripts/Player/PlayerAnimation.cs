using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float locomotionBlendSpeed = 8f; // Smooth transition speed

    private PlayerState _playerState;
    private PlayerLocomotionInput _playerLocomotionInput;

    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    private float _currentSpeed = 0f;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // Determine if player is moving
        bool isMoving = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running ||
                        _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

        // Smooth transition for running animation
        float targetSpeed = isMoving ? 1f : 0f; // 1 = running, 0 = idle
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, locomotionBlendSpeed * Time.deltaTime);

        _animator.SetFloat(IsRunningHash, _currentSpeed);
    }
}
