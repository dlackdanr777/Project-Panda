using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    WallPaper,
    Floor,
    LeftWall,
    RightWall,
    LeftFurniture,
    RightFurniture,
    LeftProp,
    RightProp
}

public enum EFurnitureViewType
{
    None = -1,
    WallPaper,
    Floor,
    Wall,
    Furniture,
    Prop
}

public class ItemDatabase 
{
    //public List<Item>[] ItemList = new List<Item>[System.Enum.GetValues(typeof(ItemField)).Length - 1];
    //public int[] ItemCount = new int[System.Enum.GetValues(typeof(ItemField)).Length - 1];

    //Snack
    private List<Dictionary<string, object>> _dataSnack;

    //Furniture
    public FurnitureType[] FurnitureTypeList;
    public Dictionary<string, Furniture> FurnitureDic = new Dictionary<string, Furniture>();
    private List<Dictionary<string, object>> _dataFurniture;

    //GatheringItem
    //Fish
    public List<GatheringItem> ItemFishList = new List<GatheringItem>();
    public Dictionary<string, GatheringItem> ItemFishDic = new Dictionary<string, GatheringItem>();
    //Bug
    public List<GatheringItem> ItemBugList = new List<GatheringItem>();
    public Dictionary<string, GatheringItem> ItemBugDic = new Dictionary<string, GatheringItem>();
    //Fruit
    public List<GatheringItem> ItemFruitList = new List<GatheringItem>();
    public Dictionary<string, GatheringItem> ItemFruitDic = new Dictionary<string, GatheringItem>();
    //ToolItem
    public List<ToolItem> ItemToolList = new List<ToolItem>();


    public Dictionary<string, Item> AllItemDic = new Dictionary<string, Item>();



    //Image
    //GatheringItem
    public ItemSpriteDatabase[] GatheringItemSpriteArray = new ItemSpriteDatabase[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
    public Dictionary<string, Sprite>[] _gatheingItemSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
    //ToolItem
    public ItemSpriteDatabase[] ToolItemSpriteArray = new ItemSpriteDatabase[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];
    public Dictionary<string, Sprite>[] _toolItemSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];

    public void Register()
    {
        //Image
        //GatheringItem
        DatabaseManager database = DatabaseManager.Instance;
        for (int i = 0; i < GatheringItemSpriteArray.Length; i++)
        {
            GatheringItemSpriteArray[i] = database.GatheringItemImages[i];
        }
        //ToolItem
        for (int i = 0; i < ToolItemSpriteArray.Length; i++)
        {
            ToolItemSpriteArray[i] = database.ToolItemImages[i];
        }

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

        _dataFurniture = CSVReader.Read("Furniture");

        BugItemParserByLocal();
        FishItemParserByLocal();
        FruitItemParserByLocal();
        ToolItemParserByLocal();

        //Furniture //아직 수정 중
        Dictionary<char, FurnitureType> FurnitureTypes = new Dictionary<char, FurnitureType>();

        // 가구 종류 설정
        for (int i = 0; i < System.Enum.GetValues(typeof(FurnitureType)).Length - 1; i++)
        {
            FurnitureTypes.Add((char)((int)'A' + i), (FurnitureType)i);
        }

         for (int i = 0; i < _dataFurniture.Count; i++)
        {
            string id = _dataFurniture[i]["ID"].ToString();
            string name = _dataFurniture[i]["이름"].ToString();
            string description = _dataFurniture[i]["설명"].ToString();
            string storyId = _dataFurniture[i]["스토리단계"].ToString();
            int price = (int)_dataFurniture[i]["가격"];
            int layer = (int)_dataFurniture[i]["레이어"];
            FurnitureType type = FurnitureTypes[char.Parse(id.Substring(4, 1))];
            Sprite roomSprite = DatabaseManager.Instance.FurnitureItemImages.ItemSprites[i].Image;
            Sprite sprite = DatabaseManager.Instance.FurnitureItemImages.ItemSprites[i].Thumbnails;

            Furniture data = new Furniture(id, name, description, storyId, price, null, "MN04", layer, type, sprite, roomSprite);
            FurnitureDic.Add(id, data);
        }


        DatabaseManager.Instance.UserInfo.LoadUserReceivedItem();
        DatabaseManager.Instance.StartPandaInfo.LoadMyFurniture();
    }


    public GatheringItem GetGatheringItemById(string id)
    {
        GatheringItem item;

        if(ItemBugDic.TryGetValue(id, out item))
        {
            return item;
        }
        else if(ItemFishDic.TryGetValue(id, out item))
        {
            return item;
        }
        else if(ItemFruitDic.TryGetValue(id, out item))
        {
            return item;
        }
        else
        {
            Debug.LogErrorFormat("{0} ID를 가진 아이템이 존재하지 않습니다.", id);
            return null;
        }
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

    public void LoadData()
    {
        BackendManager.Instance.GetChartData("105320", 10, BugItemParserByServer);
        BackendManager.Instance.GetChartData("105331", 10, FishItemParserByServer);
        BackendManager.Instance.GetChartData("105332", 10, FruitItemParserByServer);
        BackendManager.Instance.GetChartData("105888", 10, ToolItemParserByServer);
    }

    #region Load Server

    /// <summary>서버에서 곤충 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void BugItemParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        ItemBugList.Clear();
        ItemBugDic.Clear();
        AllItemDic.Clear();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string itemID = json[i]["ItemID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            int price = int.Parse(json[i]["Price"].ToString());
            string time = json[i]["Time"].ToString();
            string season = json[i]["Season"].ToString();
            string rating = json[i]["Rating"].ToString();
            string mapID = json[i]["MapID"].ToString();
            Sprite sprite = GetItemSpriteById(itemID, GatheringItemType.Bug);

            GatheringItem item = new GatheringItem(itemID, name, description, price, rating, mapID, sprite, time, season);
            ItemBugList.Add(item);
            ItemBugDic.Add(itemID, item);
            AllItemDic.Add(itemID, item);
        }
        Debug.Log("곤충 아이템 받아오기 성공!");
    }


    /// <summary>서버에서 생선 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void FishItemParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        ItemFishList.Clear();
        ItemFishDic.Clear();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string itemID = json[i]["ItemID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            int price = int.Parse(json[i]["Price"].ToString());
            string time = json[i]["Time"].ToString();
            string season = json[i]["Season"].ToString();
            string rating = json[i]["Rating"].ToString();
            string mapID = json[i]["MapID"].ToString();
            Sprite sprite = GetItemSpriteById(itemID, GatheringItemType.Fish);
            GatheringItem item = new GatheringItem(itemID, name, description, price, rating, mapID, sprite, time, season);

            ItemFishList.Add(item);
            ItemFishDic.Add(itemID, item);
            AllItemDic.Add(itemID, item);
        }
        Debug.Log("생선 아이템 받아오기 성공!");
    }


    /// <summary>서버에서 과일 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void FruitItemParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        ItemFruitList.Clear();
        ItemFruitDic.Clear();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            string itemID = json[i]["ItemID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            int price = int.Parse(json[i]["Price"].ToString());
            string time = json[i]["Time"].ToString();
            string season = json[i]["Season"].ToString();
            string rating = json[i]["Rating"].ToString();
            string mapID = json[i]["MapID"].ToString();
            Sprite sprite = GetItemSpriteById(itemID, GatheringItemType.Fruit);

            GatheringItem item = new GatheringItem(itemID, name, description, price, rating, mapID, sprite, time, season);
            ItemFruitList.Add(item);
            ItemFruitDic.Add(itemID, item);
            AllItemDic.Add(itemID, item);
        }
        Debug.Log("과일 아이템 받아오기 성공!");
    }


    /// <summary>서버에서 장비 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void ToolItemParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        ItemToolList.Clear();
        for (int i = 0; i < json.Count; i++)
        {
            string itemID = json[i]["ItemID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            int price = int.Parse(json[i]["Price"].ToString());
            int gatheringPercentage = int.Parse(json[i]["GatheringPercentage"].ToString());
            int storyStep = int.Parse(json[i]["StoryStep"].ToString());
            string mapID = json[i]["MapID"].ToString();

            ToolItem item = new ToolItem(itemID, name, description, price, mapID, GetItemSpriteById(itemID, ToolItemType.GatheringTool)
                  , gatheringPercentage, storyStep);

            ItemToolList.Add(item);
            AllItemDic.Add(itemID, item);
        }
    }

    #endregion


    #region Load Local

    /// <summary>리소스 폴더에서 곤충 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void BugItemParserByLocal()
    {
        List<Dictionary<string, object>> dataBug = CSVReader.Read("ItemBug");

        for (int i = 0; i < dataBug.Count; i++)
        {
            GatheringItem item = new GatheringItem(
                    dataBug[i]["ID"].ToString(),
                    dataBug[i]["이름"].ToString(),
                    dataBug[i]["설명"].ToString(),
                    (int)dataBug[i]["가격"],
                    dataBug[i]["등급"].ToString(),
                    dataBug[i]["맵 ID"].ToString(),
                    GetItemSpriteById(dataBug[i]["ID"].ToString(), GatheringItemType.Bug),
                    dataBug[i]["시간"].ToString(),
                    dataBug[i]["계절"].ToString()
                    );
            ItemBugList.Add(item);
            ItemBugDic.Add(dataBug[i]["ID"].ToString(), item);
            AllItemDic.Add(item.Id, item);
        }
        Debug.Log("곤충 아이템 받아오기 성공!");
    }


    /// <summary>리소스 폴더에서 생선 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void FishItemParserByLocal()
    {
        List<Dictionary<string, object>> dataFish = CSVReader.Read("ItemFish");

        for (int i = 0; i < dataFish.Count; i++)
        {
            GatheringItem item = new GatheringItem(
                    dataFish[i]["ID"].ToString(),
                    dataFish[i]["이름"].ToString(),
                    dataFish[i]["설명"].ToString(),
                    (int)dataFish[i]["가격"],
                    dataFish[i]["등급"].ToString(),
                    dataFish[i]["맵 ID"].ToString(),
                    GetItemSpriteById(dataFish[i]["ID"].ToString(), GatheringItemType.Fish),
                    dataFish[i]["시간"].ToString(),
                    dataFish[i]["계절"].ToString()
                    );
            ItemFishList.Add(item);
            ItemFishDic.Add(dataFish[i]["ID"].ToString(), item);
            AllItemDic.Add(item.Id, item);
        }
        Debug.Log("생선 아이템 받아오기 성공!");
    }


    /// <summary>리소스 폴더에서 과일 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void FruitItemParserByLocal()
    {
        List<Dictionary<string, object>> dataFruit = CSVReader.Read("ItemFruit");

        for (int i = 0; i < dataFruit.Count; i++)
        {
            GatheringItem item = new GatheringItem(
                    dataFruit[i]["ID"].ToString(),
                    dataFruit[i]["이름"].ToString(),
                    dataFruit[i]["설명"].ToString(),
                    (int)dataFruit[i]["가격"],
                    dataFruit[i]["등급"].ToString(),
                    dataFruit[i]["맵 ID"].ToString(),
                    GetItemSpriteById(dataFruit[i]["ID"].ToString(), GatheringItemType.Fruit),
                    dataFruit[i]["시간"].ToString(),
                    dataFruit[i]["계절"].ToString()
                    );
            ItemFruitList.Add(item);
            ItemFruitDic.Add(dataFruit[i]["ID"].ToString(), item);
            AllItemDic.Add(item.Id, item);
        }

        Debug.Log("과일 아이템 받아오기 성공!");
    }


    /// <summary>리소스 폴더에서 장비 아이템의 정보를 받아와 List에 넣는 함수</summary>
    public void ToolItemParserByLocal()
    {
        List<Dictionary<string, object>>  dataTool = CSVReader.Read("ItemTool");

        for (int i = 0; i < dataTool.Count; i++)
        {
            ToolItem item = new ToolItem(
                    dataTool[i]["ID"].ToString(),
                    dataTool[i]["이름"].ToString(),
                    dataTool[i]["설명"].ToString(),
                    (int)dataTool[i]["가격"],
                    dataTool[i]["맵 ID"].ToString(),
                    GetItemSpriteById(dataTool[i]["ID"].ToString(), ToolItemType.GatheringTool),
                    (int)dataTool[i]["채집 확률"],
                    (int)dataTool[i]["스토리 단계"]
                    );

            ItemToolList.Add(item);
            AllItemDic.Add(item.Id, item);
        }
        Debug.Log("장비 아이템 받아오기 성공!");
    }

    #endregion
}