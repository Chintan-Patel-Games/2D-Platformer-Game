using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelection : MonoBehaviour
{
    private Button levelBtn;
    private TextMeshProUGUI levelText;
    [SerializeField] AudioSource levelSfx;
    [SerializeField] string levelNo;
    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;
    [SerializeField] GameObject levelSelectScreen;
    [SerializeField] GameObject loadingScreen;

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
        levelSfx.Play();
        // levelSelectScreen.SetActive(false);
        // loadingScreen.SetActive(true);
        SceneManager.LoadScene(levelNo);

        // if (IsAnimationFinished(loadingScreen))
        // {
        //     SceneManager.LoadScene(levelNo);
        // }
    }

    // private bool IsAnimationFinished(GameObject loadingBar)
    // {
    //     Animator animator = loadingBar.GetComponentInChildren<Animator>();
    //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Get state info of layer 0

    //     // Check if the current animation is the target animation and if it has finished
    //     return stateInfo.normalizedTime >= 1.0f;
    // }
}