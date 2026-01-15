using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("UI Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip buttonHover;

    [Header("Gameplay Sound Effects")]
    public AudioClip orbPlace;
    public AudioClip explosion;
    public AudioClip chainExplosion; // Slightly different for chain reactions
    public AudioClip winSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;

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
            return;
        }

        // Initialize audio sources
        if (musicSource == null || sfxSource == null)
        {
            CreateAudioSources();
        }
    }

    void Start()
    {
        // Load saved volume settings
        LoadVolumeSettings();

        // Apply volumes
        UpdateVolumes();

        // Start playing menu music
        PlayMusic(menuMusic);
    }

    void CreateAudioSources()
    {
        // Create music source
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // Create SFX source
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
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
            musicSource.Play();
        }
    }

    // Stop music
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    // Play a sound effect once
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip, volumeScale);
    }

    // Specific sound methods for easy calling
    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void PlayButtonHover()
    {
        PlaySFX(buttonHover, 0.5f);
    }

    public void PlayOrbPlace()
    {
        PlaySFX(orbPlace);
    }

    public void PlayExplosion()
    {
        PlaySFX(explosion);
    }

    public void PlayChainExplosion()
    {
        // Use different sound or slightly modified
        AudioClip clip = chainExplosion != null ? chainExplosion : explosion;
        PlaySFX(clip, 0.8f);
    }

    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }

    // Volume controls
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;

        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        SaveVolumeSettings();
    }

    public void ToggleMusic(bool enabled)
    {
        if (musicSource != null)
            musicSource.mute = !enabled;

        PlayerPrefs.SetInt("MusicEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFX(bool enabled)
    {
        if (sfxSource != null)
            sfxSource.mute = !enabled;

        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    void UpdateVolumes()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        bool sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        if (musicSource != null)
            musicSource.mute = !musicEnabled;

        if (sfxSource != null)
            sfxSource.mute = !sfxEnabled;
    }

    // Change music for different scenes
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }
}