using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && other as CapsuleCollider2D)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.PickKey();
            Destroy(gameObject);
        }
    }
}