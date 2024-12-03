using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager soundManager;
    public static SoundManager Instance { get { return soundManager; } }
    [SerializeField] public AudioSource SFX;
    [SerializeField] public AudioSource BGM;
    [SerializeField] SoundType[] Sounds;
    [SerializeField] bool isMute = false;

    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == LevelList.Lobby.ToString())
        {
            PlayMusic(global::Sounds.lobbyBgm);
        }
        else
        {
            PlayMusic(global::Sounds.levelBgm);
        }
    }

    public void PlayMusic(Sounds sound)
    {
        if (isMute) { return; }
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            BGM.clip = clip;
            BGM.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " + sound);
        }
    }

    public void Play(Sounds sound)
    {
        if (isMute) { return; }
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            SFX.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " + sound);
        }
    }

    public AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }

    public void Mute(bool status)
    {
        isMute = status;
        BGM.mute = status;
    }

    public void SetSFXVol(float volume)
    {
        SFX.volume = volume;
    }

    public void SetBGMVol(float volume)
    {
        BGM.volume = volume;
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    // BGM music
    lobbyBgm,
    levelBgm,

    // UI SFX
    levelComplete,

    // Player Sounds
    playerMelee1,
    playerMelee2,
    playerMelee3,
    playerMelee4,
    playerRanged1,
    playerRanged2,
    playerRanged3,
    playerRanged4,
    playerFoots1,
    playerFoots2,
    playerFoots3,
    playerFoots4,
    playerHurt1,
    playerHurt2,
    playerHurt3,
    playerHurt4,
    playerHurt5,
    playerHurt6,
    playerjump,
    playerLand,
    playerDied,

    // Chomper Sounds
    chompAttack1,
    chompAttack2,
    chompAttack3,
    chompAttack4,
    chompFoots1,
    chompFoots2,
    chompDie,

    // Spitter Sounds
    spitAttack1,
    spitAttack2,
    spitAttack3,
    spitFoots1,
    spitFoots2,
    spitDie,

    // Button Sounds
    startBtn,
    confirmBtn,
    optionsBtn,
    backBtn,
    quitBtn,
}