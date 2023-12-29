using System;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum InventoryItemField
{
    None = -1,
    GatheringItem,
    Snack,
    Tool,
    Furniture
}

public enum ToolItemType
{
    None = - 1,

}

public enum GatheringItemType
{
    None = -1,
    Fish,
    Bug,
    Fruit
}

public enum FurnitureType
{
    None = -1,
    Furniture,
    Appliances,
    Kitchen,
    Light,
    Props
}

public enum MessageField
{
    None = -1,
    Mail,
    Wish
}

public class ItemDatabase 
{
    //public List<Item>[] ItemList = new List<Item>[System.Enum.GetValues(typeof(ItemField)).Length - 1];
    //public int[] ItemCount = new int[System.Enum.GetValues(typeof(ItemField)).Length - 1];

    //Snack
    private List<Dictionary<string, object>> _dataSnack;

    //Furniture
    public FurnitureType[] FurnitureTypeList;
    public List<Item> FurnitureList = new List<Item>();
    private List<Dictionary<string, object>> _dataFurniture;

    //GatheringItem
    //Fish
    public List<Item> ItemFishList = new List<Item>();
    private List<Dictionary<string, object>> _dataFish;
    //Bug
    public List<Item> ItemBugList = new List<Item>();
    private List<Dictionary<string, object>> _dataBug;
    //Fruit
    public List<Item> ItemFruitList = new List<Item>();
    private List<Dictionary<string, object>> _dataFruit;


    public Sprite Test;

    public void Register()
    {
        _dataFurniture = CSVReader.Read("Furniture");

        //GatheringItem
        _dataFish = CSVReader.Read("ItemFish");
        _dataBug = CSVReader.Read("ItemBug");
        _dataFruit = CSVReader.Read("ItemFruit");

        //Gathering Item
        //Fish
        for (int i = 0; i < _dataFish.Count; i++)
        {
            ItemFishList.Add(new Item(_dataFish[i]["ID"].ToString(),
                    _dataFish[i]["이름"].ToString(),
                    _dataFish[i]["설명"].ToString(),
                    (int)_dataFish[i]["가격"],
                    Test)); //아직 이미지는 받아오지 않음
        }

        //Bug
        for (int i = 0; i < _dataBug.Count; i++)
        {
            ItemBugList.Add(new Item(_dataBug[i]["ID"].ToString(),
                    _dataBug[i]["이름"].ToString(),
                    _dataBug[i]["설명"].ToString(),
                    (int)_dataBug[i]["가격"],
                    Test)); //아직 이미지는 받아오지 않음
        }

        //Fruit
        for (int i = 0; i < _dataFruit.Count; i++)
        {
            ItemFruitList.Add(new Item(_dataFruit[i]["ID"].ToString(),
                    _dataFruit[i]["이름"].ToString(),
                    _dataFruit[i]["설명"].ToString(),
                    (int)_dataFruit[i]["가격"],
                    Test)); //아직 이미지는 받아오지 않음
        }

        //Furniture
        FurnitureTypeList = new FurnitureType[_dataFurniture.Count];
        for(int i = 0; i < _dataFurniture.Count; i++)
        {
            FurnitureList.Add(new Item(_dataFurniture[i]["Id"].ToString(),
                    _dataFurniture[i]["Name"].ToString(),
                    _dataFurniture[i]["Description"].ToString(),
                    (int)_dataFurniture[i]["Price"],
                    Test)); //아직 이미지는 받아오지 않음
            FurnitureTypeList[i] = (FurnitureType)Enum.Parse(typeof(FurnitureType), _dataFurniture[i]["FurnitureType"].ToString());
        }
    }
}