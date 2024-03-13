using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private GameObject _gamePauseButtonUI;
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _gameSettingsUI;

    private void Awake()
    {
        _gamePauseButtonUI.SetActive(true);
        _pauseMenuUI.SetActive(false);
        _gameSettingsUI.SetActive(false);
    }
}
