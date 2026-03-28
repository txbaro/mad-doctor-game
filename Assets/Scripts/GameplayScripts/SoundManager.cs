using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>(); 
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); 
        }
    }

    public void PlayLoopingSound(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying) return; 

        audioSource.clip = clip;
        audioSource.loop = true; 
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}