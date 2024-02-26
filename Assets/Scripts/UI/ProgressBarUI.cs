using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _progressGameObject;
    [SerializeField] private Slider _slider;

    private Camera _mainCamera;
    private IProgress _progress;

    private void Start()
    {
        _progress = _progressGameObject.GetComponent<IProgress>();
        if (_progress == null)
            Debug.LogError(_progressGameObject.name + "does not implement IProgress");

        _progress.OnProgressChanged += Progress_OnProgresChanged;

        _mainCamera = Camera.main;
        
        _slider.value = 0f;
        Hide();
    }

    private void LateUpdate()
    {
        transform.forward = _mainCamera.transform.forward;
    }

    private void Progress_OnProgresChanged(object sender, IProgress.OnProgressChangedEventArgs e)
    {
        _slider.value = e.progressTime;
        if (e.maxValue != 0f)
            _slider.maxValue = e.maxValue;

        if (e.progressTime <= _slider.minValue || e.progressTime >= _slider.maxValue)
            Hide();
        else
            Show();
    }

    private void Show()
    {
        _slider.gameObject.SetActive(true);
    }

    private void Hide()
    {
        _slider.gameObject.SetActive(false);
    }
}
