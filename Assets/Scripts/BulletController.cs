using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Particle hit: {other.name}");
        if (other.GetComponent<EnemyController>() != null)
        {
            other.GetComponent<EnemyController>().TakeDamage(10); // Damage enemy
            Debug.Log("Enemy took damage!");
        }
    }
}