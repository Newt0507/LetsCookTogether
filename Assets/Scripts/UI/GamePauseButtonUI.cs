using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseButtonUI : MonoBehaviour
{
    [SerializeField] private Button _btnPause;
    [SerializeField] private GameObject _pauseMenuUI;

    private void Awake()
    {
        _btnPause.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            Time.timeScale = 0;
            _pauseMenuUI.SetActive(true);
        });
    }
}
