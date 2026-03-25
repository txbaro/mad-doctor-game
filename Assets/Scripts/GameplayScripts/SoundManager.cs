using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip shootSound;
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

    public void PlayShootSound()
    {
        if(audioSource) audioSource.PlayOneShot(shootSound); 
    }
}