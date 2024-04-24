using BackEnd;
using LitJson;
using Muks.BackEnd;
using UnityEngine;


/// <summary> 유저의 출석 정보를 보관하는 클래스 </summary>
public class AttendanceUserData
{
    private int _attendanceDayCount; //출석일 카운트
    public int AttendanceDayCount => _attendanceDayCount;

    private string _lastAttendanceDay;//마지막 출석 체크 일자
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

    /// <summary> 동기적으로 서버 출석 정보를 불러옴 </summary>
    public void LoadAttendanceData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        _attendanceDayCount = int.Parse(json[0]["AttendanceDayCount"].ToString());
        _lastAttendanceDay = json[0]["LastAttendanceDay"].ToString();

        Debug.Log("Attendance Load성공");
    }


    /// <summary> 동기적으로 서버 출석 정보를 저장 </summary>
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 비동기적으로 서버 출석 정보를 저장 </summary>
    public void AsyncSaveAttendanceData(int maxRepeatCount)
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

                    Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    /// <summary> 동기적으로 서버 출석 정보 삽입 </summary>
    private void InsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} 삽입을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버 출석 정보 업데이트 </summary>
    private void UpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} 수정을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 비동기적으로 서버 출석 정보 삽입 </summary>
    private void AsyncInsertAttendanceData(string selectedProbabilityFileId)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} 삽입을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 비동기적으로 서버 출석 정보 업데이트 </summary>
    private void AsyncUpdateAttendanceData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetAttendanceParam();
        Debug.LogFormat("{0} 수정을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 유저 데이터를 모아 반환 </summary>
    private Param GetAttendanceParam()
    {
        Param param = new Param();

        param.Add("AttendanceDayCount", _attendanceDayCount);
        param.Add("LastAttendanceDay", _lastAttendanceDay);

        return param;
    }

    #endregion

}
