using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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


    private int _tool;
    public int Tool => _tool;


    private FoodItem _item;
    public FoodItem Item => _item;

    public RecipeData(List<KeyValuePair<string, int>> materialItemList, string successItemID,
         float successLocation, float successRangeLevel_S, float successRangeLevel_A, float successRangeLevel_B, int tool)
    {
        _materialItemList = materialItemList;
        _successItemID = successItemID;
        _successLocation = successLocation;
        _successRangeLevel_S = successRangeLevel_S;
        _successRangeLevel_A = successRangeLevel_A;
        _successRangeLevel_B = successRangeLevel_B;
        _tool = tool;

        _item = DatabaseManager.Instance.ItemDatabase.GetFoodItemById(_successItemID);
    }
}


public class RecipeDatabase
{
    private List<RecipeData> _recipeDataList = new List<RecipeData>();

    private Dictionary<Tuple<string, string, int>, RecipeData> _recipeDataDic = new Dictionary<Tuple<string, string, int>, RecipeData>();
    public Dictionary<Tuple<string, string, int>, RecipeData> RecipeDataDic => _recipeDataDic;

    private Parser _parser = new Parser();

    private string _chartID => "110099";


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

            string toolToStr = json[i]["Tool"].ToString().Trim();
            int tool = -1; //(enum)Cookware에 연동
            switch (toolToStr)
            {
                case "oven":
                    tool = 0;
                    break;

                case "pan":
                    tool = 1;
                    break;

                case "pot":
                    tool = 2;
                    break;
            }

            RecipeData recipeData = new RecipeData(itemList, successItemID, successLocation,
                successRangeLevel_S, successRangeLevel_A, successRangeLevel_B, tool); //레시피 클래스 생성

            recipeDataList.Add(recipeData);

            Tuple<string, string, int> tuple = Tuple.Create(recipeData.MaterialItemList[0].Key, recipeData.MaterialItemList[1].Key, tool);
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
        if (0 < _recipeDataList.Count)
            return;

        RecipeData[] recipeDatas = _parser.RecipeDataParse("Recipe");

        for (int i = 0, count = recipeDatas.Length; i < count; i++)
        {
            _recipeDataList.Add(recipeDatas[i]);

            string key1 = !string.IsNullOrWhiteSpace(recipeDatas[i].MaterialItemList[0].Key)
                ? recipeDatas[i].MaterialItemList[0].Key
                : "";

            string key2 = !string.IsNullOrWhiteSpace(recipeDatas[i].MaterialItemList[1].Key)
                ? recipeDatas[i].MaterialItemList[1].Key
                : "";

            Tuple<string, string, int> tuple = Tuple.Create(key1, key2, recipeDatas[i].Tool);
            _recipeDataDic.Add(tuple, recipeDatas[i]);
        }
    }

}