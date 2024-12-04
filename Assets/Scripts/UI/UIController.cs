using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Animator[] lifeAnimators; // Array of Animators for each life
    [SerializeField] private GameObject[] keyIcons;   // Array of key icons
    [SerializeField] private GameObject gamePaused;   // Game Paused UI
    [SerializeField] private TextMeshProUGUI levelNoText; // UI element to display the level number
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button quitBtn;
    private bool isPaused = false;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(ResumeGame);
        homeBtn.onClick.AddListener(Home);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        DisplayLevelNumber();
    }

    private void Update()
    {
        // Check if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); // Toggle pause when Escape is pressed
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused; // Toggle the pause state
        Time.timeScale = isPaused ? 0 : 1; // Pause or resume the game
        SoundManager.Instance.Play(Sounds.backBtn);
        gamePaused.SetActive(isPaused); // Show or hide the pause menu
    }

    private void ResumeGame()
    {
        isPaused = false;
        SoundManager.Instance.Play(Sounds.confirmBtn);
        Time.timeScale = 1; // Resume the game
        gamePaused.SetActive(false); // Hide the pause menu
    }

    private void Home()
    {
        SoundManager.Instance.Play(Sounds.startBtn);
        Time.timeScale = 1; // Resume time before navigating
        SceneManager.LoadScene((int)LevelList.Lobby);
    }

    private void QuitGame()
    {
        SoundManager.Instance.Play(Sounds.quitBtn);
        Application.Quit(); // Quit the application
    }

    // Display the current level number
    private void DisplayLevelNumber()
    {
        levelNoText.text = SceneManager.GetActiveScene().name;
    }

    // Calls when the player loses a life
    public void TakeLife(int lifeIndex)
    {
        if (IsValidLifeIndex(lifeIndex))
        {
            lifeAnimators[lifeIndex].SetTrigger("healthLost");
        }
    }

    // Calls when the player gains a life
    public void GainLife(int lifeIndex)
    {
        lifeIndex--;
        if (IsValidLifeIndex(lifeIndex))
        {
            lifeAnimators[lifeIndex].SetTrigger("healthGain");
        }
    }

    // Validate if the given life index is within bounds
    private bool IsValidLifeIndex(int index)
    {
        return index >= 0 && index < lifeAnimators.Length;
    }

    // Update the key icons based on the current key count
    public void UpdateKeysUI(int currentKeys)
    {
        for (int i = 0; i < keyIcons.Length; i++)
        {
            keyIcons[i].SetActive(i < currentKeys);
        }
    }
}