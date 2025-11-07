using UnityEngine;


public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; set; } = PlayerMovementState.Idling;

    public void SetPlayerMovementState(PlayerMovementState playerMovementState)
    {
        CurrentPlayerMovementState = playerMovementState;
    }

    public bool InGroundedState()
    {
        return CurrentPlayerMovementState == PlayerMovementState.Idling ||
                CurrentPlayerMovementState == PlayerMovementState.Walking ||
                CurrentPlayerMovementState == PlayerMovementState.Running ||
                CurrentPlayerMovementState == PlayerMovementState.Sprinting;
    }
        
}

public enum PlayerMovementState
{
    Idling = 0,
    Walking = 1,
    Running = 2,
    Sprinting = 3,
    Falling = 4,
    Strafing = 5,
}


