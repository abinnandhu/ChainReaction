using UnityEngine;

public class CameraController : MonoBehaviour
{
    // This will help us adjust camera to fit the grid perfectly
    public static CameraController Instance;

    private Camera cam;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    // Call this to adjust camera size to fit game board
    public void AdjustCameraToFitGrid(float gridWidth, float gridHeight)
    {
        if (cam == null)
            return;

        // Calculate aspect ratio
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = gridWidth / gridHeight;

        if (screenRatio >= targetRatio)
        {
            // Screen is wider - fit to height
            cam.orthographicSize = gridHeight / 2f + 1f;
        }
        else
        {
            // Screen is taller - fit to width
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = gridHeight / 2f * differenceInSize + 1f;
        }
    }
}