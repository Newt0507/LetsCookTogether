using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    [SerializeField] private SoundSO _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _musicSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f);
        _sfxSource.volume = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);

        AudioManager.Instance.PlayMusic(SoundEnum.BackgroundSound);
    }

    public void PlayMusic(SoundEnum sound)
    {
        bool soundFound = false;
        foreach(var musicSound in _musicSounds.soundList)
        {
            if (sound == musicSound.sound)
            {
                soundFound = true;
                _musicSource.clip = musicSound.clip[UnityEngine.Random.Range(0, musicSound.clip.Length - 1)];
                _musicSource.Play();
                break;
            }
        }

        if (!soundFound)
        {
            Debug.LogError("Sound " + sound + " Not Found!");
        }
    }

    public void PlaySFX(SoundEnum sound)
    {
        bool soundFound = false;
        foreach(var sfxSounds in _sfxSounds.soundList)
        {
            if (sound == sfxSounds.sound)
            {
                soundFound = true;
                _sfxSource.PlayOneShot(sfxSounds.clip[UnityEngine.Random.Range(0, sfxSounds.clip.Length - 1)]);
                break;
            }
        }

        if (!soundFound)
        {
            Debug.LogError("Sound " + sound + " Not Found!");
        }
    }

    public float GetMusicVolume()
    {
        return _musicSource.volume;
    }

    public float GetSFXVolume()
    {
        return _sfxSource.volume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopSFX()
    {
        _sfxSource.Stop();
    }
}
