using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _txtMessage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failedColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failedSprite;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {

        gameObject.SetActive(true);
        _backgroundImage.color = _failedColor;
        _txtMessage.text = "DELIVERY\nFAILED";
        _iconImage.sprite = _failedSprite;
        StartCoroutine(Hide());
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        _backgroundImage.color = _successColor;
        _txtMessage.text = "DELIVERY\nSUCCESS";
        _iconImage.sprite = _successSprite;
        StartCoroutine(Hide());
    }

    private void LateUpdate()
    {
        transform.forward = _mainCamera.transform.forward;
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
