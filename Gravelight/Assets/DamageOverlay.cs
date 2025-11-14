using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageOverlay : MonoBehaviour
{
    public static DamageOverlay Instance;

    private Image img;
    private Coroutine flashRoutine;

    // Darkened red
    [SerializeField] private Color flashColor = new Color(75f / 255f, 24f / 255f, 24f / 255f);

    // Tweakable softness
    [SerializeField] private float fadeInTime = 0.12f;
    [SerializeField] private float fadeOutTime = 0.45f;
    [SerializeField] private float maxAlpha = 0.28f;

    private void Awake()
    {
        Instance = this;
        img = GetComponent<Image>();
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float t = 0;

        // Fade IN
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            img.color = new Color(
                flashColor.r,
                flashColor.g,
                flashColor.b,
                Mathf.Lerp(0f, maxAlpha, t / fadeInTime)
            );
            yield return null;
        }

        // Fade OUT
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            img.color = new Color(
                flashColor.r,
                flashColor.g,
                flashColor.b,
                Mathf.Lerp(maxAlpha, 0f, t / fadeOutTime)
            );
            yield return null;
        }

        img.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }
}
