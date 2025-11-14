using UnityEngine;
using System.Collections;

public class EnemyFlash : MonoBehaviour
{
    public Material flashMaterial;            // red flash material
    public float flashDuration = 0.1f;        // quick hit flash

    private Material originalMaterial;
    private Renderer rend;
    private Coroutine flashRoutine;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
            originalMaterial = rend.material;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        rend.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        rend.material = originalMaterial;
    }
}
