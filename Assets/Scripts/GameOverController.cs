using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private Button restartbtn;
    [SerializeField] private Button quitbtn;

    private void Awake()
    {
        restartbtn.onClick.AddListener(ReloadLevel);
        quitbtn.onClick.AddListener(QuitLevel);
    }
    public void PlayerDied()
    {
        gameObject.SetActive(true);
        PlayGameOverAnimation();
    }

    public void PlayGameOverAnimation()
    {
        if (gameOverAnimator != null)
        {
            gameOverAnimator.Play("Game_Over");
            Debug.Log("GameOver animation triggered.");
        }
        else
        {
            Debug.LogError("Animator not assigned.");
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(0);
    }
}