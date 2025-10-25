using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 2f;
    public float hoverHeight = 4f;   // <- stays at this height

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private Rigidbody rb;
    private float nextAttackTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        hoverHeight = rb.position.y;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 myPos = rb.position;
        Vector3 playerPos = player.position;

        // --- Keep floating at hover height ---
        myPos.y = hoverHeight;

        // --- Move toward player horizontally ---
        Vector3 toPlayer = playerPos - myPos;
        toPlayer.y = 0; // ignore Y difference
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            Vector3 moveDir = toPlayer.normalized;
            Vector3 targetPos = myPos + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }

        // --- Rotate toward player ---
        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 5f * Time.fixedDeltaTime));
        }

        // --- Attack ---
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void AttackPlayer()
    {
        if (player.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
