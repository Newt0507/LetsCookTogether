using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MenuSO")]
public class MenuSO : ScriptableObject
{
    public List<RecipeSO> recipeList;
}