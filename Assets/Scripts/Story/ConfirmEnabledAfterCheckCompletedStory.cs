using System;
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
    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� �� Ȱ��(������ ��� �ش����)")]
    [SerializeField] private string _disableStoryId;


    private bool _isStoryIdCompleted;
    private bool _isDisableStoryIdCompleted;

    private void Start()
    {
        CheckCompletedStory("0");

        UIMainDialogue.OnFinishStoryHandler += CheckCompletedStory;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
    private void CheckCompletedStory(string id)
    {
        List<string> completeStoryList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;

        //�̹� Ȯ���� ��ģ �����ε��� List�� ��ȸ�ϸ� ã�� ��� ���ҽ����� �� �� �ֱ⿡ ���´�.
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
