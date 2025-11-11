using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public System.Action OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;

        if (gameObject.name == "PlayerLiliAnimated" && heartDisplay == null)
        {
            heartDisplay = GameObject.Find("HealthBar")?.GetComponent<Image>();
        }

        UpdateHeartsUI();
    }

    private void Update()
    {
        if (gameObject.name == "PlayerLiliAnimated")
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

        if (gameObject.name == "PlayerLiliAnimated")
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

        OnDeath?.Invoke();


        if (gameObject.name == "PlayerLiliAnimated")
        {
            StartCoroutine(DeathSequence());
        }
        else if (gameObject.tag == "Enemy")
        {
            
            gameObject.SetActive(false);
            

        }
    }

    private IEnumerator DeathSequence()
    {
        // Disable player input immediately
        var playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();
        if (playerInput != null)
            playerInput.actions.Disable();

        // Fade screen to black (0.8s recommended)
        yield return ScreenFader.Instance.FadeToWhite(0.8f);

        // Small pause for dramatic effect
        yield return new WaitForSeconds(0.2f);

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
 