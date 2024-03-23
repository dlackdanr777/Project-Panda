using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Tooltip("UI�� ����ٴ� ������Ʈ")]
    [SerializeField] private Transform _targetTrnsform;

    [Tooltip("���� ��ġ ����")]
    [SerializeField] private Vector3 _addPos;

    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� Ȱ��(������ ��� ������ Ȱ��)")]
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


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
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
