using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private const string HIGH_SCORE = "HighScore";

    [SerializeField] private TextMeshProUGUI _txtNumberRecicpesDelivered;
    [SerializeField] private TextMeshProUGUI _txtHighScore;
    [SerializeField] private Button _btnOK;

    private void Awake()
    {
        _txtHighScore.text = PlayerPrefs.GetInt(HIGH_SCORE, 0).ToString();
        _btnOK.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SoundEnum.OnClickSound);
            SceneManager.LoadScene(SceneEnum.MainMenuScene.ToString());
        });
    }

    private void Start()
    {
        GamePlayingManager.Instance.OnStageChanged += GameManager_OnStageChanged;

        Hide();
    }

    private void GameManager_OnStageChanged(object sender, System.EventArgs e)
    {
        if (GamePlayingManager.Instance.IsGameOverState())
        {
            _txtNumberRecicpesDelivered.text = DeliveryManager.Instance.GetNumberRecicpesDelivered().ToString();
            
            if(DeliveryManager.Instance.GetNumberRecicpesDelivered() > PlayerPrefs.GetInt(HIGH_SCORE))
                _txtHighScore.text = DeliveryManager.Instance.GetNumberRecicpesDelivered().ToString();

            Show();
        }
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.StopSFX();
        AudioManager.Instance.PlaySFX(SoundEnum.GameOverSound);
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
