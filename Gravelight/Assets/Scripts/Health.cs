using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        Debug.Log("taking damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // Disable or destroy, play animation, etc.
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
