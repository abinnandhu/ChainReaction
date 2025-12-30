using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int rows = 8;
    public int columns = 6;
    public float cellSize = 1f;
    public float cellSpacing = 0.1f;

    [Header("Prefab")]
    public GameObject cellPrefab;

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
                cellObj.transform.SetParent(transform); // Parent to Grid object
                cellObj.name = $"Cell ({x},{y})"; // Nice name for debugging

                // Get Cell component and initialize
                Cell cell = cellObj.GetComponent<Cell>();
                int cellCapacity = GetCellCapacity(x, y);
                cell.Initialize(x, y, cellCapacity);

                // Store in array
                grid[x, y] = cell;
            }
        }

        // Center the grid on screen
        CenterGrid();
    }

    // Determine capacity based on position
    int GetCellCapacity(int x, int y)
    {
        bool isCorner = (x == 0 || x == columns - 1) && (y == 0 || y == rows - 1);
        bool isEdge = x == 0 || x == columns - 1 || y == 0 || y == rows - 1;

        if (isCorner) return 1;      // Corners: 1 orb capacity
        if (isEdge) return 2;         // Edges: 2 orb capacity
        return 3;                     // Center: 3 orb capacity
    }

    // Center grid in camera view
    void CenterGrid()
    {
        float gridWidth = (columns - 1) * (cellSize + cellSpacing);
        float gridHeight = (rows - 1) * (cellSize + cellSpacing);

        float centerX = gridWidth / 2f;
        float centerY = gridHeight / 2f;

        // Move grid so it's centered at (0,0)
        transform.position = new Vector3(-centerX, -centerY, 0);

        // Tell camera to adjust
        if (CameraController.Instance != null)
        {
            CameraController.Instance.AdjustCameraToFitGrid(gridWidth, gridHeight);
        }
    }

    // Get all neighboring cells
    public List<Cell> GetNeighbors(int x, int y)
    {
        List<Cell> neighbors = new List<Cell>();

        // Check left
        if (x > 0)
            neighbors.Add(grid[x - 1, y]);

        // Check right
        if (x < columns - 1)
            neighbors.Add(grid[x + 1, y]);

        // Check down
        if (y > 0)
            neighbors.Add(grid[x, y - 1]);

        // Check up
        if (y < rows - 1)
            neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    // Get cell at position (for debugging)
    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
            return grid[x, y];
        return null;
    }
}