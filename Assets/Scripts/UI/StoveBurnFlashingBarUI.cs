using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter _stoveCounter;

    private Camera _mainCamera;
    private Animator _anim;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _anim = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        _anim.SetBool(IS_FLASHING, false);
    }

    private void LateUpdate()
    {
        transform.forward = _mainCamera.transform.forward;
    }

    private void StoveCounter_OnProgressChanged(object sender, IProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool canShow = _stoveCounter.IsFried() && e.progressTime >= burnShowProgressAmount;

        _anim.SetBool(IS_FLASHING, canShow);
    }
}

