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
        if(HasKitchenObject()) InteractPlayer();
        
        Animation();
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //Counter này đang k có đồ
        {
            if (player.HasKitchenObject()) //Player đang cầm đồ
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) //Đồ player cầm là nguyên liệu có thể cắt
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
            else //Player đang k cầm gì
            {

            }
        }
        else //Counter này đang có đồ
        {
            if (player.HasKitchenObject()) //Player đang cầm đồ
            {
                if (player.GetKitchenObject().TryGetContainerKitchenObject(out ContainerKitchenObject plateKitchenObject)) //Player đang cầm đĩa
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) //Đồ trên quầy có thể add được vào trong đĩa
                        GetKitchenObject().DestroySelf();
                }
                else //Player đang cầm đồ gì đó k phải đĩa
                {
                    //Nếu player đang cầm có HasRecipeWithInput(...) = true -> Swap
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) //Đồ player cầm là nguyên liệu có thể cắt
                    {
                        //Đổi vị trí của 2 kitchenobj
                        KitchenObject.SwapKitchenObject(GetKitchenObject(), this, player.GetKitchenObject(), player); 

                        _cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        _cuttingTimer = 0f;
                        //_isCutting = true;

                        OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                        {
                            progressTime = _cuttingTimer,
                            maxValue = _cuttingRecipeSO.cuttingAmountMax
                        });
                    }
                }
            }
            else //Player đang k cầm gì
            {
                if (player.IsInteractWithCuttingCounter()) //Player đang tương tác với CuttingCounter
                {
                    if (_cuttingRecipeSO != null && _cuttingTimer < _cuttingRecipeSO.cuttingAmountMax) //Đồ chưa cắt xong
                        _isCutting = true;
                    else //Đồ đã cắt xong
                        GetKitchenObject().SetNewKitchenObjectParent(player);
                }
            }
        }
    }

    private void InteractPlayer()
    {
        float interactDistance = 1f;        
        if (Physics.BoxCast(transform.position, transform.lossyScale / 2, transform.forward , out RaycastHit hit, transform.rotation, interactDistance))
        {
            //Quá trình cắt bắt đầu khi player đang tương tác với CutingCounter và player k cầm đồ gì 
            if (hit.transform.TryGetComponent(out Player player) && player.IsInteractWithCuttingCounter() && !player.HasKitchenObject()) 
            {
                if (_cuttingRecipeSO == null)
                    return;

                if (_cuttingTimer >= _cuttingRecipeSO.cuttingAmountMax)
                {
                    if (_isCutting)
                    {
                        Cut();

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
            _isCutting = false;

            OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
            {
                progressTime = 0f,
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
