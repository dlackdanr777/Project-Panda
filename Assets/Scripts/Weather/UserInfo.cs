using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class UserInfo
{

    //유저 데이터
    //==========================================================================================================

    public string UserId;    //아이디
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
                    Debug.LogFormat("서버 시간은 {0} 입니다.", parsedDate);
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
  //컴퓨터의 현재 날짜와 시간을 가져옴(추후 서버 시간으로 변경해야함)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //마지막 접속일

    public string _lastAccessDay;

    public int DayCount; //몇일 접속했나?

    public bool IsExistingUser; //기존 유저인가?

    private bool _isExistingStory1Outro; //스토리1 아웃트로를 감상했는가?


    //출석 체크
    //==========================================================================================================
    public int AttendanceDayCount; //출석일 카운트

    public string LastAttendanceDay; //마지막 출석 체크 일자


    //Inventory
    public List<InventoryData> GatheringInventoryDataArray = new List<InventoryData>(); //저장할 인벤토리 데이터
    private Inventory[] GatheringItemInventory; //게임 속 인벤토리

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
    private MessageList[] MessageLists; //게임 속 메시지리스트


    //Sticker
    public List<string> StickerReceived = new List<string>();
    public List<StickerData> StickerDataList = new List<StickerData>();

    //Challenges
    public List<string> ChallengeDoneId = new List<string>(); // 도전과제 완료
    public List<string> ChallengeClearId = new List<string>(); // 도전과제 완료 후 클릭

    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제 인덱스

    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수
    public int[] ChallengesCount = new int[7]; // 도전과제 성공 횟수


    //Storys
    private List<string> _storyCompletedList = new List<string>();
    public List<string> StoryCompletedList => _storyCompletedList;


    //==========================================================================================================
    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    public static string PhotoPath => "Data/";

    //쿼터니언 값을 서버에 올릴 수 없으므로 중간에 관리해 줄 Class List
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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

            Debug.Log("UserInfo Load성공");
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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertUserInfoData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("게임 정보 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("게임 정보 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 유저 데이터를 모아 반환하는 클래스 </summary>
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            AttendanceDayCount = int.Parse(json[0]["AttendanceDayCount"].ToString());
            LastAttendanceDay = json[0]["LastAttendanceDay"].ToString();

            Debug.Log("Attendance Load성공");
        }
    }


    public void SaveAttendanceData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Attendance";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();

        Debug.LogFormat("{0} 삽입을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();

        Debug.LogFormat("{0} 수정을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 유저 데이터를 모아 반환하는 클래스 </summary>
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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
            Debug.Log("Inventory Load성공");
        }
    }


    /// <summary>서버에 인벤토리 데이터를 동기적으로 입력하는 함수</summary>
    public void SaveInventoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Inventory";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary>서버에 인벤토리 데이터를 비동기적으로 입력하는 함수</summary>
    public void AsyncSaveInventoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Inventory";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 서버 인벤토리 데이터를 동기적으로 삽입하는 함수 </summary>

    public void InsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 서버 인벤토리 데이터를 비동기적으로 삽입하는 함수 </summary>

    public void AsyncInsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 서버 인벤토리 데이터를 동기적으로 수정하는 함수 </summary>
    public void UpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버 인벤토리 데이터를 비동기적으로 수정하는 함수 </summary>
    public void AsyncUpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 인벤토리 데이터를 모아 반환하는 클래스 </summary>
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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
            Debug.Log("NPC Load성공");
        }
    }


    public void SaveNPCData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "NPC";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 서버 스티커 데이터 삽입 함수 </summary>
    public void InsertNPCData(string selectedProbabilityFileId)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("스티커 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 서버 스티커 데이터 수정 함수 </summary>
    public void UpdateNPCData(string selectedProbabilityFileId, string inDate)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("스티커 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 스티커 데이터를 모아 반환하는 클래스 </summary>
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
        for (int i = 0; i < GatheringInventoryDataArray.Count; i++) //저장된 데이터
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

        GatheringInventoryDataArray.Clear(); //있던 데이터 지우고 거기에 저장
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
        CookInventoryDataArray.Clear(); //있던 데이터 지우고 거기에 저장
        List<InventoryItem> cookInventory = CookItemInventory[index].GetItemList();

        for (int i = 0; i < cookInventory.Count; i++)
        {
            CookInventoryDataArray.Add(new InventoryData(
                cookInventory[i].Id,
                cookInventory[i].Count));
        }

        index = (int)ToolItemType.GatheringTool;
        ToolInventoryDataArray.Clear(); //있던 데이터 지우고 거기에 저장
        List<InventoryItem> toolInventory = ToolItemInventory[index].GetItemList();

        for (int i = 0; i < toolInventory.Count; i++)
        {
            ToolInventoryDataArray.Add(new InventoryData(toolInventory[i].Id, 1));
        }

    }
    #endregion

    #region Item
    //TODO: 차후 추가
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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
            Debug.Log("Book Load성공");
        }
    }


    public void SaveBookData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Book";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
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

        Debug.LogFormat("{0} 데이터 삽입을 요청합니다.", selectedProbabilityFileId);

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

        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 도감 데이터를 모아 반환하는 클래스 </summary>
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
        Debug.LogErrorFormat("{0}Id가 존재하지 않습니다.");
        return null;
    }

    #endregion

    #region Challenges

    public void LoadChallengesData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {

            // 도전과제 관련
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
            Debug.Log("Challenges Load성공");
        }
    }


    public void SaveChallengesData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Challenges";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void SaveChallengesDataByAsync(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Challenges";

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

        
        //BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());
        Backend.GameData.GetMyData(selectedProbabilityFileId, new Where(), 10, bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    Debug.LogError("초기화 실패");
                    break;

                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;

                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
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

                    Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        });
       
    }


    public void InsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 도전과제 정보를 모아 반환하는 클래스 </summary>
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

        // 완료된 도전과제 불러오기
        for (int i = 0; i < ChallengeDoneId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[ChallengeDoneId[i]].IsDone = true;
        }

        // 완료 후 클릭한 도전과제 불러오기
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
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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
            Debug.Log("StoryData Load성공");
        }
    }


    public void SaveStoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Story";

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

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertStoryData(string selectedProbabilityFileId)
    {

        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 유저 데이터를 모아 반환하는 클래스 </summary>
    public Param GetStoryParam()
    {
        Param param = new Param();
        _storyCompletedList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;
        param.Add("StoryCompletedList", _storyCompletedList);

        return param;
    }

    #endregion
}