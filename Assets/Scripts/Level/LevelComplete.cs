using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] GameCompleteController gameCompleteController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null && other as CapsuleCollider2D)
        {
            LevelManager.Instance.MarkLevelComplete();
            if (playerController.Keys == 3)
            {
                playerController.enabled = false;
                gameCompleteController.GameComplete();
            }
        }
    }
}