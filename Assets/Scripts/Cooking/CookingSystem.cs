using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CookingUserData
{
    [SerializeField] private int _maxStamina;
    public int MaxStamina => _maxStamina;

    [Space]
    [SerializeField] private int _moreAddValue;
    [SerializeField] private int _moreAddValueStamina;

    [Space]
    [SerializeField] private int _addValue;
    [SerializeField] private int _addValueStamina;

    [Space]
    [SerializeField] private int _smallAddValue;
    [SerializeField] private int _smallAddValueStamina;

    public int MoreAddValue => _moreAddValue;
    public int MoreAddValueStamina => _moreAddValueStamina;
    public int AddValue => _addValue;
    public int AddValueStamina => _addValueStamina;
    public int SmallAddValue => _smallAddValue;
    public int SmallAddValueStamina => _smallAddValueStamina;
}


public class CookingSystem : MonoBehaviour
{
    [SerializeField] private UICooking _uiCooking;

    [SerializeField] private CookingUserData _userData;

    private RecipeDatabase _recipeDatabase => DatabaseManager.Instance.RecipeDatabase;

    private Inventory[] _inventory => GameManager.Instance.Player.Inventory;

    private RecipeData[] _recipeDatas;
    public CookingUserData CookingData => _userData;




    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _recipeDatas = _recipeDatabase.GetRecipeDataArray();
    }

    public bool CheckRecipe(InventoryItem item)
    {
        foreach(RecipeData data in _recipeDatas)
        {
            if (data.MaterialItemID == item.Id && data.MaterialValue <= item.Count)
            {
                return true;
            }
                
        }

        return false;
    }

    public RecipeData StartCooking(InventoryItem item)
    {
        foreach (RecipeData data in _recipeDatas)
        {
            if (data.MaterialItemID == item.Id)
            {
                for(int i = 0, count = data.MaterialValue; i < count; i++)
                {
                    _inventory[0].Remove(item);
                }
                
                _uiCooking.UpdateUI();

                return data;
            }
        }

        return default;
    }
}
