using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeData
{
    private string _materialItemID;
    /// <summary>���� �ʿ� ������</summary>
    public string MaterialItemID => _materialItemID;


    private int _materialValue;
    /// <summary>���� �ʿ� ������ ����</summary>
    public int MaterialValue => _materialValue;


    private string _successItemID;
    /// <summary>���� �Ϸ� ������ ID</summary>
    public string SuccessItemID => _successItemID;


    private string _cookware;
    /// <summary>���� ����</summary>
    public string Cookware => _cookware;


    private int _cookingAmount;
    /// <summary>���� Ƚ��</summary>
    public int CookingAmount => _cookingAmount;


    private float _successLocation;
    /// <summary>���� ������ ����(0~1��)</summary>
    public float SuccessLocation => _successLocation;


    private float _successRangeLevel_S;
    /// <summary>S��� ���� ����(0~1��)</summary>
    public float SuccessRangeLevel_S => _successRangeLevel_S;



    private float _successRangeLevel_A;
    /// <summary>S��� ���� ����(0~1��)</summary>
    public float SuccessRangeLevel_A => _successRangeLevel_A;



    private float _successRangeLevel_B;
    /// <summary>S��� ���� ����(0~1��)</summary>
    public float SuccessRangeLevel_B => _successRangeLevel_B;


    private Item _item;
    public Item Item => _item;

    public RecipeData(string materialItemID, int materialValue, string successItemID,
        string cookware, int cookingAmount, float successLocation,
       float successRangeLevel_S, float successRangeLevel_A, float successRangeLevel_B)
    {
        _materialItemID = materialItemID;
        _materialValue = materialValue;
        _successItemID = successItemID;
        _cookware = cookware;
        _cookingAmount = CookingAmount;
        _successLocation = successLocation;
        _successRangeLevel_S = successRangeLevel_S;
        _successRangeLevel_A = successRangeLevel_A;
        _successRangeLevel_B = successRangeLevel_B;

        List<Item>[] itemList = DatabaseManager.Instance.ItemDatabase.ItemList;

        for (int i = 0, listCount = itemList.Length; i < listCount; i++)
        {
            foreach (Item item in itemList[i])
            {
                if (item.Id != _successItemID)
                    continue;

                _item = item;
                Debug.Log("������");
                return;

            }
        }

        Debug.Log("�������� ����");

    }
}

public class RecipeDatabase
{
    private List<RecipeData> _recipeDataList = new List<RecipeData>();

    private DialogueParser _parser = new DialogueParser();

    public void Register()
    {
        DataParse();
    }

    private void DataParse()
    {
        RecipeData[] recipeDatas = _parser.RecipeDataParse("Recipes");

        for (int i = 0, count = recipeDatas.Length; i < count; i++)
        {
            _recipeDataList.Add(recipeDatas[i]);
        }
    }

    public RecipeData[] GetRecipeDataArray()
    {
        return _recipeDataList.ToArray();
    }
}