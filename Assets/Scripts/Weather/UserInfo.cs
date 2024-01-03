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

    //public List<InventoryItem>[] GatheringItemInventoryData;
    public Inventory[] GatheringItemInventory; //게임 속 인벤토리
    //public string[] GatheringItemInventoryData;
    public List<InventoryData> GatheringInventoryDataArray; //저장할 인벤토리 데이터

    public Inventory[] CookItemInventory;
    public List<InventoryData> CookInventoryDataArray;

    public Inventory[] ToolItemInventory;
    public List<InventoryData> ToolInventoryDataArray;


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
  
        //GatheringItemInventoryData = new List<InventoryItem>[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];
        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1];

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

        for (int i=0;i< GatheringInventoryDataArray.Count; i++) //저장된 데이터
        {
            Debug.Log("GatheringInventoryDataArray : " + GatheringInventoryDataArray[i].Id);
            for (int j = 0; j < GatheringItems.Count; j++) //데이터베이스
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
