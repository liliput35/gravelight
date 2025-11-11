using UnityEngine;

public class PageSfx : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioClip deathSFX;
    void Start()
    {
        deathAudioSource.PlayOneShot(deathSFX);
    }

}
