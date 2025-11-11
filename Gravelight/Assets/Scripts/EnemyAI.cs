using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Detection")]
    public float detectRadius = 10f;     // start chasing when player is this close

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 2f;
    public float hoverHeight = 4f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private int pagesToSpawn = 6;
    [SerializeField] private float burstForce = 5f;


    private Rigidbody rb;
    private float nextAttackTime = 0f;

    private enum State { Idle, Chasing }
    private State currentState = State.Idle;

    private Health health;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        hoverHeight = transform.position.y;

        health = GetComponent<Health>();

        // When this enemy dies  spawn pages
        if (health != null)
        {
            health.OnDeath += SpawnPages;
        }

    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ----- STATE CHECK -----
        if (distance <= detectRadius)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Idle;
        }

        // ----- STATE BEHAVIOR -----
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior();
                break;

            case State.Chasing:
                ChaseBehavior(distance);
                break;
        }
    }

    // ------------------------------
    //           IDLE
    // ------------------------------
    private void IdleBehavior()
    {
        // Enemy stays still but keeps hovering at proper height
        Vector3 pos = rb.position;
        pos.y = hoverHeight;
        rb.MovePosition(pos);
    }

    // ------------------------------
    //           CHASING
    // ------------------------------
    private void ChaseBehavior(float distance)
    {
        Vector3 myPos = rb.position;
        Vector3 playerPos = player.position;

        // keep constant hover height
        myPos.y = hoverHeight;

        // direction to player
        Vector3 toPlayer = playerPos - myPos;
        toPlayer.y = 0;

        // rotate toward player
        if (toPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 7f * Time.fixedDeltaTime));
        }

        // move if outside stopDistance
        if (distance > stopDistance)
        {
            Vector3 moveDir = toPlayer.normalized;
            rb.MovePosition(myPos + moveDir * moveSpeed * Time.fixedDeltaTime);
        }

        // attack when close enough
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

    // show detection + attack ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void SpawnPages()
    {
        if (pagePrefab == null) return;

        for (int i = 0; i < pagesToSpawn; i++)
        {
            // random rotation
            Quaternion randomRot = Random.rotation;

            // spawn page
            GameObject page = Instantiate(
                pagePrefab,
                transform.position + Vector3.up * 0.5f,
                randomRot
            );

            // add burst force
            Rigidbody rb = page.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = Random.insideUnitSphere + Vector3.up * 1.5f;
                rb.AddForce(forceDir.normalized * burstForce, ForceMode.Impulse);

                rb.AddTorque(Random.insideUnitSphere * burstForce, ForceMode.Impulse);
            }
        }
    }


}
