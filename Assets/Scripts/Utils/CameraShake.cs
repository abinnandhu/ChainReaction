using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    // Shake camera with intensity and duration
    public void Shake(float duration = 0.3f, float intensity = 0.2f)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, intensity));
        }
    }

    IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Random offset
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Return to original position
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}