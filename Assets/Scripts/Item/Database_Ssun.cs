using System;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum ItemField
{
    None = -1,
    Toy,
    Snack,
    Furniture
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

public class Database_Ssun : SingletonHandler<Database_Ssun>
{
    //Snack
    private List<Dictionary<string, object>> DataSnack;
    public List<Item>[] ItemList = new List<Item>[3];
    public int[] ItemCount = new int[3];

    //Furniture
    private List<Dictionary<string, object>> DataFurniture;
    public FurnitureType[] FurnitureTypeList;

    public Sprite Test;

    public override void Awake()
    {
        base.Awake();
        DataSnack = CSVReader.Read("Snack");
        DataFurniture = CSVReader.Read("Furniture");

        //snack, toy
        for (int i = 0; i < ItemList.Length -1; i++)
        {
            ItemList[i] = new List<Item>();
            for (int j = 0; j < DataSnack.Count; j++)
            {
                ItemList[i].Add(new Item(DataSnack[j]["Id"].ToString(),
                    DataSnack[j]["Name"].ToString(),
                    DataSnack[j]["Description"].ToString(),
                    (int)DataSnack[i]["Price"],
                    ItemField.Snack,
                    Test)); //아직 이미지는 받아오지 않음
            }
            ItemCount[i] = ItemList[i].Count;

        }

        //Furniture
        FurnitureTypeList = new FurnitureType[DataFurniture.Count];
        ItemList[2] = new List<Item>();
        for(int i = 0; i < DataFurniture.Count; i++)
        {
            ItemList[2].Add(new Item(DataFurniture[i]["Id"].ToString(),
                    DataFurniture[i]["Name"].ToString(),
                    DataFurniture[i]["Description"].ToString(),
                    (int)DataFurniture[i]["Price"],
                    ItemField.Furniture,
                    Test)); //아직 이미지는 받아오지 않음
            FurnitureTypeList[i] = (FurnitureType)Enum.Parse(typeof(FurnitureType), DataFurniture[i]["FurnitureType"].ToString());
        }
        ItemCount[2] = ItemList[2].Count;
    }
}