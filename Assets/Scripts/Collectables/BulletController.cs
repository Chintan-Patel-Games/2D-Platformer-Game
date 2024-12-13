using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(2); // Damage enemy
        }
    }
}