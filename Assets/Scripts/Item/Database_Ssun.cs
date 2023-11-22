using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum Field
{
    None = -1,
    Toy,
    Snack,
    Furniture
}

public class Database_Ssun : SingletonHandler<Database_Ssun>
{
    private List<Dictionary<string, object>> DataSnack;
    public List<Item>[] ItemList = new List<Item>[2];
    public int[] ItemCount = new int[2];

    public Sprite Test;

    public override void Awake()
    {
        base.Awake();
        DataSnack = CSVReader.Read("Snack");

        for (int i = 0; i < ItemList.Length; i++)
        {
            ItemList[i] = new List<Item>();
            for (int j = 0; j < DataSnack.Count; j++)
            {
                ItemList[i].Add(new Item(DataSnack[j]["Id"].ToString(),
                    DataSnack[j]["Name"].ToString(),
                    DataSnack[j]["Description"].ToString(),
                    (int)DataSnack[i]["Price"],
                    Test)); //아직 이미지는 받아오지 않음
            }
            ItemCount[i] = ItemList[i].Count;

        }
    }
}
