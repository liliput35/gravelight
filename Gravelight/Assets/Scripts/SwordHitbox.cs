using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Health health))
                health.TakeDamage(damage);
        }
    }
}
