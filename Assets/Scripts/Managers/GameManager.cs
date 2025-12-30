using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern - only one GameManager exists
    public static GameManager Instance;

    // Game settings
    public int numberOfPlayers = 2;
    public int currentPlayerIndex = 0;

    // Player colors for the game
    public Color[] playerColors = new Color[4]
    {
        new Color(1f, 0.2f, 0.2f),      // Red
        new Color(0.2f, 0.5f, 1f),      // Blue
        new Color(0.3f, 1f, 0.3f),      // Green
        new Color(1f, 0.9f, 0.2f)       // Yellow
    };

    void Awake()
    {
        // Singleton setup - ensures only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    // Call this to load the game scene
    public void LoadGameScene()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene("GameScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    // Call this to go back to menu
    public void LoadMenuScene()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene("MenuScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    // Set number of players before starting game
    public void SetNumberOfPlayers(int count)
    {
        numberOfPlayers = count;
        currentPlayerIndex = 0; // Reset to player 1
    }
}