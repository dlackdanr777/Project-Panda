using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI가 따라다닐 오브젝트")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("세부 위치 조정")]
    [SerializeField] private Vector3 _addPos;

    [Tooltip("해당 스토리ID가 완료 상태일 경우 활성(공백일 경우 무조건 활성)")]
    [SerializeField] private string _stroyId;

    private void Start()
    {
        if (_targetTrnsform == null)
            enabled = false;

        CheckCompletedStory();

        StoryManager.Instance.OnAddCompletedStoryHandler += CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    private void Update()
    {
        //Vector3 screenPointPos = Camera.main.WorldToScreenPoint(_targetTrnsform.position + _addPos);
        transform.position = Camera.main.WorldToScreenPoint(_targetTrnsform.position + _addPos);
    }


    /// <summary>인스펙터창에 적힌 스토리id가 완료된 상태면 아이콘을 활성화, 아닐경우 비활성화 하는 함수</summary>
    private void CheckCompletedStory()
    {
        if (string.IsNullOrWhiteSpace(_stroyId))
        {
            gameObject.SetActive(true);
            return;
        }
            
        List<string> completeStoryList = StoryManager.Instance.StoryCompletedList;

        for(int i = 0, count = completeStoryList.Count; i < count; i++)
        {
            if (completeStoryList[i] != _stroyId)
                continue;

            gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }


    private void OnChangeSceneEvent()
    {
        StoryManager.Instance.OnAddCompletedStoryHandler -= CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }
}
