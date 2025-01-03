using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelection : MonoBehaviour
{
    private Button levelBtn;
    private TextMeshProUGUI levelText;
    [SerializeField] string levelNo;
    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;

    private void Awake()
    {
        levelBtn = GetComponent<Button>();
        levelText = levelBtn.GetComponentInChildren<TextMeshProUGUI>();

        // Initialize button state
        UpdateButtonState();

        levelBtn.onClick.AddListener(StartLevel);
    }

    private void UpdateButtonState()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelNo);

        switch (levelStatus)
        {
            case LevelStatus.Locked:
                levelBtn.interactable = false;
                levelText.color = disabledColor;
                break;
            case LevelStatus.Unlocked:
                levelBtn.interactable = true;
                levelText.color = enabledColor;
                break;
            case LevelStatus.Completed:
                levelBtn.interactable = true;
                levelText.color = enabledColor;
                break;
        }
    }

    public void StartLevel()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelNo);

        switch (levelStatus)
        {
            case LevelStatus.Locked:
                Debug.Log("Complete the previous level to unlock " + levelNo);
                break;
            case LevelStatus.Unlocked:
                StartLevelLoading();
                break;
            case LevelStatus.Completed:
                StartLevelLoading();
                break;
        }
    }

    private void StartLevelLoading()
    {
        SoundManager.Instance.Play(Sounds.startBtn);
        SceneManager.LoadScene(levelNo);
    }
}