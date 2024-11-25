using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeBtn;

    private void Awake()
    {
        restartBtn.onClick.AddListener(ReloadLevel);
        homeBtn.onClick.AddListener(Home);
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

    public void Home()
    {
        SceneManager.LoadScene((int)LevelList.Lobby);
    }
}