using UnityEngine;

public class CellHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Cell cell;

    private Color hoverColor = new Color(1f, 1f, 1f, 0.3f);

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cell = GetComponent<Cell>();
    }

    void OnMouseEnter()
    {
        // Only highlight if player can place orb here
        if (GamePlayManager.Instance != null && cell != null)
        {
            int currentPlayer = GamePlayManager.Instance.currentPlayerIndex;

            if (cell.CanAddOrb(currentPlayer) && !GamePlayManager.Instance.gameOver)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = originalColor + hoverColor;
            }
        }
    }

    void OnMouseExit()
    {
        // Return to original color
        if (GamePlayManager.Instance != null && cell != null)
        {
            int currentPlayer = GamePlayManager.Instance.currentPlayerIndex;
            Color playerColor = GamePlayManager.Instance.GetPlayerColor(cell.ownerIndex);
            Color emptyColor = new Color(0.8f, 0.8f, 0.8f);

            if (cell.ownerIndex == -1)
            {
                spriteRenderer.color = emptyColor;
            }
            else
            {
                spriteRenderer.color = playerColor * 0.4f + emptyColor * 0.6f;
            }
        }
    }
}