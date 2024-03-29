using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>인스펙터 storyId가 완료 스토리 리스트 존재하는지 확인해 활성, 비활성화 하는 스크립트 </summary>
public class ConfirmEnabledAfterCheckCompletedStory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;

    [Header("Options")]
    [Tooltip("해당 스토리ID가 완료 상태일 경우 활성(공백일 경우 무조건 활성)")]
    [SerializeField] private string _storyId;
    [Tooltip("해당 스토리ID가 완료 상태일 경우 비 활성(공백일 경우 해당없음)")]
    [SerializeField] private string _disableStoryId;


    private bool _isStoryIdCompleted;
    private bool _isDisableStoryIdCompleted;

    private void Start()
    {
        CheckCompletedStory("0");

        UIMainDialogue.OnFinishStoryHandler += CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    /// <summary>인스펙터창에 적힌 스토리id가 완료된 상태면 아이콘을 활성화, 아닐경우 비활성화 하는 함수</summary>
    private void CheckCompletedStory(string id)
    {
        List<string> completeStoryList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;

        //이미 확인을 마친 상태인데도 List를 순회하며 찾는 경우 리소스낭비가 될 수 있기에 막는다.
        if(!_isStoryIdCompleted)
        {
            if (completeStoryList.Contains(_storyId))
            {
                _isStoryIdCompleted = true;
            }
        }

        if (!_isDisableStoryIdCompleted)
        {
            if (completeStoryList.Contains(_disableStoryId))
            {
                _isDisableStoryIdCompleted = true;
            }
        }

        if (_isStoryIdCompleted)
        {
            if(string.IsNullOrWhiteSpace(_disableStoryId) || !_isDisableStoryIdCompleted)
            {
                _target.SetActive(true);
                return;
            }

            else if (_isDisableStoryIdCompleted)
            {
                _target.SetActive(false);
                return;
            }
        }

        else
        {
            if (_isDisableStoryIdCompleted)
            {
                _target.SetActive(false);
                return;
            }

            if (string.IsNullOrWhiteSpace(_storyId))
            {
                gameObject.SetActive(true);
                return;
            }

            _target.SetActive(false);
        }
 
    }


    private void OnChangeSceneEvent()
    {
        UIMainDialogue.OnFinishStoryHandler -= CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }
}
