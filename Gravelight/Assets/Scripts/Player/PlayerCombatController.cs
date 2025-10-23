using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private float hitboxActiveTime = 0.2f;

    private bool canAttack = true;

    void Update()
    {
        // New Input System check for Q key pressed this frame
        if (Keyboard.current.qKey.wasPressedThisFrame && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // Enable sword hitbox for a short time
        swordHitbox.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        swordHitbox.SetActive(false);

        // Cooldown before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
