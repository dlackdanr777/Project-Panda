using BackEnd;
using LitJson;
using Muks.BackEnd;
using UnityEngine;


/// <summary> ������ �⼮ ������ �����ϴ� Ŭ���� </summary>
public class AttendanceUserData
{
    private int _attendanceDayCount; //�⼮�� ī��Ʈ
    public int AttendanceDayCount => _attendanceDayCount;

    private string _lastAttendanceDay;//������ �⼮ üũ ����
    public string LastAttendanceDay => _lastAttendanceDay;


    public AttendanceUserData(string lastAttendanceDay)
    {
        _attendanceDayCount = 0;
        _lastAttendanceDay = lastAttendanceDay;
    }


    public void AttendanceCheck()
    {
        _attendanceDayCount++;
        _lastAttendanceDay = DatabaseManager.Instance.UserInfo.TODAY.ToString();
    }


    #region SaveAndLoadAttendance

    /// <summary> ���������� ���� �⼮ ������ �ҷ��� </summary>
    public void LoadAttendanceData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        _attendanceDayCount = int.Parse(json[0]["AttendanceDayCount"].ToString());
        _lastAttendanceDay = json[0]["LastAttendanceDay"].ToString();

        Debug.Log("Attendance Load����");
    }


    /// <summary> ���������� ���� �⼮ ������ ���� </summary>
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveAttendanceData(maxRepeatCount - 1);
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


    /// <summary> �񵿱������� ���� �⼮ ������ ���� </summary>
    public void AsyncSaveAttendanceData(int maxRepeatCount)
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

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), (bro) =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveAttendanceData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertAttendanceData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateAttendanceData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertAttendanceData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    /// <summary> ���������� ���� �⼮ ���� ���� </summary>
    private void InsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���������� ���� �⼮ ���� ������Ʈ </summary>
    private void UpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> �񵿱������� ���� �⼮ ���� ���� </summary>
    private void AsyncInsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> �񵿱������� ���� �⼮ ���� ������Ʈ </summary>
    private void AsyncUpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ </summary>
    private Param GetAttendanceParam()
    {
        Param param = new Param();

        param.Add("AttendanceDayCount", _attendanceDayCount);
        param.Add("LastAttendanceDay", _lastAttendanceDay);

        return param;
    }

    #endregion

}
