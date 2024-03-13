using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //Counter này đang k có đồ
        {
            if (player.HasKitchenObject()) //Player đang cầm đồ
            {
                AudioManager.Instance.PlaySFX(SoundEnum.DropSound);
                player.GetKitchenObject().SetNewKitchenObjectParent(this);
            }
            else //Player đang k cầm gì
            {


            }
        }
        else //Counter này đang có đồ
        {            
            if (player.HasKitchenObject()) //Player đang cầm đồ
            {                
                if (player.GetKitchenObject().TryGetContainerKitchenObject(out ContainerKitchenObject containerKitchenObject)) //Player đang cầm đĩa hoặc fries box
                {
                    if (containerKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) //Đồ trên quầy có thể add được vào trong đĩa hoặc fries box
                    {
                        AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                        GetKitchenObject().DestroySelf();
                    }
                    else //Đồ trên quầy k thể add được vào trong đĩa hoặc fries box
                    {
                        AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                        KitchenObject.SwapKitchenObject(GetKitchenObject(), this, player.GetKitchenObject(), player);
                    }
                }
                else //Player đang cầm đồ gì đó k phải đĩa hoặc fries box
                {
                    if (GetKitchenObject().TryGetContainerKitchenObject(out containerKitchenObject)) //Counter đang có đĩa hoặc fries box
                    {
                        if (containerKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) //Đồ player cầm có thể add được vào trong đĩa hoặc fries box
                        {
                            AudioManager.Instance.PlaySFX(SoundEnum.DropSound);
                            player.GetKitchenObject().DestroySelf();
                        }
                        else //Đồ player cầm k thể add được vào trong đĩa hoặc fries box
                        {
                            AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                            KitchenObject.SwapKitchenObject(GetKitchenObject(), this, player.GetKitchenObject(), player);
                        }
                    }
                    else //Đồ trên counter k phải đĩa hoặc fries box
                    {
                        AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                        KitchenObject.SwapKitchenObject(GetKitchenObject(), this, player.GetKitchenObject(), player);
                    }
                }

            }
            else //Player đang k cầm gì
            {
                AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                GetKitchenObject().SetNewKitchenObjectParent(player);
            }
        }
    }

    
}
