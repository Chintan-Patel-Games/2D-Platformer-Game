using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject levelScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] Button startBtn;
    [SerializeField] Button levelBackBtn;
    [SerializeField] Button optionsBackBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Slider sfxVol;
    [SerializeField] Slider bgmVol;
    [SerializeField] Toggle muteToggle;
    private float lastVolume = 1f; // Store the last volume for unmuting

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
        levelBackBtn.onClick.AddListener(LevelBack);
        optionsBtn.onClick.AddListener(Options);
        optionsBackBtn.onClick.AddListener(OptionsBack);
        quitBtn.onClick.AddListener(QuitGame);

        // Initialize UI with current audio settings
        sfxVol.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bgmVol.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        muteToggle.isOn = PlayerPrefs.GetInt("Mute", 0) == 1;

        // Apply initial settings
        SetSFXVolume(sfxVol.value);
        SetBGMVolume(bgmVol.value);
        SetMute(muteToggle.isOn);

        // Add listeners to UI components
        sfxVol.onValueChanged.AddListener(SetSFXVolume);
        bgmVol.onValueChanged.AddListener(SetBGMVolume);
        muteToggle.onValueChanged.AddListener(SetMute);
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

        #if UNITY_WEBGL
        // Show a native browser alert
        Application.ExternalEval("alert('Thank you for playing! Please close the browser tab to exit.');");
        #else
        Application.Quit();
        #endif
    }

    public void SetSFXVolume(float volume)
    {
        if (!muteToggle.isOn)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetSFXVol(volume);
            }
            else
                Debug.Log("SoundManager instance not found!");
            lastVolume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
    }
    public void SetBGMVolume(float volume)
    {
        if (!muteToggle.isOn)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetBGMVol(volume);
            }
            else
                Debug.Log("SoundManager instance not found!");
            lastVolume = volume;
            PlayerPrefs.SetFloat("BGMVolume", volume);
        }
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            SoundManager.Instance.Mute(true);
            PlayerPrefs.SetInt("Mute", 1);
        }
        else
        {
            SoundManager.Instance.Mute(false);
            SoundManager.Instance.SFX.volume = lastVolume;
            PlayerPrefs.SetInt("Mute", 0);
        }
    }
}