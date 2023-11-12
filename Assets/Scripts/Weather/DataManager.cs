using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class EverydayData
{
    public int Id;
    public int Date;
    public int Item;
    public int Amount;
}

public class ItemData
{
    public int Id;
    public string Name;
    public string Iamge;
    public int Amount;
}



public class DataManager : SingletonHandler<DataManager>
{
    public Dictionary<int, ItemData> dicItemData;
    public Dictionary<int, EverydayData> dicEverydayData;

    public override void Awake()
    {
        dicItemData = new Dictionary<int, ItemData>();
        dicEverydayData = new Dictionary<int, EverydayData>();
    }

    public bool LoadDatas()
    {
        bool reuslt = false;
        string path = string.Empty;
        string json = string.Empty;

        try
        {
            path = "./data/everyday_data.json";

            //지정된 위치에 파일이 있는지 확인
            if (File.Exists(path))
            {
                //지정된 위치에 파일의 텍스트를 불러와 json변수에 넣는다.
                json = File.ReadAllText(path);

                //Netonsoft.Json라이브러리 사용
                //JSON을 EverydayData[]형식으로 역직렬화하여 딕셔너리에 넣는다.
                dicEverydayData = JsonConvert.DeserializeObject<EverydayData[]>(json).ToDictionary(v => v.Id, v => v);
            }

            path = "./data/item_data.json";

            if (File.Exists(path))
            {
                //지정된 위치에 파일의 텍스트를 불러와 json변수에 넣는다.
                json = File.ReadAllText(path);

                //Netonsoft.Json라이브러리 사용
                //JSON을 ItemData[]형식으로 역직렬화하여 딕셔너리에 넣는다.
                dicItemData = JsonConvert.DeserializeObject<ItemData[]>(json).ToDictionary(v => v.Id, v => v);
            }
            reuslt = true;
        }

        catch(Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

        return reuslt;
    }

    public Dictionary<int, EverydayData> GetEverydayDatas()
    {
        return dicEverydayData;
    }


    public EverydayData GetEverydayData(int id)
    {
        if (dicEverydayData.TryGetValue(id, out EverydayData everydayData))
        {
            return everydayData;
        }

        Debug.LogError("존재하지 않는 ID입니다.");
        return default;
    }

    public Dictionary<int, ItemData> GetItemDatas()
    {
        return dicItemData;
    }

    public ItemData GetItemData(int id)
    {
        if (dicItemData.TryGetValue(id, out ItemData itemData))
        {
            return itemData;
        }

        Debug.LogError("존재하지 않는 ID입니다.");
        return default;
    }

}
