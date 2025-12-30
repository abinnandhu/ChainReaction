using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Sound Effect Clips")]
    public AudioClip buttonClick;
    public AudioClip orbPlace;
    public AudioClip explosion;
    public AudioClip winSound;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start playing menu music if available
        if (musicSource != null && menuMusic != null)
        {
            PlayMusic(menuMusic);
        }
    }

    // Play background music
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        // Only change if different music
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Play a sound effect once
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    // Play button click sound
    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    // Stop all audio
    public void StopAllAudio()
    {
        if (musicSource != null)
            musicSource.Stop();
        if (sfxSource != null)
            sfxSource.Stop();
    }
}