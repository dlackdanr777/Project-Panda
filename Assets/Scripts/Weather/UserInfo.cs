using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class UserInfo
{

    //���� ������
    //==========================================================================================================

    public string UserId;    //���̵�
    public DateTime TODAY
    {
        get
        {
            if (Backend.IsLogin)
            {
                try
                {
                    BackendReturnObject servertime = Backend.Utils.GetServerTime();
                    string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                    DateTime parsedDate = DateTime.Parse(time);
                    Debug.LogFormat("���� �ð��� {0} �Դϴ�.", parsedDate);
                    return parsedDate;
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                    return DateTime.Now;
                }

            }
            else
            {
                return DateTime.Now;
            }
        }
    }
  //��ǻ���� ���� ��¥�� �ð��� ������(���� ���� �ð����� �����ؾ���)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //������ ������

    public string _lastAccessDay;

    public int DayCount; //���� �����߳�?

    public bool IsExistingUser; //���� �����ΰ�?

    private bool _isExistingStory1Outro; //���丮1 �ƿ�Ʈ�θ� �����ߴ°�?


    //�⼮ üũ
    //==========================================================================================================
    public int AttendanceDayCount; //�⼮�� ī��Ʈ

    public string LastAttendanceDay; //������ �⼮ üũ ����


    //Inventory
    public List<InventoryData> GatheringInventoryDataArray = new List<InventoryData>(); //������ �κ��丮 ������
    private Inventory[] GatheringItemInventory; //���� �� �κ��丮

    public List<InventoryData> CookInventoryDataArray = new List<InventoryData>();
    private Inventory[] CookItemInventory;

    public List<InventoryData> ToolInventoryDataArray = new List<InventoryData>();
    private Inventory[] ToolItemInventory;

    //Item
    public List<string> GatheringItemReceived = new List<string>();
    public List<string> CookItemReceived = new List<string>();
    public List<string> ToolItemReceived = new List<string>();
    public List<string> AlbumReceived = new List<string>();

    //Message
    private MessageList[] MessageLists; //���� �� �޽�������Ʈ


    //Sticker
    public List<string> StickerReceived = new List<string>();
    public List<StickerData> StickerDataList = new List<StickerData>();

    //Challenges
    public List<string> ChallengeDoneId = new List<string>(); // �������� �Ϸ�
    public List<string> ChallengeClearId = new List<string>(); // �������� �Ϸ� �� Ŭ��

    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� �������� �ε���

    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��
    public int[] ChallengesCount = new int[7]; // �������� ���� Ƚ��


    //Storys
    private List<string> _storyCompletedList = new List<string>();
    public List<string> StoryCompletedList => _storyCompletedList;


    //==========================================================================================================
    //���� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    public static string PhotoPath => "Data/";

    //���ʹϾ� ���� ������ �ø� �� �����Ƿ� �߰��� ������ �� Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();

    private Dictionary<string, Item> _allItemDic => DatabaseManager.Instance.ItemDatabase.AllItemDic;

    public void Register()
    {
        CreateUserInfoData();
    }


    private void CreateUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
        AttendanceDayCount = 0;
        LastAttendanceDay = TODAY.AddDays(-1).ToString();
    }


    #region UserInfoData

    public enum StoryOutroType
    {
        Story1,
        Syory2
    }


    public event Action OnSetOutroHandler;


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
            IsExistingUser = (bool)json[0]["IsExistingUser"];

            if (json[0].ContainsKey("IsExistingStory1Outro"))
                _isExistingStory1Outro = (bool)json[0]["IsExistingStory1Outro"];

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

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

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
        param.Add("IsExistingUser", IsExistingUser);
        param.Add("IsExistingStory1Outro", _isExistingStory1Outro);

        return param;
    }


    public void SetStoryOutro(StoryOutroType type)
    {
        switch (type)
        {
            case StoryOutroType.Story1:
                _isExistingStory1Outro = true;
                break;
        }


        OnSetOutroHandler?.Invoke();
    }


    public bool GetStoryOutroBool(StoryOutroType type)
    {
        switch (type)
        {
            case StoryOutroType.Story1:
                return _isExistingStory1Outro;



            default:
                return false;
        }
    }

    #endregion

    #region SaveAndLoadAttendance
    public void LoadAttendanceData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            AttendanceDayCount = int.Parse(json[0]["AttendanceDayCount"].ToString());
            LastAttendanceDay = json[0]["LastAttendanceDay"].ToString();

            Debug.Log("Attendance Load����");
        }
    }


    public void SaveAttendanceData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Attendance";

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
                        InsertAttendanceData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateAttendanceData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertAttendanceData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();

        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();

        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetAttendanceParam()
    {
        Param param = new Param();

        param.Add("AttendanceDayCount", AttendanceDayCount);
        param.Add("LastAttendanceDay", LastAttendanceDay);

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
            LoadGatheringItemReceived();
            LoadFoodItemReceived();
            LoadToolItemReceived();
            Debug.Log("Inventory Load����");
        }
    }


    /// <summary>������ �κ��丮 �����͸� ���������� �Է��ϴ� �Լ�</summary>
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


    /// <summary>������ �κ��丮 �����͸� �񵿱������� �Է��ϴ� �Լ�</summary>
    public void AsyncSaveInventoryData(int maxRepeatCount)
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
                AsyncSaveInventoryData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        AsyncInsertInventoryData(selectedProbabilityFileId);
                    }
                    else
                    {
                        AsyncUpdateInventoryData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    AsyncInsertInventoryData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> ���� �κ��丮 �����͸� ���������� �����ϴ� �Լ� </summary>

    public void InsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� �񵿱������� �����ϴ� �Լ� </summary>

    public void AsyncInsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� ���������� �����ϴ� �Լ� </summary>
    public void UpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� �񵿱������� �����ϴ� �Լ� </summary>
    public void AsyncUpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ �κ��丮 �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetInventoryParam()
    {
        Param param = new Param();
        param.Add("GatheringInventoryDataArray", GatheringInventoryDataArray);
        param.Add("CookInventoryDataArray", CookInventoryDataArray);
        param.Add("ToolInventoryDataArray", ToolInventoryDataArray);

        param.Add("GatheringItemReceived", GatheringItemReceived);
        param.Add("CookItemReceived", CookItemReceived);
        param.Add("ToolItemReceived", ToolItemReceived);

        return param;
    }

    #endregion

    #region SaveAndLoadNPC

    public List<NPCData> NPCReceived = new List<NPCData>();


    public void LoadNPCData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            NPCReceived.Clear();
            for (int i = 0, count = json[0]["NPCReceived"].Count; i < count; i++)
            {
                NPCData item = JsonUtility.FromJson<NPCData>(json[0]["NPCReceived"][i].ToJson());
                NPCReceived.Add(item);
            }
            LoadNPCReceived();
            Debug.Log("NPC Load����");
        }
    }


    public void SaveNPCData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "NPC";

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
                        InsertNPCData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateNPCData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertNPCData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void InsertNPCData(string selectedProbabilityFileId)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� ��ƼĿ ������ ���� �Լ� </summary>
    public void UpdateNPCData(string selectedProbabilityFileId, string inDate)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("��ƼĿ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ��ƼĿ �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetNPCParam()
    {
        Param param = new Param();
        param.Add("NPCReceived", NPCReceived);
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
        for (int i = 0; i < GatheringInventoryDataArray.Count; i++) //����� ������
        {
            InventoryData data = GatheringInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }

        for(int i = 0, count = CookInventoryDataArray.Count; i < count; i++)
        {
            InventoryData data = CookInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }

        for (int i = 0, count = ToolInventoryDataArray.Count; i < count; i++)
        {
            InventoryData data = ToolInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }
    }


    private void SaveUserInventory()
    {
        GatheringItemInventory = GameManager.Instance.Player.GatheringItemInventory;
        CookItemInventory = GameManager.Instance.Player.CookItemInventory;
        ToolItemInventory = GameManager.Instance.Player.ToolItemInventory;

        GatheringInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        for (int i = 0; i < GatheringItemInventory.Length; i++)
        {
            for (int j = 0; j < GatheringItemInventory[i].ItemsCount; j++)
            {
                GatheringInventoryDataArray.Add(new InventoryData(
                    GatheringItemInventory[i].GetItemList()[j].Id,
                    GatheringItemInventory[i].GetItemList()[j].Count));
            }
        }


        int index = (int)CookItemType.CookFood;
        CookInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        List<InventoryItem> cookInventory = CookItemInventory[index].GetItemList();

        for (int i = 0; i < cookInventory.Count; i++)
        {
            CookInventoryDataArray.Add(new InventoryData(
                cookInventory[i].Id,
                cookInventory[i].Count));
        }

        index = (int)ToolItemType.GatheringTool;
        ToolInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        List<InventoryItem> toolInventory = ToolItemInventory[index].GetItemList();

        for (int i = 0; i < toolInventory.Count; i++)
        {
            ToolInventoryDataArray.Add(new InventoryData(toolInventory[i].Id, 1));
        }

    }
    #endregion

    #region Item
    //TODO: ���� �߰�
    public void SaveGatheringItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.GatheringItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.GatheringItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (GatheringItemReceived.Find((x) => x == _items[i].Id) != null)
            {
                continue;
            }

            GatheringItemReceived.Add(_items[i].Id);
        }
    }


    public void LoadGatheringItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemBugList);
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFruitList);
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFishList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (GatheringItemReceived.Find((x) => x == _items[i].Id) == null)
            {
                continue;
            }

            _items[i].IsReceived = true;
        }
    }


    public void SaveFoodItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.CookItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.CookItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (CookItemReceived.Find((x) => x == _items[i].Id) != null)
                continue;

            CookItemReceived.Add(_items[i].Id);
        }
    }


    public void LoadFoodItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFoodList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (CookItemReceived.Find((x) => x == _items[i].Id) == null)
                continue;

            _items[i].IsReceived = true;
        }
    }


    public void SaveToolItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.ToolItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.ToolItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (ToolItemReceived.Find((x) => x == _items[i].Id) != null)
                continue;

            ToolItemReceived.Add(_items[i].Id);
        }
    }


    public void LoadToolItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemToolList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (ToolItemReceived.Find((x) => x == _items[i].Id) == null)
                continue;


            _items[i].IsReceived = true;
        }
    }


    #endregion

    #region NPC Received
    public void LoadNPCReceived()
    {
        List<NPC> npcList = DatabaseManager.Instance.GetNPCList();
        //NPC
        for (int i = 0; i < NPCReceived.Count; i++)
        {
            for (int j = 0, count = npcList.Count; j < count; j++)
            {
                if (NPCReceived[i].Id.Equals(npcList[j].Id))
                {
                    npcList[j].IsReceived = true;
                    npcList[j].Intimacy = NPCReceived[i].Intimacy;
                    npcList[j].SSId = NPCReceived[i].SSId;
                }
            }

        }
    }



    private void SaveNPCReceived()
    {
        List<NPC>[] npcDatabase = { DatabaseManager.Instance.GetNPCList() };

        NPCReceived.Clear();
        for (int i = 0; i < npcDatabase.Length; i++)
        {
            for (int j = 0; j < npcDatabase[i].Count; j++)
            {
                if (npcDatabase[i][j].IsReceived)
                {
                    NPCReceived.Add(new NPCData(npcDatabase[i][j].Id, npcDatabase[i][j].Intimacy, npcDatabase[i][j].SSId));
                }
            }
        }
    }

    #endregion

    #region Book

    public void LoadBookData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["AlbumReceived"].Count; i < count; i++)
            {
                string item = json[0]["AlbumReceived"][i].ToString();
                AlbumReceived.Add(item);
            }

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
                StickerDataList.Add(item.GetStickerData());
            }

            LoadAlbumReceived();
            LoadStickerReceived();
            Debug.Log("Book Load����");
        }
    }


    public void SaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

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
                SaveBookData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertBookData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateBookData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertBookData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertBookData(string selectedProbabilityFileId)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataList)
        {
            Debug.Log(data.Id);
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateBookData(string selectedProbabilityFileId, string inDate)
    {
        SaveAlbumReceived();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataList)
        {
            Debug.Log(data.Id);
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetBookParam();

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetBookParam()
    {
        Param param = new Param();

        param.Add("AlbumReceived", AlbumReceived);
        param.Add("StickerReceived", StickerReceived);
        param.Add("StickerDataArray", _saveStickerDataList);

        return param;
    }


    public void LoadAlbumReceived()
    {
        List<Album> albumList = DatabaseManager.Instance.AlbumDatabase.GetAlbumList();

        for (int i = 0; i < AlbumReceived.Count; i++)
        {
            Album album = albumList.Find(x => x.Id == AlbumReceived[i]);
            if (album == null)
                continue;

            album.IsReceived = true;
        }
    }


    private void SaveAlbumReceived()
    {
        List<Album> albumList = DatabaseManager.Instance.AlbumDatabase.GetAlbumList();

        for (int i = 0; i < albumList.Count; i++)
        {
            if (AlbumReceived.Find((x) => x == albumList[i].Id) != null)
                continue;

            AlbumReceived.Add(albumList[i].Id);
        }
    }


    private void SaveStickerReceived()
    {
        StickerReceived.Clear();
        for (int i = 0; i < GameManager.Instance.Player.StickerInventory.Count; i++)
        {
            StickerReceived.Add(GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id);
        }
    }


    private void LoadStickerReceived()
    {
        // Sticker
        for (int i = 0; i < StickerReceived.Count; i++)
        {
            GameManager.Instance.Player.StickerInventory.AddById(StickerReceived[i], GetStickerImage(StickerReceived[i]));
        }
    }

    private Sprite GetStickerImage(string id)
    {
        string code = id.Substring(0, 3);
        Sprite image = FindItemSpriteById(id);
        switch (code)
        {
            case "NPC":
                image = FindNPCSpriteById(id);
                break;

            default:
                image = FindItemSpriteById(id);
                break;
        }

        return image;
    }

    public List<StickerData> GetStickerList()
    {
        return StickerDataList;
    }

    #endregion

    #region Find

    private Sprite FindItemSpriteById(string id)
    {
        return DatabaseManager.Instance.GetStickerImage(id);
    }


    private Sprite FindNPCSpriteById(string id)
    {
        Dictionary<string, NPC> npcDic = DatabaseManager.Instance.NPCDatabase.NpcDic;
        if (npcDic.TryGetValue(id, out NPC npc))
        {
            return npc.Image;
        }
        Debug.LogErrorFormat("{0}Id�� �������� �ʽ��ϴ�.");
        return null;
    }

    #endregion

    #region Challenges

    public void LoadChallengesData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {

            // �������� ����
            for (int i = 0, count = json[0]["ChallengesNum"].Count; i < count; i++)
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

            LoadChallengesReceived();
            Debug.Log("Challenges Load����");
        }
    }


    public void SaveChallengesData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Challenges";

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
                        InsertChallengesData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateChallengesData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertChallengesData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void SaveChallengesDataByAsync(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Challenges";

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

        
        //BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());
        Backend.GameData.GetMyData(selectedProbabilityFileId, new Where(), 10, bro =>
        {
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
                            InsertChallengesData(selectedProbabilityFileId);
                        }
                        else
                        {
                            UpdateChallengesData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        InsertChallengesData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        });
       
    }


    public void InsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ �������� ������ ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetChallengesParam()
    {
        Param param = new Param();

        param.Add("ChallengesNum", ChallengesNum);
        param.Add("GatheringSuccessCount", GatheringSuccessCount);
        param.Add("ChallengesCount", ChallengesCount);
        param.Add("ChallengeDoneId", ChallengeDoneId);
        param.Add("ChallengeClearId", ChallengeClearId);

        return param;
    }


    private void SaveChallengesReceived()
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

        ChallengeDoneId.Clear();
        ChallengeClearId.Clear();
        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        foreach (string key in challengesDic.Keys)
        {
            if (challengesDic[key].IsDone)
                ChallengeDoneId.Add(key);

            if (challengesDic[key].IsClear)
                ChallengeClearId.Add(key);
        }
    }

    private void LoadChallengesReceived()
    {
        DatabaseManager.Instance.Challenges.ChallengesNum = ChallengesNum;
        DatabaseManager.Instance.Challenges.GatheringSuccessCount = GatheringSuccessCount;

        DatabaseManager.Instance.Challenges.StackedBambooCount = ChallengesCount[0];
        DatabaseManager.Instance.Challenges.PurchaseCount = ChallengesCount[1];
        DatabaseManager.Instance.Challenges.SalesCount = ChallengesCount[2];
        DatabaseManager.Instance.Challenges.FurnitureCount = ChallengesCount[3];
        DatabaseManager.Instance.Challenges.CookingCount = ChallengesCount[4];
        DatabaseManager.Instance.Challenges.TakePhotoCount = ChallengesCount[5];
        DatabaseManager.Instance.Challenges.SharingPhotoCount = ChallengesCount[6];

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

    #region Story
    public void LoadStoryData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["StoryCompletedList"].Count; i < count; i++)
            {
                string item = json[0]["StoryCompletedList"][i].ToString();
                _storyCompletedList.Add(item);
            }
            DatabaseManager.Instance.MainDialogueDatabase.SetCompletedStoryList(_storyCompletedList);
            Debug.Log("StoryData Load����");
        }
    }


    public void SaveStoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Story";

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
                        InsertStoryData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateStoryData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertStoryData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertStoryData(string selectedProbabilityFileId)
    {

        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetStoryParam()
    {
        Param param = new Param();
        _storyCompletedList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;
        param.Add("StoryCompletedList", _storyCompletedList);

        return param;
    }

    #endregion
}