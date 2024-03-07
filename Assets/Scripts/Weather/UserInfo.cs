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
    public List<NPCData> NPCReceived = new List<NPCData>();
    public List<string> CookItemReceived = new List<string>();
    public List<string> ToolItemReceived = new List<string>();
    public List<string> AlbumReceived = new List<string>();

    //Message
    public List<MessageData> MessageDataArray = new List<MessageData>(); //저장할 메시지 데이터
    private MessageList[] MessageLists; //게임 속 메시지리스트


    //Sticker
    public List<string> StickerReceived = new List<string>();
    public List<StickerData> StickerDataArray = new List<StickerData>();

    //Challenges
    public List<string> ChallengeDoneId = new List<string>(); // 도전과제 완료
    public List<string> ChallengeClearId = new List<string>(); // 도전과제 완료 후 클릭

    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제 인덱스

    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수
    public int[] ChallengesCount = new int[7]; // 도전과제 성공 횟수


    //Storys
    private List<string> _storyCompletedList = new List<string>();

    //==========================================================================================================
    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

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


    #region SaveAndLoadUserInfo
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

            // 도전과제 관련
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


            for(int i = 0, count = json[0]["StoryCompletedList"].Count; i < count; i++)
            {
                string item = json[0]["StoryCompletedList"][i].ToString();
                _storyCompletedList.Add(item);
            }
            StoryManager.Instance.SetStoryCompletedList(_storyCompletedList);

            LoadUserChallengesData();
            LoadAlbumReceived();
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

        SaveUserChallengesData();
        SaveUserMailData();

        Param param = GetUserInfoParam();

        Debug.LogFormat("게임 정보 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        SaveUserChallengesData();
        SaveUserMailData();

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

        param.Add("ChallengesNum", ChallengesNum);
        param.Add("GatheringSuccessCount", GatheringSuccessCount);
        param.Add("ChallengesCount", ChallengesCount);
        param.Add("ChallengeDoneId", ChallengeDoneId);
        param.Add("ChallengeClearId", ChallengeClearId);

        param.Add("AlbumReceived", AlbumReceived);
        param.Add("MessageDataArray", MessageDataArray);

        _storyCompletedList = StoryManager.Instance.GetStoryCompletedList();
        param.Add("StoryCompletedList", _storyCompletedList);

        return param;
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
            Debug.Log("Inventory Load성공");
        }
    }


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


    /// <summary> 서버 인벤토리 데이터 삽입 함수 </summary>

    public void InsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 서버 인벤토리 데이터 수정 함수 </summary>
    public void UpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        Param param = GetInventoryParam();

        Debug.LogFormat("인벤토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
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

    #region SaveAndLoadSticker

    public void LoadStickerData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
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

            LoadStickerReceived();
            LoadUserStickerData();
            Debug.Log("Sticker Load성공");
        }
    }


    public void SaveStickerData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Sticker";

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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 서버 스티커 데이터 삽입 함수 </summary>
    public void InsertStickerData(string selectedProbabilityFileId)
    {
        SaveUserStickerData();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("스티커 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 서버 스티커 데이터 수정 함수 </summary>
    public void UpdateStickerData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserStickerData();
        SaveStickerReceived();

        _saveStickerDataList.Clear();
        foreach (StickerData data in StickerDataArray)
        {
            _saveStickerDataList.Add(data.GetSaveStickerData());
        }

        Param param = GetStickerParam();

        Debug.LogFormat("스티커 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 스티커 데이터를 모아 반환하는 클래스 </summary>
    public Param GetStickerParam()
    {
        Param param = new Param();
        param.Add("StickerReceived", StickerReceived);
        param.Add("StickerDataArray", _saveStickerDataList);
        return param;
    }

    #endregion

    #region SaveAndLoadNPC

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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
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
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None);
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
    //TODO: 차후 추가

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
    public void LoadNPCReceived()
    {
        //NPC
        for (int i = 0; i < NPCReceived.Count; i++)
        {
            for (int j = 0; j < DatabaseManager.Instance.GetNPCList().Count; j++)
            {
                if (NPCReceived[i].Id.Equals(DatabaseManager.Instance.GetNPCList()[j].Id))
                {
                    DatabaseManager.Instance.GetNPCList()[j].IsReceived = true;
                    DatabaseManager.Instance.GetNPCList()[j].Intimacy = NPCReceived[i].Intimacy;
                    DatabaseManager.Instance.GetNPCList()[j].SSId = NPCReceived[i].SSId;
                }
            }

        }
    }

    public void LoadAlbumReceived()
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


    private void SaveAlbumReceived()
    {
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


    private void SaveStickerReceived()
    {
        StickerReceived = new List<string>();
        for (int i = 0; i < GameManager.Instance.Player.StickerInventory.Count; i++)
        {
            StickerReceived.Add(GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id);
        }
    }


    public void LoadStickerReceived()
    {
        // Sticker
        for (int i = 0; i < StickerReceived.Count; i++)
        {
            GameManager.Instance.Player.StickerInventory.AddById(
               StickerReceived[i], GetStickerImage(StickerReceived[i]));
        }
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
}