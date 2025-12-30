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
        orbContainer = container.transform;
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
    // Add an orb to this cell
    public void AddOrb(int playerIndex)
    {
        // Set owner
        ownerIndex = playerIndex;

        // Increase orb count
        orbCount++;

        // Update visual
        UpdateVisual();

        // Check if we need to explode
        if (orbCount > capacity)
        {
            // Explode after short delay
            Invoke("Explode", 0.5f);  // Changed from 0.3f to 0.5f
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

    // Create a single orb
    // Create a single orb (2D version)
    void CreateOrb(int index, Color color)
    {
        // Create 3D sphere
        GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orb.transform.SetParent(orbContainer);
        orb.transform.localScale = Vector3.one * 0.3f;

        // Remove collider
        Collider collider = orb.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);

        // Get renderer and apply material
        Renderer renderer = orb.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Load the orb material
            Material orbMaterial = Resources.Load<Material>("OrbMaterial");
            if (orbMaterial != null)
            {
                renderer.material = new Material(orbMaterial); // Create instance
                renderer.material.color = color;
            }
            else
            {
                // Fallback to default material
                renderer.material.color = color;
            }
        }

        // Position
        orb.transform.localPosition = GetOrbPosition(index, orbCount);

        // Add animator and play animation
        OrbAnimator animator = orb.AddComponent<OrbAnimator>();
        animator.AnimateSpawn();
        // Add point light (optional - for extra polish)
        Light light = orb.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.range = 1f;
        light.intensity = 2f;

        // Add to list
        orbs.Add(orb);

        // Add to list
        orbs.Add(orb);
    }

    // Get position for orb based on count
    Vector3 GetOrbPosition(int index, int total)
    {
        switch (total)
        {
            case 1:
                return Vector3.zero; // Center

            case 2:
                // Two orbs side by side
                return new Vector3(index == 0 ? -0.2f : 0.2f, 0, -0.1f);

            case 3:
                // Three orbs in triangle
                if (index == 0) return new Vector3(-0.2f, 0.15f, -0.1f);
                if (index == 1) return new Vector3(0.2f, 0.15f, -0.1f);
                return new Vector3(0, -0.15f, -0.1f);

            default:
                return Vector3.zero;
        }
    }

    // Cell explodes and sends orbs to neighbors
    // Cell explodes and sends orbs to neighbors
    // Cell explodes and sends orbs to neighbors
    void Explode()
    {
        // Prevent multiple explosions
        if (isExploding)
            return;

        isExploding = true;

        Debug.Log($"Cell ({gridX},{gridY}) exploding!");

        // Store owner before resetting
        int explosionOwner = ownerIndex;
        Color explosionColor = GamePlayManager.Instance.GetPlayerColor(explosionOwner);

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