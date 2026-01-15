using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Settings State")]
    public bool soundEnabled = true;
    public bool musicEnabled = true;
    public bool vibrationEnabled = true;

    [Header("UI References (Optional - assign if needed)")]
    public Toggle soundToggle;
    public Toggle musicToggle;
    public Toggle vibrationToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    void Awake()
    {
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

        LoadSettings();
    }

    void Start()
    {
        // Update UI if references are assigned
        UpdateUI();
    }

    public void ToggleSound(bool enabled)
    {
        soundEnabled = enabled;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX(enabled);
        }

        SaveSettings();
    }

    public void ToggleMusic(bool enabled)
    {
        musicEnabled = enabled;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic(enabled);
        }

        SaveSettings();
    }

    public void ToggleVibration(bool enabled)
    {
        vibrationEnabled = enabled;
        SaveSettings();
    }

    public void SetMusicVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("VibrationEnabled", vibrationEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
    }

    void UpdateUI()
    {
        if (soundToggle != null)
            soundToggle.isOn = soundEnabled;

        if (musicToggle != null)
            musicToggle.isOn = musicEnabled;

        if (vibrationToggle != null)
            vibrationToggle.isOn = vibrationEnabled;

        if (musicVolumeSlider != null && AudioManager.Instance != null)
            musicVolumeSlider.value = AudioManager.Instance.musicVolume;

        if (sfxVolumeSlider != null && AudioManager.Instance != null)
            sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
    }
}