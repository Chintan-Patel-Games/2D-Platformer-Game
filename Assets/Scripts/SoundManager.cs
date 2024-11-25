using System;
using UnityEngine;

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

    private void Start()
    {
        PlayMusic(global::Sounds.bgm);
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

    public void PlayMusic(Sounds sound)
    {
        if (isMute) { return; }
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            BGM.PlayOneShot(clip);
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
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    bgm,
    playerJump,
    playerLand,
    playerDied,
    startBtn,
    confirmBtn,
    optionsBtn,
    backBtn,
    quitBtn,
}