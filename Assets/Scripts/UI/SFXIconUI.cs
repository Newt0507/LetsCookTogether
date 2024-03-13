using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXIconUI : MonoBehaviour
{
    [SerializeField] private GameSettingsUI _gameSettingsUI;
    [SerializeField] private Sprite _sfxON;
    [SerializeField] private Sprite _sfxOFF;

    private Image _iconImage;

    private void Awake()
    {
        _iconImage = gameObject.GetComponent<Image>();
        SetIcon(AudioManager.Instance.GetSFXVolume());
    }

    private void Start()
    {
        _gameSettingsUI.OnSFXVolumeChanged += GameSettingsUI_OnSFXVolumeChanged;
    }

    private void GameSettingsUI_OnSFXVolumeChanged(object sender, GameSettingsUI.OnVolumeChangedEventArgs e)
    {
        SetIcon(e.volume);
    }

    private void SetIcon(float volume)
    {
        if(volume <= 0)
            _iconImage.sprite = _sfxOFF;
        else
            _iconImage.sprite = _sfxON;
    }
}
