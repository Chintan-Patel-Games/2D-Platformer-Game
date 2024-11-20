using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuCard;
    [SerializeField] GameObject optionsCard;
    [SerializeField] Button startBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button optionsBackBtn;
    [SerializeField] Button quitBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        optionsBtn.onClick.AddListener(Options);
        optionsBackBtn.onClick.AddListener(OptionsBack);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Options()
    {
        menuCard.SetActive(false);
        optionsCard.SetActive(true);
    }

    private void OptionsBack()
    {
        optionsCard.SetActive(false);
        menuCard.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
