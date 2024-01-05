using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class UserInfo
{

    //���� ������
    //==========================================================================================================

    public string UserId;    //���̵�
    public DateTime TODAY => DateTime.Now;    //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //������ ������

    public string _lastAccessDay;

    public int DayCount; //���� �����߳�?

    public bool IsTodayRewardReceipt; //���� �������� �����߳�?

    public bool IsExistingUser; //���� �����ΰ�?

    //Inventory

    //public List<InventoryItem>[] GatheringItemInventoryData;
    public Inventory[] GatheringItemInventory; //���� �� �κ��丮
    //public string[] GatheringItemInventoryData;
    public List<InventoryData> GatheringInventoryDataArray; //������ �κ��丮 ������

    public Inventory[] CookItemInventory;
    public List<InventoryData> CookInventoryDataArray;

    public Inventory[] ToolItemInventory;
    public List<InventoryData> ToolInventoryDataArray;


    //==========================================================================================================

    //���� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        LoadUserInfoData();
    }


    //������ �����͸� �������� �Լ�
    public void LoadUserInfoData()
    {
        Debug.Log(_path);
        if (!File.Exists(_path))
        {
            Debug.Log("���� ���� ������ �������� �ʽ��ϴ�.");

            CreateUserInfoData();
            return;
        }

        UserInfo userInfo = new UserInfo();

        string loadJson = File.ReadAllText(_path);
        userInfo = JsonUtility.FromJson<UserInfo>(loadJson);

        UserId = userInfo.UserId;
        string paser = userInfo._lastAccessDay.ToString();
        _lastAccessDay = paser;
        DayCount = userInfo.DayCount;  
        IsExistingUser = userInfo.IsExistingUser;
        GatheringInventoryDataArray = userInfo.GatheringInventoryDataArray;
  
        //GatheringItemInventoryData = new List<InventoryItem>[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];

        IsTodayRewardReceipt = true;


        if (TODAY.Day > LastAccessDay.Day)
        {
            Debug.Log("����");
            DayCount++;
            IsTodayRewardReceipt = false;
        }
    }

    private void CreateUserInfoData()
    {
        IsExistingUser = false;
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
        GatheringInventoryDataArray = new List<InventoryData>();
        //GatheringItemInventoryData = new string[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];

        DayCount++;
        IsTodayRewardReceipt = false;


        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }


    public void SaveUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        //Inventory[] GatheringItemInventory = GameManager.Instance.Player.GatheringItemInventory;
        //for(int i=0;i<GatheringItemInventory.Length;i++)
        //{
        //    Debug.Log("Gathering : " + GatheringItemInventory);
        //    //GatheringItemInventoryData[i] = JsonConvert.SerializeObject(GatheringItemInventory[i].GetInventoryList(), Formatting.Indented, new JsonSerializerSettings
        //    //{
        //    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    //});
        //    //Debug.Log(GatheringItemInventory[i].GetInventoryList()[0].Id);

        //    //GatheringItemInventoryData[i] = new List<InventoryItem>();
        //    //GatheringItemInventoryData[i] = GatheringItemInventory[i].GetInventoryList();


        //}
        SaveUserInventory();

        Debug.Log("g inventory : " + GatheringItemInventory[0].GetInventoryList()[0].Id);
        Debug.Log(GatheringInventoryDataArray[0].Id);

        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }

    #region Inventory
    [Serializable]
    public class InventoryData
    {
        public string Id;
        public int Count;
        public InventoryData(string id, int count) 
        {
            Id = id;
            Count = count;
        }

    }

    public void LoadUserInventory()
    {
        List<Item> GatheringItems = new List<Item>();
        GatheringItems.AddRange(DatabaseManager.Instance.GetBugItemList());
        GatheringItems.AddRange(DatabaseManager.Instance.GetFishItemList());
        GatheringItems.AddRange(DatabaseManager.Instance.GetFruitItemList());

        for (int i=0;i< GatheringInventoryDataArray.Count; i++) //����� ������
        {
            Debug.Log("GatheringInventoryDataArray : " + GatheringInventoryDataArray[i].Id);
            for (int j = 0; j < GatheringItems.Count; j++) //�����ͺ��̽�
            {
                int fieldIndex = -1;
                if (GatheringInventoryDataArray[i].Id.Equals(GatheringItems[j].Id))
                {
                    if (GatheringInventoryDataArray[i].Id.StartsWith("IB"))
                    {
                        fieldIndex = 0;
                    }
                    else if (GatheringInventoryDataArray[i].Id.StartsWith("IFI"))
                    {
                        fieldIndex = 1;
                    }
                    else if (GatheringInventoryDataArray[i].Id.StartsWith("IFR"))
                    {
                        fieldIndex = 2;
                    }
                    for (int k = 0; k < GatheringInventoryDataArray[i].Count; k++)
                    {
                        GameManager.Instance.Player.GatheringItemInventory[fieldIndex].AddById(InventoryItemField.GatheringItem, fieldIndex, GatheringInventoryDataArray[i].Id);
                    }
                }
            }
        }

    }

    private void SaveUserInventory()
    {
        GatheringInventoryDataArray = null;
        CookInventoryDataArray = null;
        ToolInventoryDataArray = null;

        GatheringItemInventory = GameManager.Instance.Player.GatheringItemInventory;
        CookItemInventory = GameManager.Instance.Player.CookItemInventory;
        ToolItemInventory = GameManager.Instance.Player.ToolItemInventory;

        if(GatheringInventoryDataArray != null)
        {
            for(int i=0;i< GatheringItemInventory.Length; i++)
            {
                for(int j=0;j< GatheringItemInventory[i].ItemsCount; j++)
                {
                    GatheringInventoryDataArray.Add(new InventoryData(
                        GatheringItemInventory[i].GetInventoryList()[j].Id,
                        GatheringItemInventory[i].GetInventoryList()[j].Count));
                }
            }
        }

        if (CookInventoryDataArray != null)
        {
            for (int i = 0; i < CookItemInventory.Length; i++)
            {
                for (int j = 0; j < CookItemInventory[i].ItemsCount; j++)
                {
                    CookInventoryDataArray.Add(new InventoryData(
                        CookItemInventory[i].GetInventoryList()[j].Id,
                        CookItemInventory[i].GetInventoryList()[j].Count));
                }
            }
        }

        if (ToolInventoryDataArray != null)
        {
            for (int i = 0; i < ToolItemInventory.Length; i++)
            {
                for (int j = 0; j < ToolItemInventory[i].ItemsCount; j++)
                {
                    ToolInventoryDataArray.Add(new InventoryData(
                        ToolItemInventory[i].GetInventoryList()[j].Id,
                        ToolItemInventory[i].GetInventoryList()[j].Count));
                }

            }
        }
    }
    #endregion
}
