using System;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum InventoryItemField
{
    None = -1,
    GatheringItem,
    Cook,
    Tool,
    Furniture
}

public enum CookItemType
{
    None = -1,
}

public enum ToolItemType
{
    None = - 1,
    GatheringTool

}

public enum GatheringItemType
{
    None = -1,
    Bug,
    Fish,
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
    public List<GatheringItem> ItemFishList = new List<GatheringItem>();
    private List<Dictionary<string, object>> _dataFish;
    //Bug
    public List<GatheringItem> ItemBugList = new List<GatheringItem>();
    private List<Dictionary<string, object>> _dataBug;
    //Fruit
    public List<GatheringItem> ItemFruitList = new List<GatheringItem>();
    private List<Dictionary<string, object>> _dataFruit;

    //ToolItem
    public List<ToolItem> ItemToolList = new List<ToolItem>();
    private List<Dictionary<string, object>> _dataTool;

    //Image
    //GatheringItem
    public ItemSpriteDatabase[] GatheringItemSpriteArray = new ItemSpriteDatabase[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
    public Dictionary<string, Sprite>[] _gatheingItemSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
    //ToolItem
    public ItemSpriteDatabase[] ToolItemSpriteArray = new ItemSpriteDatabase[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];
    public Dictionary<string, Sprite>[] _toolItemSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];


    private Sprite Test;

    public void Register()
    {
        //Image
        //GatheringItem
        for (int i = 0; i < _gatheingItemSpriteDic.Length; i++)
        {
            _gatheingItemSpriteDic[i] = new Dictionary<string, Sprite>();
            for(int j = 0; j < GatheringItemSpriteArray[i].ItemSprites.Length; j++)
            {
                _gatheingItemSpriteDic[i].Add(GatheringItemSpriteArray[i].ItemSprites[j].Id, GatheringItemSpriteArray[i].ItemSprites[j].Image);
            }
        }
        //ToolItem
        for (int i = 0; i < _toolItemSpriteDic.Length; i++)
        {
            _toolItemSpriteDic[i] = new Dictionary<string, Sprite>();
            for (int j = 0; j < ToolItemSpriteArray[i].ItemSprites.Length; j++)
            {
                _toolItemSpriteDic[i].Add(ToolItemSpriteArray[i].ItemSprites[j].Id, ToolItemSpriteArray[i].ItemSprites[j].Image);
            }
        }

        //GatheringItem
        _dataBug = CSVReader.Read("ItemBug");
        _dataFish = CSVReader.Read("ItemFish");
        _dataFruit = CSVReader.Read("ItemFruit");
        //ToolItem
        _dataTool = CSVReader.Read("ItemTool");
        _dataFurniture = CSVReader.Read("Furniture");

        #region Gathering Item
        //Bug
        for (int i = 0; i < _dataBug.Count; i++)
        {
            ItemBugList.Add(new GatheringItem(
                    _dataBug[i]["ID"].ToString(),
                    _dataBug[i]["이름"].ToString(),
                    _dataBug[i]["설명"].ToString(),
                    (int)_dataBug[i]["가격"],
                    _dataBug[i]["등급"].ToString(),
                    _dataBug[i]["맵 ID"].ToString(),
                    GetItemSpriteById(_dataBug[i]["ID"].ToString(), GatheringItemType.Bug),
                    _dataBug[i]["시간"].ToString(),
                    _dataBug[i]["계절"].ToString()
                    ));
        }

        //Fish
        for (int i = 0; i < _dataFish.Count; i++)
        {
            ItemFishList.Add(new GatheringItem(
                    _dataFish[i]["ID"].ToString(),
                    _dataFish[i]["이름"].ToString(),
                    _dataFish[i]["설명"].ToString(),
                    (int)_dataFish[i]["가격"],
                    _dataFish[i]["등급"].ToString(),
                    _dataFish[i]["맵 ID"].ToString(),
                    GetItemSpriteById(_dataFish[i]["ID"].ToString(), GatheringItemType.Fish),
                    _dataFish[i]["시간"].ToString(),
                    _dataFish[i]["계절"].ToString()
                    ));   
        }

        //Fruit
        for (int i = 0; i < _dataFruit.Count; i++)
        {
            ItemFruitList.Add(new GatheringItem(
                    _dataFruit[i]["ID"].ToString(),
                    _dataFruit[i]["이름"].ToString(),
                    _dataFruit[i]["설명"].ToString(),
                    (int)_dataFruit[i]["가격"],
                    _dataFruit[i]["등급"].ToString(),
                    _dataFruit[i]["맵 ID"].ToString(),
                    GetItemSpriteById(_dataFruit[i]["ID"].ToString(), GatheringItemType.Fruit),
                    _dataFruit[i]["시간"].ToString(),
                    _dataFruit[i]["계절"].ToString()
                    ));
        }
        #endregion

        #region Tool Item
        for (int i = 0; i < _dataTool.Count; i++)
        {
            ItemToolList.Add(new ToolItem(
                    _dataTool[i]["ID"].ToString(),
                    _dataTool[i]["이름"].ToString(),
                    _dataTool[i]["설명"].ToString(),
                    (int)_dataTool[i]["가격"],
                    _dataTool[i]["맵 ID"].ToString(),
                    GetItemSpriteById(_dataTool[i]["ID"].ToString(), ToolItemType.GatheringTool),
                    (int)_dataTool[i]["채집 확률"],
                    (int)_dataTool[i]["스토리 단계"]
                    ));
        }
        #endregion

        //Furniture //아직 수정 중
        FurnitureTypeList = new FurnitureType[_dataFurniture.Count];
        for(int i = 0; i < _dataFurniture.Count; i++)
        {
            FurnitureList.Add(new Item(_dataFurniture[i]["Id"].ToString(),
                    _dataFurniture[i]["Name"].ToString(),
                    _dataFurniture[i]["Description"].ToString(),
                    (int)_dataFurniture[i]["Price"],
                    "등급",
                    "맵",
                    Test)); //아직 이미지는 받아오지 않음
            FurnitureTypeList[i] = (FurnitureType)Enum.Parse(typeof(FurnitureType), _dataFurniture[i]["FurnitureType"].ToString());
        }

        DatabaseManager.Instance.UserInfo.LoadUserReceivedItem();
    }

    private Sprite GetItemSpriteById(string id, GatheringItemType type)
    {
        Sprite sprite = _gatheingItemSpriteDic[(int)type][id];
        return sprite;
    }

    private Sprite GetItemSpriteById(string id, ToolItemType type)
    {
        Sprite sprite = _toolItemSpriteDic[(int)type][id];
        return sprite;
    }
}