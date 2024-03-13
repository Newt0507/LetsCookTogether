using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList;
    [SerializeField] private Transform _iconTemplateVisual;
    [SerializeField] private bool _isPlate;

    private List<KitchenObjectSO> _kitchenObjectSOList;

    private void Awake()
    {
        _kitchenObjectSOList = new List<KitchenObjectSO>();
        _iconTemplateVisual.gameObject.SetActive(false);
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }

    public void SetKitchenObjectSOList(List<KitchenObjectSO> kitchenObjectSOList)
    {
        _kitchenObjectSOList = kitchenObjectSOList;
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        if (_kitchenObjectSOList.Contains(kitchenObjectSO))
            return false;
        else
        {
            _kitchenObjectSOList.Add(kitchenObjectSO);
            UpdateVisual(kitchenObjectSO);
            return true;
        }
    }

    public bool IsPlate()
    {
        return _isPlate;
    }


    private void UpdateVisual(KitchenObjectSO kitchenObjectSO)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out KitchenObject kitchenObject))
            {
                if (kitchenObject.GetKitchenObjectSO() == kitchenObjectSO)
                {
                    kitchenObject.gameObject.SetActive(true);
                    if (_isPlate)
                        UpdateUIVisual(kitchenObjectSO);
                }
            }
        }
    }

    private void UpdateUIVisual(KitchenObjectSO kitchenObjectSO)
    {
        Transform iconTransform = Instantiate(_iconTemplateVisual, _iconTemplateVisual.parent);
        iconTransform.gameObject.SetActive(true);
        iconTransform.GetComponent<IconTemplateVisualUI>().SetKitchenObjectSOVisual(kitchenObjectSO);
    }
}