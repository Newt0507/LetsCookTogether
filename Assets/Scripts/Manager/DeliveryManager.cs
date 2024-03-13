using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private MenuSO _menuSO;
    [SerializeField] private int _waitingRecipeMax;

    private List<RecipeSO> _waitingRecipeList;
    private int _spawnTimer;
    private int _spawnTimerMax = 10;

    private int _numberRecicpesDelivered = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _waitingRecipeList = new List<RecipeSO>();
    }
    private void Start()
    {
        StartCoroutine(WaitingTimer());
    }

    private IEnumerator WaitingTimer()
    {
        yield return new WaitForSeconds(1f);
        _spawnTimer--;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _spawnTimerMax;
            if(GamePlayingManager.Instance.IsPlayingState())
                SpawnRecipeSO();
        }
        StartCoroutine(WaitingTimer());
    }

    private void SpawnRecipeSO()
    {
        if (_waitingRecipeList.Count < _waitingRecipeMax)
        {            
            RecipeSO newWaitingRecipeSO = _menuSO.recipeList[UnityEngine.Random.Range(0, _menuSO.recipeList.Count)];
            _waitingRecipeList.Add(newWaitingRecipeSO);

            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliveryRecipe(KitchenObject kitchenObject)
    {
        for (int i = 0; i < _waitingRecipeList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeList[i];

            bool matchRecipe = true;
            
            if (kitchenObject.TryGetContainerKitchenObject(out ContainerKitchenObject containerKitchenObject)) //Player đưa đĩa hoặc friesBox vào
            {
                if (waitingRecipeSO.kitchenObjectSOList.Count == containerKitchenObject.GetKitchenObjectSOList().Count) //Trong đĩa hoặc friesBox có thành phần = thành phần trong order đang xét
                {
                    //Loop thành phần 
                    //Check khi có thành phần giống nhau thì kiểm tra thành phần tiếp theo
                    //Khi có thành phần k giống thì matchRecipe = false và ngay lập tức thoát loop

                    foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                    {
                        bool ingredientFound = false;
                        foreach (KitchenObjectSO plateKitchenObjectSO in containerKitchenObject.GetKitchenObjectSOList())
                        {
                            if (plateKitchenObjectSO == recipeKitchenObjectSO)
                            {
                                ingredientFound = true;
                                break;
                            }
                        }

                        if (!ingredientFound)
                        {
                            matchRecipe = false;
                            break;
                        }
                    }
                }
                else //Trong đĩa hoặc friesBox số lượng thành phần k bằng thành phần trong order đang xét
                {
                    matchRecipe = false;
                }

            }
            else //Player đưa gì đó k phải đĩa hoặc friesBox
            {
                bool ingredientFound = false;

                //Check kitchenObject: có thể giao và có trong order đang xét
                if (kitchenObject.CanBeDelivered() && waitingRecipeSO.kitchenObjectSOList.Contains(kitchenObject.GetKitchenObjectSO()))
                    ingredientFound = true;

                if (!ingredientFound)
                    matchRecipe = false;
            }


            if (matchRecipe)
            {
                _numberRecicpesDelivered++;
                _waitingRecipeList.RemoveAt(i);
                AudioManager.Instance.PlaySFX(SoundEnum.DeliverySuccessSound);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        AudioManager.Instance.PlaySFX(SoundEnum.DeliveryFailedSound);
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeList;
    }

    public int GetNumberRecicpesDelivered()
    {
        return _numberRecicpesDelivered;
    }
}
