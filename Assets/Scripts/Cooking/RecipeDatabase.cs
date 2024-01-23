using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeData
{
    private List<KeyValuePair<string, int>> _materialItemList;
    /// <summary>조리 필요 아이템</summary>
    public List<KeyValuePair<string, int>> MaterialItemList => _materialItemList;


    private string _successItemID;
    /// <summary>조리 완료 아이템 ID</summary>
    public string SuccessItemID => _successItemID;


    private float _successLocation;
    /// <summary>성공 게이지 기준(0~1값)</summary>
    public float SuccessLocation => _successLocation;


    private float _successRangeLevel_S;
    /// <summary>S등급 성공 기준(0~1값)</summary>
    public float SuccessRangeLevel_S => _successRangeLevel_S;



    private float _successRangeLevel_A;
    /// <summary>S등급 성공 기준(0~1값)</summary>
    public float SuccessRangeLevel_A => _successRangeLevel_A;



    private float _successRangeLevel_B;
    /// <summary>S등급 성공 기준(0~1값)</summary>
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

        //TODO: 차후 레시피 수정 예정
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
                return;

            }
        }
    }
}

public class RecipeDatabase
{
    private List<RecipeData> _recipeDataList = new List<RecipeData>();

    private Dictionary<Tuple<string, string>, RecipeData> _recipeDataDic = new Dictionary<Tuple<string, string>, RecipeData>();
    public Dictionary<Tuple<string, string>, RecipeData> RecipeDataDic => _recipeDataDic;

    private DialogueParser _parser = new DialogueParser();

    private string _chartID => "104967";


    public void Register()
    {
    }


    public void LoadData()
    {
        BackendManager.Instance.GetChartData(_chartID, 10, RecipeParse);
    }


    /// <summary>서버에서 레시피 정보를 받아 List, Dic로 변환하는 함수</summary>
    public void RecipeParse(BackendReturnObject bro)
    {
        JsonData json = bro.FlattenRows();
        List<RecipeData> recipeDataList = new List<RecipeData>(); //리스트 생성

        for (int i = 1; i < json.Count; i++)
        {
            List<KeyValuePair<string, int>> itemList = new List<KeyValuePair<string, int>>();

            if (!string.IsNullOrWhiteSpace(json[i]["MaterialItemID1"].ToString()))
            {
                string materialItemID = json[i]["MaterialItemID1"].ToString();
                int materialValue = int.Parse(json[i]["MaterialItemValue1"].ToString());

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }
            else
            {
                string materialItemID = "";
                int materialValue = 0;

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }

            if (!string.IsNullOrWhiteSpace(json[i]["MaterialItemID2"].ToString()))
            {
                string materialItemID = json[i]["MaterialItemID2"].ToString();
                int materialValue = int.Parse(json[i]["MaterialItemValue2"].ToString());

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }
            else
            {
                string materialItemID = "";
                int materialValue = 0;

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }


            string successItemID = json[i]["CookItemID"].ToString();
            float successLocation = float.Parse(json[i]["Pos"].ToString());
            float successRangeLevel_S = float.Parse(json[i]["RangeS"].ToString());
            float successRangeLevel_A = float.Parse(json[i]["RangeA"].ToString());
            float successRangeLevel_B = float.Parse(json[i]["RangeB"].ToString());

            RecipeData recipeData = new RecipeData(itemList, successItemID, successLocation,
                successRangeLevel_S, successRangeLevel_A, successRangeLevel_B); //레시피 클래스 생성

            recipeDataList.Add(recipeData);

            Tuple<string, string> tuple = Tuple.Create(recipeData.MaterialItemList[0].Key, recipeData.MaterialItemList[1].Key);
            _recipeDataDic.Add(tuple, recipeData);
        }

         _recipeDataList = recipeDataList;

        Debug.Log("레시피 데이터 정보 받기 성공!");
    }


    public RecipeData[] GetRecipeDataArray()
    {
        return _recipeDataList.ToArray();
    }


    /// <summary>로컬에서 레시피 정보를 받아 List, Dic로 변환하는 함수</summary>
    public void LocalRecipeParse()
    {
        if(0 < _recipeDataList.Count)
            return;

        RecipeData[] recipeDatas = _parser.RecipeDataParse("Recipes");

        for (int i = 0, count = recipeDatas.Length; i < count; i++)
        {
            _recipeDataList.Add(recipeDatas[i]);

            Tuple<string, string> tuple = Tuple.Create(recipeDatas[i].MaterialItemList[0].Key, recipeDatas[i].MaterialItemList[1].Key);
            _recipeDataDic.Add(tuple, recipeDatas[i]);
        }
    }
}