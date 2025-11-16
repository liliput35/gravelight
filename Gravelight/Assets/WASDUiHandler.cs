using UnityEngine;
using TMPro;

public class WASDUiHandler : MonoBehaviour
{
    public float delayBeforeFade = 3f;
    public float fadeDuration = 1.5f;

    private TextMeshProUGUI tmpText;
    private float startAlpha;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();

        // Store starting alpha (e.g., 0.6 if 60%)
        startAlpha = tmpText.color.a;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // After delay, start fading
        if (!fading && timer >= delayBeforeFade)
        {
            fading = true;
        }

        // Perform fade
        if (fading)
        {
            float t = (timer - delayBeforeFade) / fadeDuration;

            Color c = tmpText.color;
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            tmpText.color = c;

            if (t >= 1f)
            {
                gameObject.SetActive(false); // optional
            }
        }
    }
}
