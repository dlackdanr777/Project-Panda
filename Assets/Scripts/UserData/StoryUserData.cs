using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 유저의 스토리 정보를 보관하는 클래스 </summary>
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


    /// <summary> 동기적으로 서버에 유저 스토리 정보 저장 </summary>
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 비동기적으로 서버에 유저 스토리 정보 저장 </summary>
    public void AsyncSaveStoryData(int maxRepeatCount)
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

                    Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        }); 
    }


    /// <summary> 동기적으로 서버에 유저 스토리 정보 추가 </summary>
    private void InsertStoryData(string selectedProbabilityFileId)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버에 유저 스토리 정보 삽입 </summary>
    private void UpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 비동기적으로 서버에 유저 스토리 정보 추가 </summary>
    private void AsyncInsertStoryData(string selectedProbabilityFileId)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 삽입을 요청합니다.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 비동기적으로 서버에 유저 스토리 정보 삽입 </summary>
    private void AsyncUpdateStoryData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetStoryParam();

        Debug.LogFormat("스토리 데이터 수정을 요청합니다.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 유저 데이터를 모아 반환하는 클래스 </summary>
    private Param GetStoryParam()
    {
        Param param = new Param();
        _storyCompletedList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;
        param.Add("StoryCompletedList", _storyCompletedList);

        return param;
    }

    #endregion
}
