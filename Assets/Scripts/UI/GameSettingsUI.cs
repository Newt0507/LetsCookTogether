using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{
    public event EventHandler<OnVolumeChangedEventArgs> OnMusicVolumeChanged;
    public event EventHandler<OnVolumeChangedEventArgs> OnSFXVolumeChanged;
    public class OnVolumeChangedEventArgs : EventArgs
    {
        public float volume;
    }

    [SerializeField] private Slider _musicSlider, _sfxSlider;
    [SerializeField] private Button _btnOK;
    [SerializeField] private Transform _pauseMenuUI;

    private void Awake()
    {
        _musicSlider.value = AudioManager.Instance.GetMusicVolume();
        _sfxSlider.value = AudioManager.Instance.GetSFXVolume();

        _btnOK.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            _pauseMenuUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });

        _musicSlider.onValueChanged.AddListener((volume) =>
        {
            AudioManager.Instance.SetMusicVolume(volume);
            OnMusicVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs()
            {
                volume = volume
            });
        });

        _sfxSlider.onValueChanged.AddListener((volume) =>
        {
            AudioManager.Instance.SetSFXVolume(volume);
            OnSFXVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs()
            {
                volume = volume
            });
        });
    }


}
