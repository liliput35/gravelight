using UnityEngine;

public class WickFollower : MonoBehaviour
{
    public Transform player;          // assign in inspector
    public float followDistance = 2f; // how far behind player it stays
    public float hopHeight = 1f;      // how high it jumps
    public float hopSpeed = 4f;       // move speed
    public float hopInterval = 0.7f;  // delay between hops

    private bool isHopping = false;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (!isHopping && distance > followDistance)
        {
            StartCoroutine(HopTowardsPlayer());
        }
    }

    private System.Collections.IEnumerator HopTowardsPlayer()
    {
        isHopping = true;

        Vector3 startPos = transform.position;

        // Move only a short step toward the player instead of the full distance
        Vector3 dir = (player.position - startPos).normalized;
        float stepDistance = 1.5f; // tweak this for shorter/longer hops
        Vector3 targetPos = startPos + dir * stepDistance;
        targetPos.y = startPos.y; // stay on ground level

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * hopSpeed;

            // Move forward
            Vector3 flatPos = Vector3.Lerp(startPos, targetPos, t);

            // Add arc (parabola)
            float yOffset = Mathf.Sin(t * Mathf.PI) * hopHeight;

            transform.position = new Vector3(flatPos.x, flatPos.y + yOffset, flatPos.z);

            yield return null;
        }

        isHopping = false;
        yield return new WaitForSeconds(hopInterval);
    }


}
