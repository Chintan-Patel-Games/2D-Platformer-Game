using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCompleteController : MonoBehaviour
{
    [SerializeField] Button nextLevelBtn;
    [SerializeField] Button homeBtn;
    private bool isGameComplete;
    public bool IsGameComplete { get { return isGameComplete; } }

    private void Awake()
    {
        nextLevelBtn.onClick.AddListener(NextLevel);
        homeBtn.onClick.AddListener(Home);
    }

    public void GameComplete()
    {
        SoundManager.Instance.Play(Sounds.levelComplete);
        Time.timeScale = 0;
        isGameComplete = true;
        gameObject.SetActive(true);
    }
    
    public void NextLevel()
    {
        SoundManager.Instance.Play(Sounds.startBtn);
        Time.timeScale = 1;
        isGameComplete = false;

        // Get the current scene
        Scene currentLevel = SceneManager.GetActiveScene();

        // Find the current level in the enum and determine the next level
        if (System.Enum.TryParse(currentLevel.name, out LevelList currentLevelEnum))
        {
            int currentIndex = (int)currentLevelEnum;

            // Check if there is a next level
            if (currentIndex + 1 < System.Enum.GetValues(typeof(LevelList)).Length)
            {
                LevelList nextLevel = (LevelList)(currentIndex + 1);
                SceneManager.LoadScene((int)nextLevel);
            }
            else
            {
                SceneManager.LoadScene((int)LevelList.Lobby);
            }
        }
        else
        {
            Debug.LogError($"Current scene {currentLevel.name} is not in the LevelList enum.");
        }
    }

    public void Home()
    {
        SoundManager.Instance.Play(Sounds.backBtn);
        Time.timeScale = 1;
        isGameComplete = false;
        SceneManager.LoadScene((int)LevelList.Lobby);
    }
}