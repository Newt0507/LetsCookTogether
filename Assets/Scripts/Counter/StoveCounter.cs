using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IProgress
{
    public event EventHandler<IProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    [SerializeField] private GameObject[] _gameObjectVisualArray;

    private CookingState _cookingState;
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private bool _isCooking;

    private void Start()
    {
        _cookingState = CookingState.Idle;
        _isCooking = false;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_cookingState)
            {
                case CookingState.Idle:
                    break;

                case CookingState.Frying:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _fryingTimer,
                    });

                    if (_fryingTimer >= _fryingRecipeSO.fryingAmountMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        _cookingState = CookingState.Fried;
                        _fryingTimer = 0f;
                        _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }
                    break;

                case CookingState.Fried:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _fryingTimer,
                        maxValue = _fryingRecipeSO.fryingAmountMax
                    });

                    if (_fryingTimer >= _fryingRecipeSO.fryingAmountMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        _cookingState = CookingState.Burned;
                    }
                    break;

                case CookingState.Burned:
                    _isCooking = false;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _fryingTimer,
                    });

                    break;
            }

        }


        CookingVisual();
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

                    _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    MeatKitchenObject _meatKitchenObject = GetKitchenObject().GetComponent<MeatKitchenObject>();
                    if (_meatKitchenObject.GetCookingState() == CookingState.Burned)
                        _cookingState = CookingState.Burned;
                    else if (_meatKitchenObject.GetCookingState() == CookingState.Fried)
                        _cookingState = CookingState.Fried;
                    else
                        _cookingState = CookingState.Frying;

                    _fryingTimer = 0f;
                    _isCooking = true;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _fryingTimer,
                        maxValue = _fryingRecipeSO.fryingAmountMax
                    });
                }
                else
                {

                }

            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {

            }
            else
            {
                GetKitchenObject().SetNewKitchenObjectParent(player);
                _cookingState = CookingState.Idle;
                _isCooking = false;

                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressTime = 0f,
                });

            }
        }
    }

    private void CookingVisual()
    {
        foreach (GameObject visual in _gameObjectVisualArray)
        {
            visual.SetActive(_isCooking);
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray)
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;

        return null;
    }
}