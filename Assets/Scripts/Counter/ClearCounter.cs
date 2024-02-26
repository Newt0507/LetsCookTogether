using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
                player.GetKitchenObject().SetNewKitchenObjectParent(this);
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                //Đổi vị trí của 2 cái kitchenobj
                KitchenObject tempKitchenObj = Instantiate(GetKitchenObject());
                GetKitchenObject().DestroySelf();
                player.GetKitchenObject().SetNewKitchenObjectParent(this);
                tempKitchenObj.SetNewKitchenObjectParent(player);
            }
            else
                GetKitchenObject().SetNewKitchenObjectParent(player);
        }
    }

    
}
