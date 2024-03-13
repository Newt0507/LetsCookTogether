using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void LateUpdate()
    {
        transform.forward = _mainCamera.transform.forward;
    }

    private void StoveCounter_OnProgressChanged(object sender, IProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool canShow = _stoveCounter.IsFried() && e.progressTime >= burnShowProgressAmount;

        if (canShow)
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
