using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Ensure Level 1 is unlocked and the rest are locked
        foreach (LevelList level in System.Enum.GetValues(typeof(LevelList)))
        {
            if (level == LevelList.Level1)
            {
                // Ensure Level 1 is unlocked
                if (PlayerPrefs.GetInt(level.ToString(), (int)LevelStatus.Locked) == (int)LevelStatus.Locked)
                {
                    PlayerPrefs.SetInt(level.ToString(), (int)LevelStatus.Unlocked);
                }
            }
            else
            {
                // Lock all other levels
                PlayerPrefs.SetInt(level.ToString(), (int)LevelStatus.Unlocked);
            }
        }
        PlayerPrefs.Save();
    }

    public void MarkLevelComplete()
    {
        // Get the current scene
        Scene currentLevel = SceneManager.GetActiveScene();

        // Mark the current level as Completed 
        PlayerPrefs.SetInt(currentLevel.name, (int)LevelStatus.Completed);

        // Find the current level in the enum and determine the next level
        if (System.Enum.TryParse(currentLevel.name, out LevelList currentLevelEnum))
        {
            int currentIndex = (int)currentLevelEnum;

            // Check if there is a next level
            if (currentIndex + 1 < System.Enum.GetValues(typeof(LevelList)).Length)
            {
                LevelList nextScene = (LevelList)(currentIndex + 1);

                // Unlock the next level
                PlayerPrefs.SetInt(nextScene.ToString(), (int)LevelStatus.Unlocked);
            }
        }
        else
        {
            Debug.LogError($"Current scene {currentLevel.name} is not in the LevelList enum.");
        }
        
        // Save changes to PlayerPrefs
        PlayerPrefs.Save();
    }

    public LevelStatus GetLevelStatus(string level)
    {
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt(level, (int)LevelStatus.Locked);  // Default to Locked
        return levelStatus;
    }

    public void SetLevelStatus(string level, LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level, (int)levelStatus);
    }
}