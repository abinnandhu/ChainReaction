using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance;

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;
    public float explosionDuration = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    // Play explosion effect at position with player color
    public void PlayExplosion(Vector3 position, Color color)
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning("Explosion prefab not assigned!");
            return;
        }

        // Create explosion instance
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        // Get particle system
        ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            // Set color
            var main = ps.main;
            main.startColor = color;

            // Play
            ps.Play();
        }

        // Destroy after duration
        Destroy(explosion, explosionDuration + 0.5f);
    }
}