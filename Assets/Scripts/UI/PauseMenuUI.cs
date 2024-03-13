using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button _btnUnPause;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnMainMenu;
    [SerializeField] private GameObject _gameSettingsUI;

    private void Awake()
    {
        _btnUnPause.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            Time.timeScale = 1;
            gameObject.SetActive(false);
        });

        _btnSettings.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            _gameSettingsUI.SetActive(true);
            gameObject.SetActive(false);
        });

        _btnMainMenu.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneEnum.MainMenuScene.ToString());
        });
    }
}
