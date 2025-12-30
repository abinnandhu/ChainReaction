using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;

    [Header("Game State")]
    public int currentPlayerIndex = 0;
    public int numberOfPlayers = 2;
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool isProcessingTurn = false;

    [Header("Player Colors")]
    public Color[] playerColors = new Color[4]
    {
        new Color(1f, 0.2f, 0.2f),      // Red - Player 1
        new Color(0.2f, 0.5f, 1f),      // Blue - Player 2
        new Color(0.3f, 1f, 0.3f),      // Green - Player 3
        new Color(1f, 0.9f, 0.2f)       // Yellow - Player 4
    };

    // Track eliminated players
    private bool[] playerEliminated;
    private int turnCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Get number of players from GameManager
        if (GameManager.Instance != null)
        {
            numberOfPlayers = GameManager.Instance.numberOfPlayers;
        }

        // Initialize player tracking
        playerEliminated = new bool[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerEliminated[i] = false;
        }

        // Start with player 0
        currentPlayerIndex = 0;
        gameStarted = false;

        Debug.Log($"Game started with {numberOfPlayers} players");

        // Update UI with starting player
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTurnIndicator(currentPlayerIndex);
        }
    }

    // Called when a cell is clicked
    // Called when a cell is clicked
    // Called when a cell is clicked
    // Called when a cell is clicked
    // Called when a cell is clicked
    public void OnCellClicked(Cell cell)
    {
        // Check if game is over
        if (gameOver)
        {
            Debug.Log("Game is over!");
            return;
        }

        // NEW: Check if still processing previous turn
        if (isProcessingTurn)
        {
            Debug.Log("Wait for explosions to finish!");
            return;
        }

        // Check if player can place orb here
        if (!cell.CanAddOrb(currentPlayerIndex))
        {
            Debug.Log($"Player {currentPlayerIndex + 1} cannot place orb here!");
            return;
        }

        // Lock the game during explosions
        isProcessingTurn = true;

        // Place the orb
        cell.AddOrb(currentPlayerIndex);

        // Mark game as started
        if (!gameStarted)
            gameStarted = true;

        // Increase turn count
        turnCount++;

        // Wait for explosions to complete, then check game state
        Invoke("CheckGameState", 2.0f);
    }

    // Check if game is over or move to next turn
    // Check if game is over or move to next turn
    // Check if game is over (but DON'T change turn here)
    // Check if game is over or move to next turn
    void CheckGameState()
    {
        // Check for winner if enough turns have passed
        if (turnCount >= numberOfPlayers)
        {
            if (CheckForWinner())
            {
                return; // Game over
            }
        }

        // Move to next turn
        NextTurn();

        // Unlock the game - next player can now click
        isProcessingTurn = false;
    }


    // New method: Check winner after delay
    void CheckForWinnerDelayed()
    {
        if (CheckForWinner())
        {
            return; // Game over
        }

        // No winner yet, continue game
        NextTurn();
    }

    // Move to next player's turn
    // Move to next player's turn
    void NextTurn()
    {
        // Find next non-eliminated player
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        }
        while (playerEliminated[currentPlayerIndex] && !gameOver);

        Debug.Log($"Now Player {currentPlayerIndex + 1}'s turn");

        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTurnIndicator(currentPlayerIndex);
        }
    }
    // Check if there's a winner
    bool CheckForWinner()
    {
        // Count how many cells each player owns
        Dictionary<int, int> playerCellCounts = new Dictionary<int, int>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerCellCounts[i] = 0;
        }

        // Count cells for each player
        foreach (Cell cell in GridManager.Instance.grid)
        {
            if (cell.ownerIndex != -1)
            {
                playerCellCounts[cell.ownerIndex]++;
            }
        }

        // Check for eliminated players
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (playerCellCounts[i] == 0 && !playerEliminated[i])
            {
                playerEliminated[i] = true;
                Debug.Log($"Player {i + 1} eliminated!");
            }
        }

        // Count active players
        int activePlayers = 0;
        int winnerIndex = -1;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (!playerEliminated[i])
            {
                activePlayers++;
                winnerIndex = i;
            }
        }

        // Check if only one player left
        if (activePlayers == 1)
        {
            GameOver(winnerIndex);
            return true;
        }

        return false;
    }

    // End the game
    // End the game
    // End the game
    void GameOver(int winnerIndex)
    {
        gameOver = true;
        isProcessingTurn = false;
        Debug.Log($"GAME OVER! Player {winnerIndex + 1} wins!");

        // Show win screen
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowWinPanel(winnerIndex);
        }
    }

    // Get color for player
    public Color GetPlayerColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerColors.Length)
            return playerColors[playerIndex];
        return Color.white;
    }

    // Restart game
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}