using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ������ ���丮 ������ �����ϴ� Ŭ���� </summary>
public class StoryUserData
{
    //Storys
    private List<string> _storyCompletedList = new List<string>();


    #region Save&Load Story

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


    /// <summary> ���������� ������ ���� ���丮 ���� ���� </summary>
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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveStoryData(maxRepeatCount - 1);
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


    /// <summary> �񵿱������� ������ ���� ���丮 ���� ���� </summary>
    public void AsyncSaveStoryData(int maxRepeatCount)
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

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveStoryData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertStoryData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateStoryData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertStoryData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        }); 
    }


    /// <summary> ���������� ������ ���� ���丮 ���� �߰� </summary>
    private void InsertStoryData(string selectedProbabilityFileId)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���������� ������ ���� ���丮 ���� ���� </summary>
    private void UpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> �񵿱������� ������ ���� ���丮 ���� �߰� </summary>
    private void AsyncInsertStoryData(string selectedProbabilityFileId)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> �񵿱������� ������ ���� ���丮 ���� ���� </summary>
    private void AsyncUpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("���丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    private Param GetStoryParam()
    {
        Param param = new Param();
        _storyCompletedList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;
        param.Add("StoryCompletedList", _storyCompletedList);

        return param;
    }

    #endregion
}
