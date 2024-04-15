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

    private AttendanceUserData _attendanceUserData;
    public AttendanceUserData AttendanceUserData => _attendanceUserData;

    private InventoryUserData _inventoryUserData;
    public InventoryUserData InventoryUserData => _inventoryUserData;

    private ChallengesUserData _challengesUserData;
    public ChallengesUserData ChallengesUserData => _challengesUserData;

    private StoryUserData _storyUserData;
    public StoryUserData StoryUserData => _storyUserData;


    public List<string> AlbumReceived = new List<string>();

    //Message
    private MessageList[] MessageLists; //게임 속 메시지리스트


    //Sticker
    public List<string> StickerReceived = new List<string>();
    public List<StickerData> StickerDataList = new List<StickerData>();


    //==========================================================================================================
    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    public static string PhotoPath => "Data/";

    //쿼터니언 값을 서버에 올릴 수 없으므로 중간에 관리해 줄 Class List
    private List<SaveStickerData> _saveStickerDataList = new List<SaveStickerData>();

    public void Register()
    {
        CreateUserInfoData();
        _attendanceUserData = new AttendanceUserData(TODAY.AddDays(-1).ToString());
        _inventoryUserData = new InventoryUserData();
        _challengesUserData = new ChallengesUserData();
        _storyUserData = new StoryUserData();
    }


    private void CreateUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
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

        UserId = json[0]["UserId"].ToString();
        DayCount = int.Parse(json[0]["DayCount"].ToString());
        _lastAccessDay = json[0]["LastAccessDay"].ToString();
        IsExistingUser = (bool)json[0]["IsExistingUser"];

        if (json[0].ContainsKey("IsExistingStory1Outro"))
            _isExistingStory1Outro = (bool)json[0]["IsExistingStory1Outro"];

        Debug.Log("UserInfo Load성공");
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveNPCData(maxRepeatCount - 1);
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
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
}