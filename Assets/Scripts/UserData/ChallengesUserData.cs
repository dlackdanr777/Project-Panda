using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 유저의 도전과제 정보를 보관하는 클래스 </summary>
public class ChallengesUserData
{
    //Challenges
    private List<string> _challengeDoneId = new List<string>(); // 도전과제 완료
    private List<string> _challengeClearId = new List<string>(); // 도전과제 완료 후 클릭

    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제 인덱스
    private int[] _gatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수
    private int[] _challengesCount = new int[7]; // 도전과제 성공 횟수

    #region LoadAndSaveChallenges

    /// <summary> 동기적으로 도전과제 정보를 불러옴 </summary>
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
                _challengesNum[i] = int.Parse(json[0]["ChallengesNum"][i].ToString());
            }

            for (int i = 0, count = json[0]["GatheringSuccessCount"].Count; i < count; i++)
            {
                _gatheringSuccessCount[i] = int.Parse(json[0]["GatheringSuccessCount"][i].ToString());
            }

            for (int i = 0, count = json[0]["ChallengesCount"].Count; i < count; i++)
            {
                _challengesCount[i] = int.Parse(json[0]["ChallengesCount"][i].ToString());
            }

            for (int i = 0, count = json[0]["ChallengeDoneId"].Count; i < count; i++)
            {
                _challengeDoneId.Add(json[0]["ChallengeDoneId"][i].ToString());
            }

            for (int i = 0, count = json[0]["ChallengeClearId"].Count; i < count; i++)
            {
                _challengeClearId.Add(json[0]["ChallengeClearId"][i].ToString());
            }

            LoadChallengesReceived();
            Debug.Log("Challenges Load성공");
        }
    }


    /// <summary> 동기적으로 도전과제 정보 저장 </summary>
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveChallengesData(maxRepeatCount - 1);
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


    public void AsyncSaveChallengesData(int maxRepeatCount)
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

        Backend.GameData.GetMyData(selectedProbabilityFileId, new Where(), 10, bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveChallengesData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertChallengesData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateChallengesData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertChallengesData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    private void InsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    private void UpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    private void AsyncInsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 삽입을 요청합니다.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    private void AsyncUpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("도전과제 데이터 수정을 요청합니다.");

        BackendManager.Instance. AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 도전과제 정보를 모아 반환하는 클래스 </summary>
    private Param GetChallengesParam()
    {
        Param param = new Param();

        param.Add("ChallengesNum", _challengesNum);
        param.Add("GatheringSuccessCount", _gatheringSuccessCount);
        param.Add("ChallengesCount", _challengesCount);
        param.Add("ChallengeDoneId", _challengeDoneId);
        param.Add("ChallengeClearId", _challengeClearId);

        return param;
    }


    private void SaveChallengesReceived()
    {
        _challengesNum = DatabaseManager.Instance.Challenges.ChallengesNum;
        _gatheringSuccessCount = DatabaseManager.Instance.Challenges.GatheringSuccessCount;

        _challengesCount[0] = DatabaseManager.Instance.Challenges.StackedBambooCount;
        _challengesCount[1] = DatabaseManager.Instance.Challenges.PurchaseCount;
        _challengesCount[2] = DatabaseManager.Instance.Challenges.SalesCount;
        _challengesCount[3] = DatabaseManager.Instance.Challenges.FurnitureCount;
        _challengesCount[4] = DatabaseManager.Instance.Challenges.CookingCount;
        _challengesCount[5] = DatabaseManager.Instance.Challenges.TakePhotoCount;
        _challengesCount[6] = DatabaseManager.Instance.Challenges.SharingPhotoCount;

        _challengeDoneId.Clear();
        _challengeClearId.Clear();
        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        foreach (string key in challengesDic.Keys)
        {
            if (challengesDic[key].IsDone)
                _challengeDoneId.Add(key);

            if (challengesDic[key].IsClear)
                _challengeClearId.Add(key);
        }
    }

    private void LoadChallengesReceived()
    {
        DatabaseManager.Instance.Challenges.ChallengesNum = _challengesNum;
        DatabaseManager.Instance.Challenges.GatheringSuccessCount = _gatheringSuccessCount;

        DatabaseManager.Instance.Challenges.StackedBambooCount = _challengesCount[0];
        DatabaseManager.Instance.Challenges.PurchaseCount = _challengesCount[1];
        DatabaseManager.Instance.Challenges.SalesCount = _challengesCount[2];
        DatabaseManager.Instance.Challenges.FurnitureCount = _challengesCount[3];
        DatabaseManager.Instance.Challenges.CookingCount = _challengesCount[4];
        DatabaseManager.Instance.Challenges.TakePhotoCount = _challengesCount[5];
        DatabaseManager.Instance.Challenges.SharingPhotoCount = _challengesCount[6];

        // 완료된 도전과제 불러오기
        for (int i = 0; i < _challengeDoneId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[_challengeDoneId[i]].IsDone = true;
        }

        // 완료 후 클릭한 도전과제 불러오기
        for (int i = 0; i < _challengeClearId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[_challengeDoneId[i]].IsDone = true;
            DatabaseManager.Instance.GetChallengesDic()[_challengeClearId[i]].IsClear = true;
        }
    }

    #endregion
}
