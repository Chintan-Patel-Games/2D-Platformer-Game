using UnityEngine;
using UnityEngine.SceneManagement;

public class Pitfall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            // Debug.Log("You Died");
            // Time.timeScale = 0;
            
            int currentSceneIndex = SceneManager.GetActiveScene( ).buildIndex;
            SceneManager.LoadScene( currentSceneIndex );
        }
    }
}