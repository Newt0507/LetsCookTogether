using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicIconUI : MonoBehaviour
{
    [SerializeField] private GameSettingsUI _gameSettingsUI;
    [SerializeField] private Sprite _musicON;
    [SerializeField] private Sprite _musicOFF;

    private Image _iconImage;

    private void Awake()
    {
        _iconImage = gameObject.GetComponent<Image>();
        SetIcon(AudioManager.Instance.GetMusicVolume());
    }

    private void Start()
    {
        _gameSettingsUI.OnMusicVolumeChanged += GameSettingsUI_OnMusicVolumeChanged;
    }

    private void GameSettingsUI_OnMusicVolumeChanged(object sender, GameSettingsUI.OnVolumeChangedEventArgs e)
    {
        SetIcon(e.volume);
    }

    private void SetIcon(float volume)
    {
        if (volume <= 0)
            _iconImage.sprite = _musicOFF;
        else
            _iconImage.sprite = _musicON;
    }
}
