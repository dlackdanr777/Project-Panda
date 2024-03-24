using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StoryOutroType
{
    Story1,
    Syory2
}



/// <summary>유저 데이터에서 bool값을 확인해 활성, 비활성화 하는 스크립트 </summary>
public class ConfirmEnabledAfterCompletedOutro : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;

    [Header("Options")]
    [Tooltip("해당 스토리ID가 완료 상태일 경우 활성(공백일 경우 무조건 활성)")]
    [SerializeField] private string _storyId;
    [SerializeField] private StoryOutroType _outroType;
    [SerializeField] private bool _outroBoolCheck;


    private void Start()
    {
        Check();

        MainStoryController.OnFinishStoryHandler += Check;
        FadeInOutManager.Instance.OnFadeOutHandler += Check;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    private void Check()
    {
        if (CheckCompletedOutro())
        {
            CheckCompletedStory();
        }
    }


    /// <summary>인스펙터창에 적힌 스토리id가 완료된 상태면 아이콘을 활성화, 아닐경우 비활성화 하는 함수</summary>
    private bool CheckCompletedOutro()
    {
        switch (_outroType)
        {
            case StoryOutroType.Story1:

                if (DatabaseManager.Instance.UserInfo.IsExistingStory1Outro != _outroBoolCheck)
                {
                    gameObject.SetActive(false);
                    return false;
                }
                    
                else
                {
                    gameObject.SetActive(true);
                    return true;
                }
        }

        return true;
    }


    /// <summary>인스펙터창에 적힌 스토리id가 완료된 상태면 아이콘을 활성화, 아닐경우 비활성화 하는 함수</summary>
    private void CheckCompletedStory()
    {
        if (string.IsNullOrWhiteSpace(_storyId))
        {
            gameObject.SetActive(true);
            return;
        }

        List<string> completeStoryList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;

        for (int i = 0, count = completeStoryList.Count; i < count; i++)
        {
            if (completeStoryList[i] != _storyId)
                continue;

            _target.SetActive(true);
        }

        _target.SetActive(false);
    }


    private void OnChangeSceneEvent()
    {
        MainStoryController.OnFinishStoryHandler -= Check;
        FadeInOutManager.Instance.OnFadeOutHandler -= Check;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }
}
