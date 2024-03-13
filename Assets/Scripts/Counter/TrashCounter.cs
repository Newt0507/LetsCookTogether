using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject deleteKitchenObject = player.GetKitchenObject();

            if (deleteKitchenObject.TryGetContainerKitchenObject(out ContainerKitchenObject containerKitchenObject))
            {
                if (containerKitchenObject.IsPlate())
                {
                    if (containerKitchenObject.GetKitchenObjectSOList().Count != 0)
                    {
                        AudioManager.Instance.PlaySFX(SoundEnum.TrashSound);
                        player.GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
                    }
                }
                else
                {
                    AudioManager.Instance.PlaySFX(SoundEnum.TrashSound);
                    player.GetKitchenObject().DestroySelf();
                }
            }
            else
            {
                AudioManager.Instance.PlaySFX(SoundEnum.TrashSound);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
