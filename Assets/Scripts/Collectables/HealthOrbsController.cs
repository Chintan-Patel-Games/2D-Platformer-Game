using UnityEngine;

public class HealthOrbsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null && other as CapsuleCollider2D)
        {
            if (playerController.Lives < 3)
            {
                playerController.PickHealthOrb();
                Destroy(gameObject);
            }
        }
    }
}