using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<EnemyController>() != null)
        {
            other.GetComponent<EnemyController>().TakeDamage(2); // Damage enemy
        }
    }
}