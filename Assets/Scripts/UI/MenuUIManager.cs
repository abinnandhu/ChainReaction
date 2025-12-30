using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject playerSelectionPanel;
    public GameObject settingsPanel;

    void Start()
    {
        // Start by showing main menu
        ShowMainMenu();
    }

    // Show main menu, hide others
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        playerSelectionPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Play menu music
        if (AudioManager.Instance != null && AudioManager.Instance.menuMusic != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        }
    }

    // Show player selection panel
    public void ShowPlayerSelection()
    {
        mainMenuPanel.SetActive(false);
        playerSelectionPanel.SetActive(true);

        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }

    // Show settings panel
    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
}