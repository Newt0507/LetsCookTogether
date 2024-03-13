using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameReadyUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI _txtCountdown;

    private Animator _anim;
    private int _previousCountdownNumber;

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        GamePlayingManager.Instance.OnStageChanged += GameManager_OnStageChanged;

        Hide();
    }

    private void Update()
    {
        int countdownNumber = GamePlayingManager.Instance.GetReadyTimer();
        _txtCountdown.text = countdownNumber.ToString();

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _anim.SetTrigger(NUMBER_POPUP);

            if (countdownNumber == 0)
                AudioManager.Instance.PlaySFX(SoundEnum.StartSound);
            else
                AudioManager.Instance.PlaySFX(SoundEnum.WarningSound);
        }

    }

    private void GameManager_OnStageChanged(object sender, System.EventArgs e)
    {
        if (GamePlayingManager.Instance.IsReadyState())
            Show();
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
