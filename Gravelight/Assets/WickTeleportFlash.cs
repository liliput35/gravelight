using UnityEngine;
using System.Collections;

public class WickTeleportFlash : MonoBehaviour
{
    [SerializeField] private Light wickLight;
    [SerializeField] private float maxIntensity = 12f;
    [SerializeField] private float maxRange = 4f;
    [SerializeField] private float flashDuration = 0.5f;

    public IEnumerator PlayFlash()
    {
        float startIntensity = wickLight.intensity;
        float startRange = wickLight.range;

        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = timer / flashDuration;

            // Smooth curve (ease-in)
            t = Mathf.SmoothStep(0, 1, t);

            wickLight.intensity = Mathf.Lerp(startIntensity, maxIntensity, t);
            wickLight.range = Mathf.Lerp(startRange, maxRange, t);

            yield return null;
        }
    }
}
