using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuCard;
    [SerializeField] GameObject optionsCard;
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
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        optionsSFX.Play();
        menuCard.SetActive(false);
        optionsCard.SetActive(true);
    }

    public void OptionsBack()
    {
        optionsBackSFX.Play();
        optionsCard.SetActive(false);
        menuCard.SetActive(true);
    }

    public void QuitGame()
    {
        quitSFX.Play();
        Application.Quit();
    }
}