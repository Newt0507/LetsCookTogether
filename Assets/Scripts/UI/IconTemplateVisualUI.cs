using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplateVisualUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.parent.forward = _mainCamera.transform.forward;
    }

    public void SetKitchenObjectSOVisual(KitchenObjectSO kitchenObjectSO)
    {
        _image.sprite = kitchenObjectSO.sprite;
    }
}
