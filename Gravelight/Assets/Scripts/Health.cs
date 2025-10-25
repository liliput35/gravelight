using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Regen Settings")]
    public float regenDelay = 3f;      // wait 3 seconds before starting regen
    public float regenRate = 1f;       // heal 1 heart per second
    private float lastDamageTime;
    private bool isRegenerating = false;

    [Header("UI (for Player only)")]
    public Image heartDisplay;
    public Sprite fullHearts;
    public Sprite twoHearts;
    public Sprite oneHeart;
    public Sprite emptyHearts;

    private void Start()
    {
        currentHealth = maxHealth;

        if (gameObject.name == "PlayerLili" && heartDisplay == null)
        {
            heartDisplay = GameObject.Find("HealthBar")?.GetComponent<Image>();
        }

        UpdateHeartsUI();
    }

    private void Update()
    {
        if (gameObject.name == "PlayerLili")
        {
            TryRegen();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        lastDamageTime = Time.time; // reset regen timer
        isRegenerating = false;

        if (gameObject.name == "PlayerLili")
        {
            UpdateHeartsUI();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TryRegen()
    {
        if (currentHealth >= maxHealth || Time.time - lastDamageTime < regenDelay)
            return;

        if (!isRegenerating)
            StartCoroutine(RegenHealth());
    }

    private System.Collections.IEnumerator RegenHealth()
    {
        isRegenerating = true;

        while (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHeartsUI();
            yield return new WaitForSeconds(regenRate);

            // Stop if damaged during regen
            if (Time.time - lastDamageTime < regenDelay)
            {
                isRegenerating = false;
                yield break;
            }
        }

        isRegenerating = false;
    }

    private void UpdateHeartsUI()
    {
        if (heartDisplay == null) return;

        switch (currentHealth)
        {
            case 3:
                heartDisplay.sprite = fullHearts;
                break;
            case 2:
                heartDisplay.sprite = twoHearts;
                break;
            case 1:
                heartDisplay.sprite = oneHeart;
                break;
            default:
                heartDisplay.sprite = emptyHearts;
                break;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        gameObject.SetActive(false);
    }
}
 