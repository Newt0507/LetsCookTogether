using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingCounter : BaseCounter, IProgress
{
    public event EventHandler<IProgress.OnProgressChangedEventArgs> OnProgressChanged;   

    private const string IS_CUTTING = "IsCutting";

    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;

    private float _cuttingTimer;
    private bool _isCutting;

    private CuttingRecipeSO _cuttingRecipeSO;

    private Animator _animator;

    private void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(_cuttingRecipeSO != null) InteractPlayer();
        
        Animation();
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetNewKitchenObjectParent(this);

                    _cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    _cuttingTimer = 0f;
                    _isCutting = true;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _cuttingTimer,
                        maxValue = _cuttingRecipeSO.cuttingAmountMax
                    });
                }                
            }
            else
            {
                _isCutting = false;
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                _isCutting = false;
            }
            else
            {
                if (player.IsInteractWithCuttingCounter())
                {
                    _cuttingTimer = 0f;
                    _isCutting = true;
                }
                else
                {
                    _isCutting = false;
                }
            }
        }
    }

    private void InteractPlayer()
    {
        float interactDistance = 1f;        
        if (Physics.BoxCast(transform.position, transform.lossyScale / 2, transform.forward , out RaycastHit hit, transform.rotation, interactDistance))
        {
            if (hit.transform.TryGetComponent(out Player player) && player.IsInteractWithCuttingCounter())
            {
                if (_cuttingTimer >= _cuttingRecipeSO.cuttingAmountMax)
                {
                    if (_isCutting)
                    {
                        Cut();
                        GetKitchenObject().SetNewKitchenObjectParent(player);
                        _cuttingRecipeSO = null;
                        _isCutting = false;
                    }
                }
                else
                {
                    _cuttingTimer += Time.deltaTime;
                    _isCutting = true;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _cuttingTimer,
                    });
                }
            }
            else
            {
                _isCutting = false;

                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressTime = 0f,
                });
            }
        }
        else
        {
            _cuttingTimer = 0f;
            _isCutting = false;

            OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
            {
                progressTime = _cuttingTimer,
            });
        }
    }
    
    private void Animation()
    {
        _animator.SetBool(IS_CUTTING, _isCutting);
    }

    private void Cut()
    {
        KitchenObjectSO outputKitchenObjectSO = _cuttingRecipeSO.output;

        GetKitchenObject().DestroySelf();

        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in _cuttingRecipeSOArray)
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
                return cuttingRecipeSO;

        return null;
    }
}
