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



    private void Start()
    {
        CheckCompletedStory();

        StoryManager.Instance.OnAddCompletedStoryHandler += CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    /// <summary>인스펙터창에 적힌 스토리id가 완료된 상태면 아이콘을 활성화, 아닐경우 비활성화 하는 함수</summary>
    private void CheckCompletedStory()
    {
        if (string.IsNullOrWhiteSpace(_storyId))
        {
            gameObject.SetActive(true);
            return;
        }

        List<string> completeStoryList = StoryManager.Instance.StoryCompletedList;

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
        StoryManager.Instance.OnAddCompletedStoryHandler -= CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }
}
