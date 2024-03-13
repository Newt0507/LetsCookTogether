using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private Animator _animator;

    private void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            _animator.SetTrigger(OPEN_CLOSE);

            AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
            KitchenObject.SpawnKitchenObject(_kitchenObjectSO, player);
        }
    }
}
