using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] GameCompleteController gameCompleteController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            LevelManager.Instance.MarkLevelComplete();
            if (other.gameObject.GetComponent<PlayerController>().Keys == 3)
            {
                other.gameObject.GetComponent<PlayerController>().enabled = false;
                gameCompleteController.GameComplete();
            }
        }
    }
}