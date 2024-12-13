using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null && other as CapsuleCollider2D)
        {
            playerController.PickKey();
            Destroy(gameObject);
        }
    }
}