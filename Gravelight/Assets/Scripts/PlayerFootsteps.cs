using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float walkStepInterval = 0.6f;
    [SerializeField] private float runStepInterval = 0.4f;
    [SerializeField] private float sprintStepInterval = 0.3f;

    private AudioSource audioSource;
    private float stepTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only play footsteps when grounded and moving
        if (playerState.InGroundedState())
        {
            float interval = GetCurrentStepInterval();

            if (interval > 0)
            {
                stepTimer += Time.deltaTime;

                if (stepTimer >= interval)
                {
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
        }
        else
        {
            stepTimer = 0f; // reset timer when not moving
        }
    }

    private float GetCurrentStepInterval()
    {
        switch (playerState.CurrentPlayerMovementState)
        {
            case PlayerMovementState.Walking:
                return walkStepInterval;
            case PlayerMovementState.Running:
                return runStepInterval;
            case PlayerMovementState.Sprinting:
                return sprintStepInterval;
            default:
                return 0f; // no footsteps for idle/fall/strafing
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;
        int index = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}
