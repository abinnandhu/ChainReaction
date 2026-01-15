using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    RectTransform rectTransform;
    Rect lastSafeArea;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // Check if safe area changed (device rotation, etc.)
        if (lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;

        // Convert safe area to anchors
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        Debug.Log($"Safe area applied: {safeArea}");
    }
}