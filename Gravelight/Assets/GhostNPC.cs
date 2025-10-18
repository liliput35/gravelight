using UnityEngine;

public class GhostNPC : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float changeTargetInterval = 3f; // seconds before picking a new spot

    [Header("Wander Area (World Space)")]
    public Vector2 xRange = new Vector2(-20f, 0f);
    public Vector2 zRange = new Vector2(0f, 15f);
    public Vector2 yRange = new Vector2(1f, 4f); // floating height range

    private Vector3 targetPos;
    private float timer;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        // Move ghost toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Smooth rotation to face direction
        Vector3 dir = targetPos - transform.position;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 2f);
        }

        // If close or time passed, pick a new target
        timer += Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPos) < 0.5f || timer >= changeTargetInterval)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        float randX = Random.Range(xRange.x, xRange.y);
        float randZ = Random.Range(zRange.x, zRange.y);
        float randY = Random.Range(yRange.x, yRange.y);

        targetPos = new Vector3(randX, randY, randZ);
        timer = 0f;
    }
}
