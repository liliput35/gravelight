using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GemCollection : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip collectSFX;
    private AudioSource audioSource;

    [Header("Visuals")]
    [SerializeField] private GameObject pickupEffect;

    private bool collected = false;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning($"{name} missing AudioSource child!");
        }
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        // Play collection SFX
        if (collectSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSFX);
            audioSource.transform.parent = null; // detach so sound plays after destroy
            Destroy(audioSource.gameObject, collectSFX.length);
        }


        // Disable visuals and collider
        var mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // Cleanup after
        Destroy(gameObject);
    }
}
