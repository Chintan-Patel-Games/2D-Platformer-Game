using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Animator gameOverAnimator;
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeBtn;
    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } }

    private void Awake()
    {
        restartBtn.onClick.AddListener(ReloadLevel);
        homeBtn.onClick.AddListener(Home);
    }
    
    public void PlayerDied()
    {
        SoundManager.Instance.Play(Sounds.playerDied);
        Time.timeScale = 0;
        isGameOver = true;
        gameObject.SetActive(true);
        PlayGameOverAnimation();
    }

    public void PlayGameOverAnimation()
    {
        if (gameOverAnimator != null)
        {
            gameOverAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            Debug.LogError("Animator not assigned.");
        }
    }

    public void ReloadLevel()
    {
        SoundManager.Instance.Play(Sounds.startBtn);
        Time.timeScale = 1;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        SoundManager.Instance.Play(Sounds.backBtn);
        Time.timeScale = 1;
        isGameOver = false;
        SceneManager.LoadScene((int)LevelList.Lobby);
    }
}