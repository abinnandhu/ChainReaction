using UnityEngine;
using System.Collections;

public class OrbAnimator : MonoBehaviour
{
    public void AnimateSpawn()
    {
        // Start small
        transform.localScale = Vector3.zero;

        // Animate to normal size
        StartCoroutine(ScaleAnimation());
    }

    IEnumerator ScaleAnimation()
    {
        Vector3 targetScale = Vector3.one * 0.4f; // Match the orb size
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Ease out back (bouncy effect)
            float bounce = Mathf.Clamp01(1f - Mathf.Pow(1f - progress, 3f));

            transform.localScale = targetScale * bounce;

            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void AnimateDestroy()
    {
        StartCoroutine(ShrinkAnimation());
    }

    IEnumerator ShrinkAnimation()
    {
        Vector3 startScale = transform.localScale;
        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            transform.localScale = startScale * (1f - progress);

            yield return null;
        }

        Destroy(gameObject);
    }
}