using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ������ �������� ������ �����ϴ� Ŭ���� </summary>
public class ChallengesUserData
{
    //Challenges
    private List<string> _challengeDoneId = new List<string>(); // �������� �Ϸ�
    private List<string> _challengeClearId = new List<string>(); // �������� �Ϸ� �� Ŭ��

    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� �������� �ε���
    private int[] _gatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��
    private int[] _challengesCount = new int[7]; // �������� ���� Ƚ��

    #region LoadAndSaveChallenges

    /// <summary> ���������� �������� ������ �ҷ��� </summary>
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
            Debug.Log("Challenges Load����");
        }
    }


    /// <summary> ���������� �������� ���� ���� </summary>
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

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void AsyncSaveChallengesData(int maxRepeatCount)
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

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    private void InsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    private void UpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    private void AsyncInsertChallengesData(string selectedProbabilityFileId)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    private void AsyncUpdateChallengesData(string selectedProbabilityFileId, string inDate)
    {
        SaveChallengesReceived();
        Param param = GetChallengesParam();

        Debug.LogFormat("�������� ������ ������ ��û�մϴ�.");

        BackendManager.Instance. AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ �������� ������ ��� ��ȯ�ϴ� Ŭ���� </summary>
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

        // �Ϸ�� �������� �ҷ�����
        for (int i = 0; i < _challengeDoneId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[_challengeDoneId[i]].IsDone = true;
        }

        // �Ϸ� �� Ŭ���� �������� �ҷ�����
        for (int i = 0; i < _challengeClearId.Count; i++)
        {
            DatabaseManager.Instance.GetChallengesDic()[_challengeDoneId[i]].IsDone = true;
            DatabaseManager.Instance.GetChallengesDic()[_challengeClearId[i]].IsClear = true;
        }
    }

    #endregion
}
