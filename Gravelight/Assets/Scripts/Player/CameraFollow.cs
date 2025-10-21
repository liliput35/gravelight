using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;   // Assign your player here
    [SerializeField] private float followSpeed = 5f;

    private Vector3 offset;  // stores initial distance between cameraNormal and player

    private void Start()
    {
        if (player != null)
            offset = transform.position - player.position; // record starting offset
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // target position = player position + original offset
        Vector3 targetPosition = player.position + offset;

        // smoothly move cameraNormal toward target
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
