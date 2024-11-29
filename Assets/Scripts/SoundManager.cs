using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager soundManager;
    public static SoundManager Instance { get { return soundManager; } }
    public AudioSource SFX;
    public AudioSource BGM;
    public SoundType[] Sounds;
    public bool isMute = false;
    public float Volume;

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
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
        SFX.volume = volume;
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
    lobbyBgm,
    levelBgm,
    playerDied,
    levelComplete,
    enemyDied,
    startBtn,
    confirmBtn,
    optionsBtn,
    backBtn,
    quitBtn,
}