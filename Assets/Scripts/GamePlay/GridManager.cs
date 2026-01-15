using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int rows = 8;
    public int columns = 6;

    [Header("Prefab")]
    public GameObject cellPrefab;

    // Calculated values
    private float cellSize;
    private float cellSpacing = 0.08f;

    // 2D array to store all cells
    public Cell[,] grid;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    // Create the entire grid
    void GenerateGrid()
    {
        // Calculate cell size based on screen
        CalculateCellSize();

        // Initialize array
        grid = new Cell[columns, rows];

        // Create each cell
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Calculate position
                Vector3 position = new Vector3(
                    x * (cellSize + cellSpacing),
                    y * (cellSize + cellSpacing),
                    0
                );

                // Create cell
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity);
                cellObj.transform.SetParent(transform);
                cellObj.name = $"Cell ({x},{y})";

                // Get Cell component and initialize
                Cell cell = cellObj.GetComponent<Cell>();
                int cellCapacity = GetCellCapacity(x, y);
                cell.Initialize(x, y, cellCapacity);

                // Store in array
                grid[x, y] = cell;
            }
        }

        // Center and scale the grid
        FitGridToScreen();
    }

    // Calculate optimal cell size
    void CalculateCellSize()
    {
        // Start with a base size
        cellSize = 1f;
    }

    // Fit entire grid to screen
    // Fit entire grid to screen
    void FitGridToScreen()
    {
        // Calculate grid dimensions
        float gridWidth = (columns - 1) * (cellSize + cellSpacing);
        float gridHeight = (rows - 1) * (cellSize + cellSpacing);

        // Get screen dimensions in world units
        Camera cam = Camera.main;
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        // REDUCED margins - fill more screen space
        float topUISpace = 1.2f;      // Reduced
        float bottomUISpace = 0.3f;   // Reduced
        float sideMargin = 0.3f;      // Reduced

        float availableHeight = screenHeight - topUISpace - bottomUISpace;
        float availableWidth = screenWidth - (sideMargin * 2f);

        // Calculate scale factors
        float scaleX = availableWidth / gridWidth;
        float scaleY = availableHeight / gridHeight;

        // Use smaller scale to ensure grid fits
        float scale = Mathf.Min(scaleX, scaleY);

        // REMOVE or increase the clamp - let grid be bigger
        scale = Mathf.Clamp(scale, 0.7f, 2.5f); // Allow larger scale

        // Apply scale to grid
        transform.localScale = Vector3.one * scale;

        // Center the grid
        float centerX = gridWidth / 2f;
        float centerY = gridHeight / 2f;

        // Position accounting for UI - better centering
        float yOffset = (topUISpace - bottomUISpace) / 2f / scale;

        transform.position = new Vector3(-centerX, -centerY - yOffset, 0) * scale;

        Debug.Log($"Screen: {Screen.width}x{Screen.height}, Scale: {scale}");
        Debug.Log($"Grid position: {transform.position}, Scale: {transform.localScale}");
    }

    // Determine capacity based on position
    int GetCellCapacity(int x, int y)
    {
        bool isCorner = (x == 0 || x == columns - 1) && (y == 0 || y == rows - 1);
        bool isEdge = x == 0 || x == columns - 1 || y == 0 || y == rows - 1;

        if (isCorner) return 1;
        if (isEdge) return 2;
        return 3;
    }

    // Get all neighboring cells
    public List<Cell> GetNeighbors(int x, int y)
    {
        List<Cell> neighbors = new List<Cell>();

        if (x > 0) neighbors.Add(grid[x - 1, y]);
        if (x < columns - 1) neighbors.Add(grid[x + 1, y]);
        if (y > 0) neighbors.Add(grid[x, y - 1]);
        if (y < rows - 1) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    // Get cell at position
    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
            return grid[x, y];
        return null;
    }
}