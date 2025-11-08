using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background; 


    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    
}
