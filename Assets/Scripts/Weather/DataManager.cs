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

            //������ ��ġ�� ������ �ִ��� Ȯ��
            if (File.Exists(path))
            {
                //������ ��ġ�� ������ �ؽ�Ʈ�� �ҷ��� json������ �ִ´�.
                json = File.ReadAllText(path);

                //Netonsoft.Json���̺귯�� ���
                //JSON�� EverydayData[]�������� ������ȭ�Ͽ� ��ųʸ��� �ִ´�.
                dicEverydayData = JsonConvert.DeserializeObject<EverydayData[]>(json).ToDictionary(v => v.Id, v => v);
            }

            path = "./data/item_data.json";

            if (File.Exists(path))
            {
                //������ ��ġ�� ������ �ؽ�Ʈ�� �ҷ��� json������ �ִ´�.
                json = File.ReadAllText(path);

                //Netonsoft.Json���̺귯�� ���
                //JSON�� ItemData[]�������� ������ȭ�Ͽ� ��ųʸ��� �ִ´�.
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

        Debug.LogError("�������� �ʴ� ID�Դϴ�.");
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

        Debug.LogError("�������� �ʴ� ID�Դϴ�.");
        return default;
    }

}
