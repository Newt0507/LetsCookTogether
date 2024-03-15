using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject deliveredKitchenObject = player.GetKitchenObject();
            DeliveryManager.Instance.DeliveryRecipe(deliveredKitchenObject);
            player.GetKitchenObject().DestroySelf();


            if (deliveredKitchenObject.TryGetContainerKitchenObject(out ContainerKitchenObject containerKitchenObject))
                if (containerKitchenObject.IsPlate())
                    KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);            
        }
    }
}