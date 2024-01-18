using BackEnd;
using BackEnd.MultiCharacter;
using LitJson;
using Muks.BackEnd;
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
    public List<InventoryData> GatheringInventoryDataArray = new List<InventoryData>(); //저장할 인벤토리 데이터
    private Inventory[] GatheringItemInventory; //게임 속 인벤토리

    public List<InventoryData> CookInventoryDataArray = new List<InventoryData>();
    private Inventory[] CookItemInventory;

    public List<InventoryData> ToolInventoryDataArray = new List<InventoryData>();
    private Inventory[] ToolItemInventory;

    //Item
    public List<string> GatheringItemReceived = new List<string>();
    public List<string> NPCItemReceived = new List<string>();
    public List<string> CookItemReceived = new List<string>();
    public List<string> ToolItemReceived = new List<string>();
    public List<string> AlbumReceived = new List<string>();

    //Message
    public List<MessageData> MessageDataArray = new List<MessageData>(); //저장할 메시지 데이터
    private MessageList[] MessageLists; //게임 속 메시지리스트


    //==========================================================================================================

    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        CreateUserInfoData();
    }


    public void CreateUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        //string json = JsonUtility.ToJson(this, true);
        //File.WriteAllText(_path, json);
        //TODO: 모바일 테스트 중 잠금
    }


    public void LoadUserInfoData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            UserId = json[0]["UserId"].ToString();
            DayCount = int.Parse(json[0]["DayCount"].ToString());
            _lastAccessDay = json[0]["LastAccessDay"].ToString();
            IsTodayRewardReceipt = (bool)json[0]["IsTodayRewardReceipt"];
            IsExistingUser = (bool)json[0]["IsExistingUser"];

            for (int i = 0; i < json[0]["GatheringInventoryDataArray"].Count; i++)
            {
                InventoryData item = JsonUtility.FromJson<InventoryData>(json[0]["GatheringInventoryDataArray"][i].ToJson());
                GatheringInventoryDataArray.Add(item);
            }

            for (int i = 0, count = json[0]["CookInventoryDataArray"].Count; i < count; i++)
            {
                InventoryData item = JsonUtility.FromJson<InventoryData>(json[0]["CookInventoryDataArray"][i].ToJson());
                CookInventoryDataArray.Add(item);
            }

            for (int i = 0, count = json[0]["ToolInventoryDataArray"].Count; i < count; i++)
            {
                InventoryData item = JsonUtility.FromJson<InventoryData>(json[0]["ToolInventoryDataArray"][i].ToJson());
                ToolInventoryDataArray.Add(item);
            }


            for (int i = 0, count = json[0]["GatheringItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["GatheringItemReceived"][i].ToString();
                GatheringItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["NPCItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["NPCItemReceived"][i].ToString();
                NPCItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["CookItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["CookItemReceived"][i].ToString();
                CookItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["ToolItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["ToolItemReceived"][i].ToString();
                ToolItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["AlbumReceived"].Count; i < count; i++)
            {
                string item = json[0]["AlbumReceived"][i].ToString();
                AlbumReceived.Add(item);
            }

            for (int i = 0, count = json[0]["MessageDataArray"].Count; i < count; i++)
            {
                MessageData item = JsonUtility.FromJson<MessageData>(json[0]["MessageDataArray"][i].ToJson());
                MessageDataArray.Add(item);
            }

            LoadUserInventory();
            Debug.Log("Load성공");
        }
    }


    public void SaveUserInfoData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "UserInfo";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject callback = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        if (callback.IsSuccess())
        {
            if (callback.GetReturnValuetoJSON() != null)
            {
                if (callback.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
                    InsertUserInfoData();
                }
                else
                {
                    UpdateUserInfoData(callback.GetInDate());
                }
            }
            else
            {
                InsertUserInfoData();
            }

            Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
        }
        else
        {
            if (callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
            {
                SaveUserInfoData(maxRepeatCount - 1);
            }
            else if (callback.IsServerError()) // 서버의 이상 발생 시
            {
                SaveUserInfoData(maxRepeatCount - 1);
            }
            else if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
            {
                //점검 팝업창 + 로그인 화면으로 보내기
                Debug.Log("게임 점검중입니다.");
                return;
            }
            else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
            {
                //단기간에 많은 요청을 보내면 발생합니다. 5분동안 뒤끝의 함수 요청을 중지해야합니다.  
                return;
            }
            else if (callback.IsBadAccessTokenError())
            {
                bool isRefreshSuccess = BackendManager.Instance.RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                if (isRefreshSuccess)
                {
                    SaveUserInfoData(maxRepeatCount - 1);
                }
                else
                {
                    Debug.Log("토큰을 받지 못했습니다.");
                }
            }
        }
    }


    public void InsertUserInfoData()
    {
        SaveUserInventory();
        SaveUserReceivedItem();
        SaveUserReceived();
        SaveUserMailData();

        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("게임 정보 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert("UserInfo", 10, param);
    }


    public void UpdateUserInfoData(string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        SaveUserInventory();
        SaveUserReceivedItem();
        SaveUserReceived();
        SaveUserMailData();

        Param param = GetUserInfoParam();

        Debug.LogFormat("게임 정보 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate("UserInfo", inDate, 10, param);
    }


    public Param GetUserInfoParam()
    {
        Param param = new Param();

        param.Add("UserId", UserId);
        param.Add("DayCount", DayCount);
        param.Add("LastAccessDay", _lastAccessDay);
        param.Add("IsTodayRewardReceipt", IsTodayRewardReceipt);
        param.Add("IsExistingUser", IsExistingUser);

        param.Add("GatheringInventoryDataArray", GatheringInventoryDataArray);
        param.Add("CookInventoryDataArray", CookInventoryDataArray);
        param.Add("ToolInventoryDataArray", ToolInventoryDataArray);

        param.Add("GatheringItemReceived", GatheringItemReceived);
        param.Add("NPCItemReceived", NPCItemReceived);
        param.Add("CookItemReceived", CookItemReceived);
        param.Add("ToolItemReceived", ToolItemReceived);
        param.Add("AlbumReceived", AlbumReceived);

        param.Add("MessageDataArray", MessageDataArray);

        return param;
    }

    #region Inventory
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

        List<ToolItem>[] toolItemDatabase = { DatabaseManager.Instance.GetGatheringToolItemList() };
        ToolItemReceived = new List<string>();
        for (int i = 0; i < toolItemDatabase.Length; i++)
        {
            for (int j = 0; j < toolItemDatabase[i].Count; j++)
            {
                if (toolItemDatabase[i][j].IsReceived)
                {
                    ToolItemReceived.Add(toolItemDatabase[i][j].Id);
                }
            }
        }
    }
    #endregion

    #region Mail
    public class MessageData
    {
        public string Id;
        public bool IsCheck;
        public bool IsReceived;
        public MessageData(string id, bool isCheck, bool isReceived)
        {
            Id = id;
            IsCheck = isCheck;
            IsReceived = isReceived;
        }
    }

    public void SaveUserMailData()
    {
        MessageLists = GameManager.Instance.Player.Messages;

        if (MessageLists != null)
        {
            MessageDataArray = new List<MessageData>();
            for (int i = 0; i < MessageLists.Length; i++)
            {
                for (int j = 0; j < MessageLists[i].MessagesCount; j++)
                {
                    MessageDataArray.Add(new MessageData(
                        MessageLists[i].GetMessageList()[j].Id,
                        MessageLists[i].GetMessageList()[j].IsCheck,
                        MessageLists[i].GetMessageList()[j].IsReceived));
                }

            }
        }
    }

    public void LoadUserMailData()
    {
        for(int i = 0; i < MessageDataArray.Count; i++)
        {
            if (MessageDataArray[i].Id.StartsWith("ML"))
            {
                Debug.Log(MessageDataArray[i].Id);
                GameManager.Instance.Player.Messages[0].AddById(MessageDataArray[i].Id, MessageField.Mail);
                GameManager.Instance.Player.Messages[0].GetMessageList()[i].IsCheck = MessageDataArray[i].IsCheck;
                GameManager.Instance.Player.Messages[0].GetMessageList()[i].IsReceived = MessageDataArray[i].IsReceived;
            }
        }
    }
    #endregion

    #region NPC, Album
    public void LoadUserReceivedNPC()
    {
        //NPC
        for (int i = 0; i < NPCItemReceived.Count; i++)
        {
            for (int j = 0; j < DatabaseManager.Instance.GetNPCList().Count; j++)
            {
                if (NPCItemReceived[i].Equals(DatabaseManager.Instance.GetNPCList()[j].Id))
                {
                    DatabaseManager.Instance.GetNPCList()[j].IsReceived = true;
                }
            }

        }
    }

    public void LoadUserReceivedAlbum()
    {
        // Album
        for (int i = 0; i < AlbumReceived.Count; i++)
        {
            for (int j = 0; j < DatabaseManager.Instance.GetAlbumList().Count; j++)
            {
                if (AlbumReceived[i].Equals(DatabaseManager.Instance.GetAlbumList()[j].Id))
                {
                    DatabaseManager.Instance.GetAlbumList()[j].IsReceived = true;
                }
            }

        }
    }

    private void SaveUserReceived()
    {
        List<NPC>[] npcDatabase = { DatabaseManager.Instance.GetNPCList() };
        NPCItemReceived = new List<string>();
        for (int i = 0; i < npcDatabase.Length; i++)
        {
            for (int j = 0; j < npcDatabase[i].Count; j++)
            {
                if (npcDatabase[i][j].IsReceived)
                {
                    NPCItemReceived.Add(npcDatabase[i][j].Id);
                }
            }
        }

        List<Album> albumDatabase = DatabaseManager.Instance.GetAlbumList();
        AlbumReceived = new List<string>();
        for (int i = 0; i < albumDatabase.Count; i++)
        {
            if (albumDatabase[i].IsReceived)
            {
                AlbumReceived.Add(albumDatabase[i].Id);
            }
        }
    }
    #endregion
}