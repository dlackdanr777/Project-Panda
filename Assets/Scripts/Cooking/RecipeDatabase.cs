using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeData
{
    private string _materialItemID;
    public string MaterialItemID => _materialItemID;

    private int _materialValue;
    public int MaterialValue => _materialValue;

    private string _cookingItemIDLevel_B;
    public string CookingItemIDLevel_B => _cookingItemIDLevel_B;
    private string _cookingItemIDLevel_A;
    public string CookingItemIDLevel_A => _cookingItemIDLevel_A;
    private string _cookingItemIDLevel_S;
    public string CookingItemIDLevel_S => _cookingItemIDLevel_S;

    public RecipeData(string materialItemID, int materialValue,
        string cookingItemIDLevel_B, string cookingItemIDLevel_A, string cookingItemIDLevel_S)
    {
        _materialItemID = materialItemID;
        _materialValue = materialValue;
        _cookingItemIDLevel_B = cookingItemIDLevel_B;
        _cookingItemIDLevel_A = cookingItemIDLevel_A;
        _cookingItemIDLevel_S = cookingItemIDLevel_S;
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