using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using UnityEngine;


public class UserInfo
{

    //유저 데이터
    //==========================================================================================================

    public DateTime TODAY
    {
        get
        {
            if (!Backend.IsLogin)
                return DateTime.Now;

            try
            {
                BackendReturnObject servertime = Backend.Utils.GetServerTime();
                string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                DateTime parsedDate = DateTime.Parse(time);
                Debug.LogFormat("서버 시간은 {0} 입니다.", parsedDate);
                return parsedDate;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return DateTime.Now;
            }
        }
    }

    public string UserId;    //아이디
    public string _lastAccessDay;
    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //마지막 접속일
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

    private BookUserData _bookUserData;
    public BookUserData BookUserData => _bookUserData;

    private NpcUserData _npcUserData;
    public NpcUserData NpcUserData => _npcUserData;

    private MailUserData _mailUserData;
    public MailUserData MailUserData => _mailUserData;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        CreateUserInfoData();
        _attendanceUserData = new AttendanceUserData(TODAY.AddDays(-1).ToString());
        _inventoryUserData = new InventoryUserData();
        _challengesUserData = new ChallengesUserData();
        _storyUserData = new StoryUserData();
        _bookUserData = new BookUserData();
        _npcUserData= new NpcUserData();
        _mailUserData = new MailUserData();
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
}