using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _currentKitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetNewKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (_currentKitchenObjectParent != null)
            _currentKitchenObjectParent.ClearKitchenObject();

        _currentKitchenObjectParent = newKitchenObjectParent;

        if (!newKitchenObjectParent.HasKitchenObject())
            newKitchenObjectParent.SetKitchenObject(this);

        transform.parent = newKitchenObjectParent.GetSpawnPoint();
        transform.localPosition = Vector3.zero;
    }

    /*public IKitchenObjectParent GetCurrentKitchenObjectParent()
    {
        return _currentKitchenObjectParent;
    }*/

    public void DestroySelf()
    {
        _currentKitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObj = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObj.GetComponent<KitchenObject>();
        kitchenObject.SetNewKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
