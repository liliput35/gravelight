using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private float attackCooldown = 0.8f;
    private Animator _anim;

    private bool canAttack = true;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

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
        _anim.SetTrigger("attack");
        canAttack = false;


        // Cooldown before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void EnableHitbox() => swordHitbox.SetActive(true);
    public void DisableHitbox() => swordHitbox.SetActive(false);

}
