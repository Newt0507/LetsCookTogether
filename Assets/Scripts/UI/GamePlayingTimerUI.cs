using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;
    
    private void Update()
    {
        _timerImage.fillAmount = GamePlayingManager.Instance.GetPlayingTimer();
    }
}
