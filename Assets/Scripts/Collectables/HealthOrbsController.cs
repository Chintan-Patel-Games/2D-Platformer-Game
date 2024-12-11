using UnityEngine;

public class HealthOrbsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && other as CapsuleCollider2D)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.Lives < 3)
            {
                playerController.PickHealthOrb();
                Destroy(gameObject);
            }
        }
    }
}