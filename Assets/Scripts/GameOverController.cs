using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Button restartbtn;
    [SerializeField] Button quitbtn;
    private LevelList levelList;

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
        Scene reloadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(reloadScene.buildIndex);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(0);
    }
}