using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelection : MonoBehaviour
{
    private Button levelBtn;
    [SerializeField] AudioSource levelSfx;
    [SerializeField] string levelNo;
    [SerializeField] GameObject levelSelectScreen;
    [SerializeField] GameObject loadingScreen;

    private void Awake()
    {
        levelBtn = GetComponent<Button>();
        levelBtn.onClick.AddListener(StartLevel);
    }

    public void StartLevel()
    {
        levelSfx.Play();
        levelSelectScreen.SetActive(false);
        loadingScreen.SetActive(true);

        if (IsAnimationFinished(loadingScreen))
        {
            SceneManager.LoadScene(levelNo);
        }
    }

    private bool IsAnimationFinished(GameObject loadingBar)
    {
        Animator animator = loadingBar.GetComponentInChildren<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Get state info of layer 0

        // Check if the current animation is the target animation and if it has finished
        return stateInfo.normalizedTime >= 1.0f;
    }
}