using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private Camera cam;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        SetupCamera();
    }

    void SetupCamera()
    {
        if (cam == null)
            return;

        // Get actual screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float aspectRatio = screenWidth / screenHeight;

        Debug.Log($"Device: {screenWidth}x{screenHeight}, Aspect: {aspectRatio}");

        // REDUCED camera sizes - shows less area = grid looks bigger
        if (aspectRatio < 0.5f) // Very tall screens
        {
            cam.orthographicSize = 5.5f;  // Reduced from 6.5f
        }
        else if (aspectRatio < 0.6f) // Tall screens
        {
            cam.orthographicSize = 5.0f;  // Reduced from 6f
        }
        else if (aspectRatio < 0.7f) // Standard phones
        {
            cam.orthographicSize = 4.5f;  // Reduced from 5.5f
        }
        else // Tablets
        {
            cam.orthographicSize = 4.0f;  // Reduced from 5f
        }

        Debug.Log($"Camera size: {cam.orthographicSize}");
    }
}