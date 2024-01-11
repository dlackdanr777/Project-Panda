using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class UserInfo
{

    //유저 데이터
    //==========================================================================================================

    public string UserId;    //아이디
    public DateTime TODAY => DateTime.Now;    //컴퓨터의 현재 날짜와 시간을 가져옴(추후 서버 시간으로 변경해야함)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //마지막 접속일

    public string _lastAccessDay;

    public int DayCount; //몇일 접속했나?

    public bool IsTodayRewardReceipt; //오늘 아이템을 수령했나?

    public bool IsExistingUser; //기존 유저인가?

    //Inventory
    public List<InventoryData> GatheringInventoryDataArray; //저장할 인벤토리 데이터
    private Inventory[] GatheringItemInventory; //게임 속 인벤토리

    public List<InventoryData> CookInventoryDataArray;
    private Inventory[] CookItemInventory;

    public List<InventoryData> ToolInventoryDataArray;
    private Inventory[] ToolItemInventory;

    //Item
    public List<string> GatheringItemReceived;
    public List<string> NPCItemReceived;
    public List<string> CookItemReceived;
    public List<string> ToolItemReceived;


    //==========================================================================================================

    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        LoadUserInfoData();
    }


    //유저의 데이터를 가져오는 함수
    public void LoadUserInfoData()
    {
        Debug.Log(_path);
        if (!File.Exists(_path))
        {
            Debug.Log("유저 저장 문서가 존재하지 않습니다.");

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
        ToolInventoryDataArray = userInfo.ToolInventoryDataArray;

        GatheringItemReceived = userInfo.GatheringItemReceived;
        ToolItemReceived = userInfo.ToolItemReceived;

        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
        ToolItemInventory = new Inventory[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];

        IsTodayRewardReceipt = true;


        if (TODAY.Day > LastAccessDay.Day)
        {
            Debug.Log("실행");
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
        ToolInventoryDataArray = new List<InventoryData>();
        GatheringItemReceived = new List<string>();
        ToolItemReceived = new List<string>();

        DayCount++;
        IsTodayRewardReceipt = false;


        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }


    public void SaveUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        SaveUserInventory();
        SaveUserReceivedItem();

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
        //GatheringItem
        List<Item> GatheringItems = new List<Item>();
        GatheringItems.AddRange(DatabaseManager.Instance.GetBugItemList());
        GatheringItems.AddRange(DatabaseManager.Instance.GetFishItemList());
        GatheringItems.AddRange(DatabaseManager.Instance.GetFruitItemList());

        for (int i=0;i< GatheringInventoryDataArray.Count; i++) //저장된 데이터
        {
            for (int j = 0; j < GatheringItems.Count; j++) //데이터베이스
            {
                int fieldIndex = -1;
                if (GatheringInventoryDataArray[i].Id.Equals(GatheringItems[j].Id))
                {
                    if (GatheringInventoryDataArray[i].Id.StartsWith("IBG"))
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

        //ToolItem
        List<Item> ToolItems = new List<Item>();
        ToolItems.AddRange(DatabaseManager.Instance.GetGatheringToolItemList());

        for(int i = 0; i < ToolInventoryDataArray.Count; i++)
        {
            for(int j=0;j<ToolItems.Count; j++)
            {
                int fieldIndex = -1;
                if (ToolInventoryDataArray[i].Id.Equals(ToolItems[j].Id))
                {
                    if (ToolInventoryDataArray[i].Id.StartsWith("ITG"))
                    {
                        fieldIndex = 0;
                    }
                    GameManager.Instance.Player.ToolItemInventory[fieldIndex].AddById(InventoryItemField.Tool, fieldIndex, ToolInventoryDataArray[i].Id);
                }
            }
        }

    }

    private void SaveUserInventory()
    {
        GatheringItemInventory = GameManager.Instance.Player.GatheringItemInventory;
        CookItemInventory = GameManager.Instance.Player.CookItemInventory;
        ToolItemInventory = GameManager.Instance.Player.ToolItemInventory;

        if(GatheringInventoryDataArray != null)
        {
            GatheringInventoryDataArray = new List<InventoryData>(); //있던 데이터 지우고 거기에 저장
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
            CookInventoryDataArray = new List<InventoryData>(); //있던 데이터 지우고 거기에 저장
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
            ToolInventoryDataArray = new List<InventoryData>(); //있던 데이터 지우고 거기에 저장
            for (int i = 0; i < ToolItemInventory.Length; i++)
            {
                for (int j = 0; j < ToolItemInventory[i].ItemsCount; j++)
                {
                    ToolInventoryDataArray.Add(new InventoryData(
                        ToolItemInventory[i].GetInventoryList()[j].Id, 1));
                }

            }
        }
    }
    #endregion

    #region Item
    public void LoadUserReceivedItem()
    {
        //GatheringItem
        for (int i = 0; i < GatheringItemReceived.Count; i++)
        {
            if (GatheringItemReceived[i].StartsWith("IBG"))
            {
                for (int j = 0; j < DatabaseManager.Instance.GetBugItemList().Count; j++)
                {
                    if (GatheringItemReceived[i].Equals(DatabaseManager.Instance.GetBugItemList()[j].Id))
                    {
                        DatabaseManager.Instance.GetBugItemList()[j].IsReceived = true;
                    }
                }
            }
            else if (GatheringItemReceived[i].StartsWith("IFI"))
            {
                for (int j = 0; j < DatabaseManager.Instance.GetFishItemList().Count; j++)
                {
                    if (GatheringItemReceived[i].Equals(DatabaseManager.Instance.GetFishItemList()[j].Id))
                    {
                        DatabaseManager.Instance.GetFishItemList()[j].IsReceived = true;
                    }
                }
            }
            else if (GatheringItemReceived[i].StartsWith("IFR"))
            {
                for (int j = 0; j < DatabaseManager.Instance.GetFruitItemList().Count; j++)
                {
                    if (GatheringItemReceived[i].Equals(DatabaseManager.Instance.GetFruitItemList()[j].Id))
                    {
                        DatabaseManager.Instance.GetFruitItemList()[j].IsReceived = true;
                    }
                }
            }
        }

        //ToolItem
        for (int i = 0; i < ToolItemReceived.Count; i++)
        {
            if (ToolItemReceived[i].StartsWith("ITG"))
            {
                for (int j = 0; j < DatabaseManager.Instance.GetGatheringToolItemList().Count; j++)
                {
                    if (GatheringItemReceived[i].Equals(DatabaseManager.Instance.GetGatheringToolItemList()[j].Id))
                    {
                        DatabaseManager.Instance.GetGatheringToolItemList()[j].IsReceived = true;
                    }
                }
            }
        }
    }

    
    private void SaveUserReceivedItem() 
    {
        List<GatheringItem>[] gatheringItemDatabase = { DatabaseManager.Instance.GetBugItemList(), DatabaseManager.Instance.GetFishItemList(), DatabaseManager.Instance.GetFruitItemList() };
        GatheringItemReceived = new List<string>();
        for (int i = 0; i < gatheringItemDatabase.Length; i++)
        {
            for(int j = 0; j < gatheringItemDatabase[i].Count; j++)
            {
                if (gatheringItemDatabase[i][j].IsReceived)
                {
                    GatheringItemReceived.Add(gatheringItemDatabase[i][j].Id);
                }
            }
        }

        List<ToolItem>[] tooItemDatabase = { DatabaseManager.Instance.GetGatheringToolItemList() };
        ToolItemReceived = new List<string>();
        for (int i = 0; i < tooItemDatabase.Length; i++)
        {
            for (int j = 0; j < tooItemDatabase[i].Count; j++)
            {
                if (tooItemDatabase[i][j].IsReceived)
                {
                    ToolItemReceived.Add(tooItemDatabase[i][j].Id);
                }
            }
        }
    }
    #endregion
}
