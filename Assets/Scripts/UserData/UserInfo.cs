using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using UnityEngine;


public class UserInfo
{

    //���� ������
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
                Debug.LogFormat("���� �ð��� {0} �Դϴ�.", parsedDate);
                return parsedDate;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return DateTime.Now;
            }
        }
    }

    public string UserId;    //���̵�
    public string _lastAccessDay;
    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //������ ������
    public int DayCount; //���� �����߳�?
    public bool IsExistingUser; //���� �����ΰ�?
    public bool IsCookTutorialClear; //�丮�� Ʃ�丮���� �Ϸ��ߴ°�?
    private bool _isExistingStory1Outro; //���丮1 �ƿ�Ʈ�θ� �����ߴ°�?

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

    #region UserInfoData

    public enum StoryOutroType
    {
        Story1,
        Syory2
    }



    public event Action OnSetOutroHandler;

    /// <summary>���������� ���� ���� ���� �ҷ���</summary>
    public void LoadUserInfoData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        UserId = json[0]["UserId"].ToString();
        DayCount = int.Parse(json[0]["DayCount"].ToString());
        _lastAccessDay = json[0]["LastAccessDay"].ToString();
        IsExistingUser = (bool)json[0]["IsExistingUser"];

        if (json[0].ContainsKey("IsCookTutorialClear"))
            IsCookTutorialClear = (bool)json[0]["IsCookTutorialClear"];

        if (json[0].ContainsKey("IsExistingStory1Outro"))
            _isExistingStory1Outro = (bool)json[0]["IsExistingStory1Outro"];

        Debug.Log("UserInfo Load����");
    }


    /// <summary>���������� ���� ���� ���� ����</summary>
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

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary>�񵿱������� ���� ���� ���� ����</summary>
    public void AsyncSaveUserInfoData(int maxRepeatCount)
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

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveUserInfoData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertUserInfoData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateUserInfoData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertUserInfoData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        });   
    }


    /// <summary> ���������� ���� ���� ���� ���� </summary>
    private void InsertUserInfoData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���������� ���� ���� ���� ���� </summary>
    private void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� </summary>
    private void AsyncInsertUserInfoData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� </summary>
    private void AsyncUpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        Param param = GetUserInfoParam();

        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    private Param GetUserInfoParam()
    {
        Param param = new Param();

        param.Add("UserId", UserId);
        param.Add("DayCount", DayCount);
        param.Add("LastAccessDay", _lastAccessDay);
        param.Add("IsExistingUser", IsExistingUser);
        param.Add("IsCookTutorialClear", IsCookTutorialClear);
        param.Add("IsExistingStory1Outro", _isExistingStory1Outro);

        return param;
    }

 #endregion
}