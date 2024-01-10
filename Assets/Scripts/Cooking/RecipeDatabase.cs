using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeData
{
    private List<KeyValuePair<string, int>> _materialItemList;
    /// <summary>���� �ʿ� ������</summary>
    public List<KeyValuePair<string, int>> MaterialItemList => _materialItemList;


    private string _successItemID;
    /// <summary>���� �Ϸ� ������ ID</summary>
    public string SuccessItemID => _successItemID;


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

    public RecipeData(List<KeyValuePair<string, int>> materialItemList, string successItemID,
         float successLocation, float successRangeLevel_S, float successRangeLevel_A, float successRangeLevel_B)
    {
        _materialItemList = materialItemList;
        _successItemID = successItemID;
        _successLocation = successLocation;
        _successRangeLevel_S = successRangeLevel_S;
        _successRangeLevel_A = successRangeLevel_A;
        _successRangeLevel_B = successRangeLevel_B;

        //TODO: ���� ������ ���� ����
        List<List<GatheringItem>> itemList = new List<List<GatheringItem>>
        {
            DatabaseManager.Instance.ItemDatabase.ItemFishList,
            DatabaseManager.Instance.ItemDatabase.ItemBugList,
            DatabaseManager.Instance.ItemDatabase.ItemFruitList
        };
        



            for (int i = 0, listCount = itemList.Count; i < listCount; i++)
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

    private Dictionary<Tuple<string, string>, RecipeData> _recipeDataDic = new Dictionary<Tuple<string, string>, RecipeData>();
    public Dictionary<Tuple<string, string>, RecipeData> RecipeDataDic => _recipeDataDic;

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

            Tuple<string, string> tuple = Tuple.Create(recipeDatas[i].MaterialItemList[0].Key, recipeDatas[i].MaterialItemList[1].Key);
            _recipeDataDic.Add(tuple, recipeDatas[i]);
        }
    }

    public RecipeData[] GetRecipeDataArray()
    {
        return _recipeDataList.ToArray();
    }
}