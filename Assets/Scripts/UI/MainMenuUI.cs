using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnQuit;
    [SerializeField] private GameObject _gameSettingsUI;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene(SceneEnum.LoadingScene.ToString());
        });

        _btnSettings.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            _gameSettingsUI.SetActive(true);
        });

        _btnQuit.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            Debug.Log("Quit Button On Click");
            Application.Quit();
        });
    }
}
