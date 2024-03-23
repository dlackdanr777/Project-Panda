using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>�ν����� storyId�� �Ϸ� ���丮 ����Ʈ �����ϴ��� Ȯ���� Ȱ��, ��Ȱ��ȭ �ϴ� ��ũ��Ʈ </summary>
public class ConfirmEnabledAfterCheckCompletedStory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;

    [Header("Options")]
    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� Ȱ��(������ ��� ������ Ȱ��)")]
    [SerializeField] private string _storyId;



    private void Start()
    {
        CheckCompletedStory();

        StoryManager.Instance.OnAddCompletedStoryHandler += CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
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
