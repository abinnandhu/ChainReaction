using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
    // Called when player clicks 2, 3, or 4 player button
    public void SelectPlayers(int count)
    {
        if (count < 2 || count > 4)
        {
            Debug.LogError("Invalid player count: " + count);
            return;
        }

        // Play button sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Set number in GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetNumberOfPlayers(count);
            Invoke("LoadGameScene", 0.2f);
        }
    }

    // Load the game scene
    void LoadGameScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGameScene();
        }
    }

    // Go back to main menu
    public void BackToMainMenu()
    {
        // Play button sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // Find MenuUIManager and show main menu
        MenuUIManager menuManager = FindObjectOfType<MenuUIManager>();
        if (menuManager != null)
        {
            menuManager.ShowMainMenu();
        }
    }
}