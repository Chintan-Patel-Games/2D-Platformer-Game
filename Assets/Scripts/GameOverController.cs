using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Button restartbtn;
    [SerializeField] Button quitbtn;

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
        }
        else
        {
            Debug.LogError("Animator not assigned.");
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(LevelList.Lobby.ToString());
    }
}