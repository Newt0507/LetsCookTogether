using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _reactPoint;

    private KitchenObject kitchenObject;
    //private KitchenObject plate;

    private void Awake()
    {
        InitialCheck();
    }

    private void InitialCheck()
    {
        KitchenObject child = _spawnPoint.GetComponentInChildren<KitchenObject>();
        if (child != null)
        {
            kitchenObject = child;
            kitchenObject.SetNewKitchenObjectParent(this);
        }
    }

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.cs/Interact()");
    }    

    public Transform GetReactPoint()
    {
        return _reactPoint;
    }

    public Transform GetSpawnPoint()
    {
        return _spawnPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
