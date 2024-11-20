using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Animator progress;
    [SerializeField] Button startBtn;
    [SerializeField] AudioSource startSFX;
    [SerializeField] Button optionsBtn;
    [SerializeField] AudioSource optionsSFX;
    [SerializeField] Button optionsBackBtn;
    [SerializeField] AudioSource optionsBackSFX;
    [SerializeField] Button quitBtn;
    [SerializeField] AudioSource quitSFX;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        optionsBtn.onClick.AddListener(Options);
        optionsBackBtn.onClick.AddListener(OptionsBack);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        startSFX.Play();
        menuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        if (IsAnimationFinished(progress))
        {
            SceneManager.LoadScene(1);
        }
    }

    private bool IsAnimationFinished(Animator animationName)
    {
        AnimatorStateInfo stateInfo = animationName.GetCurrentAnimatorStateInfo(0); // Get state info of layer 0

        // Check if the current animation is the target animation and if it has finished
        return stateInfo.normalizedTime >= 1.0f;
    }

    public void Options()
    {
        optionsSFX.Play();
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OptionsBack()
    {
        optionsBackSFX.Play();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        quitSFX.Play();
        Application.Quit();
    }
}