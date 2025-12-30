using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Turn Indicator")]
    public TextMeshProUGUI turnText;
    public Image playerColorIndicator;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;

    [Header("Win Screen")]
    public TextMeshProUGUI winnerText;

    [Header("Pause Button")]
    public Button pauseButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Hide panels at start
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        // Update turn indicator
        UpdateTurnIndicator(0);
    }

    // Update turn indicator with current player
    public void UpdateTurnIndicator(int playerIndex)
    {
        if (turnText != null)
        {
            turnText.text = $"Player {playerIndex + 1}'s Turn";

            // Get player color from GamePlayManager
            if (GamePlayManager.Instance != null)
            {
                Color playerColor = GamePlayManager.Instance.GetPlayerColor(playerIndex);
                turnText.color = playerColor;

                // Update color indicator
                if (playerColorIndicator != null)
                {
                    playerColorIndicator.color = playerColor;
                }
            }
        }
    }

    // Show pause panel
    public void ShowPausePanel()
    {
        if (pausePanel != null)
        {
            StartCoroutine(FadeInPanel(pausePanel));
            Time.timeScale = 0;
        }
    }

    // Hide pause panel
    public void HidePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1; // Resume game
        }
    }

    // Show win panel
    public void ShowWinPanel(int winnerIndex)
    {
        if (winPanel != null && winnerText != null)
        {
            // Get player color
            Color playerColor = Color.white;
            if (GamePlayManager.Instance != null)
            {
                playerColor = GamePlayManager.Instance.GetPlayerColor(winnerIndex);
            }

            // Update winner text
            winnerText.text = $"PLAYER {winnerIndex + 1} WINS!";
            winnerText.color = playerColor;

            // Show panel
            winPanel.SetActive(true);
            StartCoroutine(AnimateWinText());
        }
    }

    // Button: Resume game
    public void OnResumeClicked()
    {
        HidePausePanel();
    }

    // Button: Restart game
    public void OnRestartClicked()
    {
        Time.timeScale = 1; // Make sure time is running

        if (GamePlayManager.Instance != null)
        {
            GamePlayManager.Instance.RestartGame();
        }
    }

    // Button: Main menu
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1; // Make sure time is running

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadMenuScene();
        }
    }

    // Button: Pause button clicked
    public void OnPauseButtonClicked()
    {
        ShowPausePanel();
    }

    // Enable/Disable pause button
    public void SetPauseButtonEnabled(bool enabled)
    {
        if (pauseButton != null)
        {
            pauseButton.interactable = enabled;
        }
    }
    IEnumerator FadeInPanel(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        cg.alpha = 0;
        panel.SetActive(true);

        while (cg.alpha < 1)
        {
            cg.alpha += Time.unscaledDeltaTime * 3f;
            yield return null;
        }
    }

    // Add this method to UIManager
    IEnumerator AnimateWinText()
    {
        if (winnerText != null)
        {
            float elapsed = 0f;
            float duration = 0.5f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;

            winnerText.transform.localScale = startScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;

                // Bounce effect
                float bounce = progress * progress * ((1.7f + 1f) * progress - 1.7f) + 1f;

                winnerText.transform.localScale = endScale * bounce;

                yield return null;
            }

            winnerText.transform.localScale = endScale;
        }
    }

    

}