using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject levelScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] Button startBtn;
    [SerializeField] AudioSource startSFX;
    [SerializeField] Button levelSecBackBtn;
    [SerializeField] Button optionsBackBtn;
    [SerializeField] AudioSource backSFX;
    [SerializeField] Button optionsBtn;
    [SerializeField] AudioSource optionsSFX;
    [SerializeField] Button quitBtn;
    [SerializeField] AudioSource quitSFX;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        levelSecBackBtn.onClick.AddListener(LevelBack);
        optionsBtn.onClick.AddListener(Options);
        optionsBackBtn.onClick.AddListener(OptionsBack);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        startSFX.Play();
        menuScreen.SetActive(false);
        levelScreen.SetActive(true);
    }

    private void LevelBack()
    {
        backSFX.Play();
        levelScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void Options()
    {
        optionsSFX.Play();
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OptionsBack()
    {
        backSFX.Play();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        quitSFX.Play();
        Application.Quit();
    }
}