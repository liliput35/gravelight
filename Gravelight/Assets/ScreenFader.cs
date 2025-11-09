using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public IEnumerator FadeToWhite(float duration)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure fully white
        c.a = 1f;
        fadeImage.color = c;
    }

    public IEnumerator FadeFromWhite(float duration)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(1f, 0f, t);
            fadeImage.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent
        c.a = 0f;
        fadeImage.color = c;
    }
}
