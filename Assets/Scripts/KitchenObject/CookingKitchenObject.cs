using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingKitchenObject : KitchenObject
{
    [SerializeField] private CookingState _cookingState;

    public CookingState GetCookingState()
    {
        return _cookingState;
    }

}