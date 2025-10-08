using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;   // Assign your player here
    [SerializeField] private float followSpeed = 5f;  // Smooth follow speed

    private void LateUpdate()
    {
        if (player == null) return;

        // Follow player's X and Z, keep Y from the pivot
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Smoothly interpolate the position for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
