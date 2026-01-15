using UnityEngine;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    [Header("Cell Properties")]
    public int gridX;           // Position in grid
    public int gridY;
    public int capacity;        // Max orbs before explosion (1, 2, or 3)
    public int orbCount = 0;    // Current number of orbs
    public int ownerIndex = -1; // Which player owns this cell (-1 = empty)
    private bool isExploding = false;
    [Header("Visual Components")]
    public SpriteRenderer spriteRenderer;
    public Transform orbContainer;

    [Header("Orb Prefab")]
    public GameObject orbPrefab;

    // List of orb objects currently in this cell
    private List<GameObject> orbs = new List<GameObject>();

    // Colors
    private Color emptyColor = new Color(0.8f, 0.8f, 0.8f); // Light gray

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create container for orbs
        GameObject container = new GameObject("OrbContainer");
        container.transform.SetParent(transform);
        container.transform.localPosition = Vector3.zero;
        container.transform.localScale = Vector3.one;
        orbContainer = container.transform;

        Debug.Log($"Cell ({name}) orbContainer created at: {orbContainer.position}");
    }

    // Called by GridManager to set up cell
    public void Initialize(int x, int y, int cellCapacity)
    {
        gridX = x;
        gridY = y;
        capacity = cellCapacity;

        // Set initial color
        spriteRenderer.color = emptyColor;
    }

    // Called when player clicks this cell
    void OnMouseDown()
    {
        // Tell GamePlayManager that this cell was clicked
        if (GamePlayManager.Instance != null)
        {
            GamePlayManager.Instance.OnCellClicked(this);
        }
    }

    // Add an orb to this cell
    public void AddOrb(int playerIndex)
    {
        // Set owner
        ownerIndex = playerIndex;

        // Increase orb count
        orbCount++;

        // Play orb placement sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOrbPlace();
        }

        // Update visual
        UpdateVisual();

        // Check if we need to explode
        if (orbCount > capacity)
        {
            Invoke("Explode", 0.5f);
        }
    }

    // Update the cell's appearance
    void UpdateVisual()
    {
        // Get player color
        Color playerColor = GamePlayManager.Instance.GetPlayerColor(ownerIndex);

        // Color the cell background
        Color cellColor = playerColor * 0.4f + emptyColor * 0.6f;

        // If cell is at capacity, make it glow (pulse)
        if (orbCount == capacity && ownerIndex != -1)
        {
            // Add pulsing glow effect
            float pulse = Mathf.PingPong(Time.time * 2f, 1f);
            cellColor = Color.Lerp(cellColor, playerColor, pulse * 0.5f);
        }

        spriteRenderer.color = cellColor;

        // Destroy old orbs with animation
        foreach (GameObject orb in orbs)
        {
            OrbAnimator animator = orb.GetComponent<OrbAnimator>();
            if (animator != null)
            {
                animator.AnimateDestroy();
            }
            else
            {
                Destroy(orb);
            }
        }
        orbs.Clear();

        // Create new orbs
        for (int i = 0; i < orbCount; i++)
        {
            CreateOrb(i, playerColor);
        }
    }

    void CreateOrb(int index, Color color)
    {
        // Create 2D sprite orb
        GameObject orb = new GameObject($"Orb_{index}");
        orb.transform.SetParent(orbContainer);

        // Add sprite renderer
        SpriteRenderer sr = orb.AddComponent<SpriteRenderer>();

        // Create a circle sprite programmatically
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float dx = (x - 16) / 16f;
                float dy = (y - 16) / 16f;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);

                if (dist <= 1f)
                {
                    pixels[y * 32 + x] = color;
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();

        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        sr.sortingOrder = 10; // In front of cells

        // Scale
        orb.transform.localScale = Vector3.one * 0.5f;

        // Position
        orb.transform.localPosition = GetOrbPosition(index, orbCount);

        // Add animator
        OrbAnimator animator = orb.AddComponent<OrbAnimator>();
        if (animator != null)
        {
            animator.AnimateSpawn();
        }

        orbs.Add(orb);
    }

    // Get position for orb based on count
    Vector3 GetOrbPosition(int index, int total)
    {
        Vector3 pos = Vector3.zero;

        switch (total)
        {
            case 1:
                pos = Vector3.zero;
                break;

            case 2:
                pos = new Vector3(index == 0 ? -0.2f : 0.2f, 0, 0);
                break;

            case 3:
                if (index == 0) pos = new Vector3(-0.2f, 0.15f, 0);
                else if (index == 1) pos = new Vector3(0.2f, 0.15f, 0);
                else pos = new Vector3(0, -0.15f, 0);
                break;

            default:
                pos = Vector3.zero;
                break;
        }

        // CRITICAL: Move orbs in front of cell (negative Z in 2D)
        pos.z = -0.5f;

        return pos;
    }

    // Cell explodes and sends orbs to neighbors
    // Cell explodes and sends orbs to neighbors
    // Cell explodes and sends orbs to neighbors
    void Explode()
    {
        if (isExploding)
            return;

        isExploding = true;

        Debug.Log($"Cell ({gridX},{gridY}) exploding!");

        // Store owner before resetting
        int explosionOwner = ownerIndex;
        Color explosionColor = GamePlayManager.Instance.GetPlayerColor(explosionOwner);

        // Play explosion sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayExplosion();
        }

        // Play explosion effect
        if (ExplosionManager.Instance != null)
        {
            Vector3 worldPos = transform.position;
            ExplosionManager.Instance.PlayExplosion(worldPos, explosionColor);
        }

        // Shake camera
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.2f, 0.15f);
        }

        if (VibrationManager.Instance != null)
        {
            VibrationManager.Instance.VibrateMedium();
        }
        // Reset this cell completely
        orbCount = 0;
        ownerIndex = -1;

        UpdateVisual();

        // Get neighbors from GridManager
        List<Cell> neighbors = GridManager.Instance.GetNeighbors(gridX, gridY);

        // Send orbs to neighbors with small delay between each
        float delay = 0f;
        foreach (Cell neighbor in neighbors)
        {
            int capturedOwner = explosionOwner;
            StartCoroutine(AddOrbWithDelay(neighbor, capturedOwner, delay));
            delay += 0.1f;
        }

        // Reset explosion flag after delay
        Invoke("ResetExplosionFlag", 1f);
    }
    // Reset explosion flag
    void ResetExplosionFlag()
    {
        isExploding = false;
    }

    // Add orb with delay
    System.Collections.IEnumerator AddOrbWithDelay(Cell cell, int playerIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        cell.AddOrb(playerIndex);
    }

    // Check if player can place orb here
    public bool CanAddOrb(int playerIndex)
    {
        // Can add if cell is empty or owned by same player
        return ownerIndex == -1 || ownerIndex == playerIndex;
    }
}