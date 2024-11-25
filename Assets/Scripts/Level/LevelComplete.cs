using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            LevelManager.Instance.MarkLevelComplete();
            other.gameObject.GetComponent<PlayerController>().LevelComplete();
        }
    }
}