using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject levelScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] Button startBtn;
    [SerializeField] Button levelSecBackBtn;
    [SerializeField] Button optionsBackBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button quitBtn;

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
        SoundManager.Instance.Play(Sounds.confirmBtn);
        menuScreen.SetActive(false);
        levelScreen.SetActive(true);
    }

    private void LevelBack()
    {
        SoundManager.Instance.Play(Sounds.backBtn);
        levelScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void Options()
    {
        SoundManager.Instance.Play(Sounds.confirmBtn);
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OptionsBack()
    {
        SoundManager.Instance.Play(Sounds.backBtn);
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        SoundManager.Instance.Play(Sounds.quitBtn);
        Application.Quit();
    }
}