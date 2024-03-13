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

    private AudioSource _audioSource;

    private float _warningSoundTimer;

    private void Awake()
    {
        _audioSource = gameObject.GetComponentInChildren<AudioSource>();
    }

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
                        Cook();

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

                    if (CanPlayWarningSound())
                        SetBurnWarningTimerSound();

                    if (_fryingTimer >= _fryingRecipeSO.fryingAmountMax)
                    {
                        Cook();

                        _cookingState = CookingState.Burned;                        
                    }
                    break;

                case CookingState.Burned:
                    _isCooking = false;
                    CookingSound();

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = 0f,
                    });

                    break;
            }
        }

        CookingVisual();
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) //Counter này đang k có đồ
        {
            if (player.HasKitchenObject()) //Player đang cầm đồ
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) //Đồ player cầm là nguyên liệu có thể nấu
                {
                    AudioManager.Instance.PlaySFX(SoundEnum.DropSound);
                    player.GetKitchenObject().SetNewKitchenObjectParent(this);

                    _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    CookingKitchenObject _meatKitchenObject = GetKitchenObject().GetComponent<CookingKitchenObject>();
                    if (_meatKitchenObject.GetCookingState() == CookingState.Burned)
                        _cookingState = CookingState.Burned;
                    else if (_meatKitchenObject.GetCookingState() == CookingState.Fried)
                        _cookingState = CookingState.Fried;
                    else
                        _cookingState = CookingState.Frying;

                    _fryingTimer = 0f;
                    _isCooking = true;
                    CookingSound();

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressTime = _fryingTimer,
                        maxValue = _fryingRecipeSO.fryingAmountMax
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
                    {
                        AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                        GetKitchenObject().DestroySelf();
                        _isCooking = false;
                        CookingSound();

                        OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                        {
                            progressTime = 0f,
                        });
                    }
                }
                else //Player đang cầm đồ gì đó k phải đĩa
                {
                    //Nếu player đang cầm có HasRecipeWithInput(...) = true -> Swap
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) //Đồ player cầm là nguyên liệu có thể nấu
                    {
                        //Đổi vị trí của 2 kitchenobj
                        AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                        KitchenObject.SwapKitchenObject(GetKitchenObject(), this, player.GetKitchenObject(), player);

                        _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        CookingKitchenObject _meatKitchenObject = GetKitchenObject().GetComponent<CookingKitchenObject>();
                        if (_meatKitchenObject.GetCookingState() == CookingState.Burned)
                            _cookingState = CookingState.Burned;
                        else if (_meatKitchenObject.GetCookingState() == CookingState.Fried)
                            _cookingState = CookingState.Fried;
                        else
                            _cookingState = CookingState.Frying;

                        _fryingTimer = 0f;
                        _isCooking = true;
                        CookingSound();

                        OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                        {
                            progressTime = _fryingTimer,
                            maxValue = _fryingRecipeSO.fryingAmountMax
                        });
                    }
                }
            }
            else //Player đang k cầm gì
            {
                _cookingState = CookingState.Idle;
                _isCooking = false;
                CookingSound();

                AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
                GetKitchenObject().SetNewKitchenObjectParent(player);
                
                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressTime = 0f,
                });

            }
        }
    }

    private void Cook()
    {
        Quaternion rot = GetKitchenObject().transform.localRotation;
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
        GetKitchenObject().transform.localRotation = rot;
    }

    private void CookingVisual()
    {
        foreach (GameObject visual in _gameObjectVisualArray)
        {
            visual.SetActive(_isCooking);
        }
    }

    private void CookingSound()
    {
        _audioSource.volume = AudioManager.Instance.GetSFXVolume();
        if (_isCooking)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }

    private bool CanPlayWarningSound()
    {
        float burnShowProgressAmount = .5f;
        return _fryingTimer >= burnShowProgressAmount;
    }

    private void SetBurnWarningTimerSound()
    {
        _warningSoundTimer -= Time.deltaTime;
        if (_warningSoundTimer <= 0f)
        {
            float warningSoundTimerMax = .2f;
            _warningSoundTimer = warningSoundTimerMax;

            AudioManager.Instance.PlaySFX(SoundEnum.WarningSound);
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

    public bool IsFried()
    {
        return _cookingState == CookingState.Fried;
    }
}