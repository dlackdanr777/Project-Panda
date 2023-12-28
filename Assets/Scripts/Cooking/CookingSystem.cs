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

    public RecipeData GetRecipeByItem(InventoryItem item)
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

                Debug.Log("레시피가 존재합니다.");
                return data;
            }
        }
        Debug.Log("레시피가 존재하지 않습니다.");
        return default;
    }

    public string CheckItemGrade(RecipeData data, float fireValue)
    {

        bool checkLevel_S = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_S) <= fireValue &&
            data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_S) >= fireValue;

        bool checkLevel_A = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_A) <= fireValue &&
            data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_A) >= fireValue;

        bool checkLevel_B = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_B) <= fireValue &&
    data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_B) >= fireValue;

        if (checkLevel_S)
        {
            return "S";
        }
        else if (checkLevel_A)
        {
            return "A";
        }
        else if (checkLevel_B)
        {
            return "B";
        }
        else
        {
            return "F";
        }
     
    }
}
