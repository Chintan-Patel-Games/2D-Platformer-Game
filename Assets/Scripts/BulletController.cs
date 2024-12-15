using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        EnemyController enemyController = other.GetComponent<EnemyController>();
        PolygonCollider2D collider = other.GetComponent<PolygonCollider2D>();
        if (enemyController != null && collider != null)
        {
            enemyController.TakeDamage(2); // Damage enemy
        }
    }
}