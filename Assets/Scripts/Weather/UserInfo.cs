using BackEnd;
using BackEnd.MultiCharacter;
using LitJson;
using Muks.BackEnd;
using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    public List<InventoryData> GatheringInventoryDataArray = new List<InventoryData>(); //������ �κ��丮 ������
    private Inventory[] GatheringItemInventory; //���� �� �κ��丮

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
    public List<MessageData> MessageDataArray = new List<MessageData>(); //������ �޽��� ������
    private MessageList[] MessageLists; //���� �� �޽�������Ʈ


    //Sticker
    public List<string> StickerReceived = new List<string>();
    public List<StickerData> StickerDataArray = new List<StickerData>();

    //Challenges
    public List<string> ChallengeDoneId = new List<string>(); // �������� �Ϸ�
    public List<string> ChallengeClearId = new List<string>(); // �������� �Ϸ� �� Ŭ��

    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� �������� �ε���

    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��
    public int[] ChallengesCount = new int[7]; // �������� ���� Ƚ��

    //==========================================================================================================
    //���� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";

    //���ʹϾ� ���� ������ �ø� �� �����Ƿ� �߰��� ������ �� Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();


    public void Register()
    {
        CreateUserInfoData();
    }


    private void CreateUserInfoData()
    {

        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
    }


    #region SaveAndLoadUserInfo
    public void LoadUserInfoData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            UserId = json[0]["UserId"].ToString();
            DayCount = int.Parse(json[0]["DayCount"].ToString());
            _lastAccessDay = json[0]["LastAccessDay"].ToString();
            IsTodayRewardReceipt = (bool)json[0]["IsTodayRewardReceipt"];
            IsExistingUser = (bool)json[0]["IsExistingUser"];

            // �������� ����
            for(int i = 0, count = json[0]["ChallengesNum"].Count; i < count; i++)
            {
                ChallengesNum[i] = int.Parse(json[0]["ChallengesNum"][i].ToString());
            }
            for (int i = 0, count = json[0]["GatheringSuccessCount"].Count; i < count; i++)
            {
                GatheringSuccessCount[i] = int.Parse(json[0]["GatheringSuccessCount"][i].ToString());
            }
            for (int i = 0, count = json[0]["ChallengesCount"].Count; i < count; i++)
            {
                ChallengesCount[i] = int.Parse(json[0]["ChallengesCount"][i].ToString());
            }
            for (int i = 0, count = json[0]["ChallengeDoneId"].Count; i < count; i++)
            {
                ChallengeDoneId.Add(json[0]["ChallengeDoneId"][i].ToString());
            }

            for (int i = 0, count = json[0]["ChallengeClearId"].Count; i < count; i++)
            {
                ChallengeClearId.Add(json[0]["ChallengeClearId"][i].ToString());
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

            LoadUserReceivedAlbum();
            Debug.Log("UserInfo Load����");
        }
    }


    public void SaveUserInfoData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "UserInfo";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;

            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;

            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
                SaveUserInfoData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertUserInfoData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateUserInfoData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertUserInfoData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertUserInfoData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        SaveUserMailData();
        SaveUserChallengesData();
        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        SaveUserChallengesData();
        SaveUserMailData();

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetUserInfoParam()
    {
        Param param = new Param();

        param.Add("UserId", UserId);
        param.Add("DayCount", DayCount);
        param.Add("LastAccessDay", _lastAccessDay);
        param.Add("IsTodayRewardReceipt", IsTodayRewardReceipt);
        param.Add("IsExistingUser", IsExistingUser);

        param.Add("ChallengesNum", ChallengesNum);
        param.Add("GatheringSuccessCount", GatheringSuccessCount);
        param.Add("ChallengesCount", ChallengesCount);
        param.Add("ChallengeDoneId", ChallengeDoneId);
        param.Add("ChallengeClearId", ChallengeClearId);

        param.Add("AlbumReceived", AlbumReceived);
        param.Add("MessageDataArray", MessageDataArray);

        return param;
    }

    #endregion

    #region SaveAndLoadInventory

    public void LoadInventoryData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
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

            LoadUserInventory();
            LoadUserReceivedNPC();
            Debug.Log("Inventory Load����");
        }
    }


    public void SaveInventoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Inventory";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;

            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;

            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
                SaveInventoryData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertInventoryData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateInventoryData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertInventoryData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> ���� �κ��丮 ������ ���� �Լ� </summary>

    public void InsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveUserReceivedItem();
        SaveUserReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� �κ��丮 ������ ���� �Լ� </summary>
    public void UpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveUserReceivedItem();
        SaveUserReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ �κ��丮 �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetInventoryParam()
    {
        Param param = new Param();
        param.Add("GatheringInventoryDataArray", GatheringInventoryDataArray);
        param.Add("CookInventoryDataArray", CookInventoryDataArray);
        param.Add("ToolInventoryDataArray", ToolInventoryDataArray);

        param.Add("GatheringItemReceived", GatheringItemReceived);
        param.Add("NPCItemReceived", NPCItemReceived);
        param.Add("CookItemReceived", CookItemReceived);
        param.Add("ToolItemReceived", ToolItemReceived);
        return param;
    }

    #endregion

    #region SaveAndLoadSticker

    public void LoadStickerData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["StickerReceived"].Count; i < count; i++)
            {
                string item = json[0]["StickerReceived"][i].ToString();
                StickerReceived.Add(item);
            }

            _saveStickerDataList.Clear();
            for (int i = 0; i < json[0]["StickerDataArray"].Count; i++)
            {
                SaveStickerData item = JsonUtility.FromJson<SaveStickerData>(json[0]["StickerDataArray"][i].ToJson());
                _saveStickerDataList.Add(item);
                StickerDataArray.Add(item.GetStickerData());
            }

            LoadUserReceivedSticker();
            LoadUserStickerData();
            Debug.Log("Sticker Load����");
        }
    }


    public void SaveStickerData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Sticker";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;

            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;

            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
                SaveInventoryData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertStickerData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateStickerData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertStickerData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void InsertStickerData(string selectedProbabilityFileId)
    {
        SaveUserStickerData();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void UpdateStickerData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserStickerData();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ��ƼĿ �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetStickerParam()
    {
        Param param = new Param();
        param.Add("StickerReceived", StickerReceived);
        param.Add("StickerDataArray", _saveStickerDataList);
        return param;
    }

    #endregion

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

        for (int i=0; i< GatheringInventoryDataArray.Count; i++) //����� ������
        {
            for (int j = 0; j < GatheringItems.Count; j++) //�����ͺ��̽�
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
                    InventoryItemField field = InventoryItemField.GatheringItem;
                    string id = GatheringInventoryDataArray[i].Id;
                    int amount = GatheringInventoryDataArray[i].Count;
                    GameManager.Instance.Player.GatheringItemInventory[fieldIndex].AddById(field, id, amount, ItemAddEventType.None);
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
                    GameManager.Instance.Player.ToolItemInventory[fieldIndex].AddById(InventoryItemField.Tool, ToolInventoryDataArray[i].Id);
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
            GatheringInventoryDataArray = new List<InventoryData>(); //�ִ� ������ ����� �ű⿡ ����
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
            CookInventoryDataArray = new List<InventoryData>(); //�ִ� ������ ����� �ű⿡ ����
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
            ToolInventoryDataArray = new List<InventoryData>(); //�ִ� ������ ����� �ű⿡ ����
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

    #region NPC, Album, Sticker Received
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

    public void LoadUserReceivedSticker()
    {
        // Sticker
        for (int i = 0; i < StickerReceived.Count; i++)
        {
            GameManager.Instance.Player.StickerInventory.AddById(
               StickerReceived[i], GetStickerImage(StickerReceived[i]));
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

        StickerReceived = new List<string>();
        for (int i = 0; i < GameManager.Instance.Player.StickerInventory.Count; i++)
        {
            StickerReceived.Add(GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id);
        }
    }
    #endregion

    #region Sticker
    public void SaveUserStickerData()
    {
        StickerDataArray = GameManager.Instance.Player.StickerPosList;
    }

    public void LoadUserStickerData()
    {
        GameManager.Instance.Player.StickerPosList = StickerDataArray;
    }

    private Sprite GetStickerImage(string id)
    {
        string code = id.Substring(0, 3);
        Sprite image = null;
        switch (code)
        {
            case "IBG":
                image = FindSpriteById(id, DatabaseManager.Instance.GetBugItemList());
                break;
            case "IFI":
                image = FindSpriteById(id, DatabaseManager.Instance.GetFishItemList());
                break;
            case "IFR":
                image = FindSpriteById(id, DatabaseManager.Instance.GetFruitItemList());
                break;
            case "ITG":
                image = FindSpriteById(id, DatabaseManager.Instance.GetGatheringToolItemList());
                break;
            case "NPC":
                image = FindSpriteById(id, DatabaseManager.Instance.GetNPCList());
                break;
        }

        return image;
    }

    private Sprite FindSpriteById(string id, List<GatheringItem> database)
    {
        for(int i=0;i<database.Count; i++)
        {
            if (database[i].Id.Equals(id))
            {
                return database[i].Image;
            }
        }
        return null;
    }
    private Sprite FindSpriteById(string id, List<ToolItem> database)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].Id.Equals(id))
            {
                return database[i].Image;
            }
        }
        return null;
    }
    private Sprite FindSpriteById(string id, List<NPC> database)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].Id.Equals(id))
            {
                return database[i].Image;
            }
        }
        return null;
    }
    #endregion

    #region Challenges
    public void SaveUserChallengesData()
    {
        ChallengesNum = DatabaseManager.Instance.Challenges.ChallengesNum;
        GatheringSuccessCount = DatabaseManager.Instance.Challenges.GatheringSuccessCount;

        ChallengesCount[0] = DatabaseManager.Instance.Challenges.StackedBambooCount;
        ChallengesCount[1] = DatabaseManager.Instance.Challenges.PurchaseCount;
        ChallengesCount[2] = DatabaseManager.Instance.Challenges.SalesCount;
        ChallengesCount[3] = DatabaseManager.Instance.Challenges.FurnitureCount;
        ChallengesCount[4] = DatabaseManager.Instance.Challenges.CookingCount;
        ChallengesCount[5] = DatabaseManager.Instance.Challenges.TakePhotoCount;
        ChallengesCount[6] = DatabaseManager.Instance.Challenges.SharingPhotoCount;
    }

    public void LoadUserChallengesData()
    {
        // �Ϸ�� �������� �ҷ�����
        for (int i = 0; i < ChallengeDoneId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[ChallengeDoneId[i]].IsDone = true;
        }

        // �Ϸ� �� Ŭ���� �������� �ҷ�����
        for (int i = 0; i < ChallengeClearId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[ChallengeDoneId[i]].IsDone = true;
            DatabaseManager.Instance.GetChallengesDic()[ChallengeClearId[i]].IsClear = true;
        }
    }
    #endregion
}