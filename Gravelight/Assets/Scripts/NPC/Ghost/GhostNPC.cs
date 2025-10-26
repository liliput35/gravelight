using UnityEngine;

public class GhostNPC : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float changeTargetInterval = 3f;
    public float turnSmoothness = 2f;

    [Header("Wander Distance (Relative to Start)")]
    public float wanderRadius = 4f;
    public Vector2 yRange = new Vector2(1f, 4f);

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 currentTarget;
    private float timer;

    void Start()
    {
        startPos = transform.position;
        PickNewTarget();
        currentTarget = targetPos;
    }

    void Update()
    {
        // Smoothly blend towards target
        currentTarget = Vector3.Lerp(currentTarget, targetPos, Time.deltaTime * turnSmoothness);

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        Vector3 dir = currentTarget - transform.position;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 2f);
        }

        timer += Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPos) < 0.5f || timer >= changeTargetInterval)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        float randX = Random.Range(-wanderRadius, wanderRadius);
        float randZ = Random.Range(-wanderRadius, wanderRadius);
        float randY = Random.Range(yRange.x, yRange.y);

        targetPos = startPos + new Vector3(randX, randY, randZ);
        timer = 0f;
    }
}
