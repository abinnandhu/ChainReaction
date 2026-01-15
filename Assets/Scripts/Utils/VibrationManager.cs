using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // Light vibration (for button clicks)
    public void VibrateLight()
    {
        if (SettingsManager.Instance != null && SettingsManager.Instance.vibrationEnabled)
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }

    // Medium vibration (for explosions)
    public void VibrateMedium()
    {
        if (SettingsManager.Instance != null && SettingsManager.Instance.vibrationEnabled)
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }

    // Heavy vibration (for chain reactions)
    public void VibrateHeavy()
    {
        if (SettingsManager.Instance != null && SettingsManager.Instance.vibrationEnabled)
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }
}