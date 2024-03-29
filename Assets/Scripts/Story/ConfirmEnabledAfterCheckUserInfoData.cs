using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>���� �����Ϳ��� bool���� Ȯ���� Ȱ��, ��Ȱ��ȭ �ϴ� ��ũ��Ʈ </summary>
public class ConfirmEnabledAfterCompletedOutro : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;

    [Header("Options")]
    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� Ȱ��(������ ��� ������ Ȱ��)")]
    [SerializeField] private string _storyId;
    [SerializeField] private UserInfo.StoryOutroType _outroType;
    [SerializeField] private bool _outroBoolCheck;


    private void Start()
    {
        Check();

        UIMainDialogue.OnFinishStoryHandler += Check;
        DatabaseManager.Instance.UserInfo.OnSetOutroHandler += Check;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;
    }


    private void Check(string id)
    {
        if (CheckCompletedOutro())
        {
            CheckCompletedStory();
        }
    }


    private void Check()
    {
        if (CheckCompletedOutro())
        {
            CheckCompletedStory();
        }
    }


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
    private bool CheckCompletedOutro()
    {
        if (DatabaseManager.Instance.UserInfo.GetStoryOutroBool(_outroType) != _outroBoolCheck)
        {
            gameObject.SetActive(false);
            return false;
        }

        gameObject.SetActive(true);
        return true;
    }


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
    private void CheckCompletedStory()
    {
        if (string.IsNullOrWhiteSpace(_storyId))
        {
            gameObject.SetActive(true);
            return;
        }

        List<string> completeStoryList = DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList;

        if (completeStoryList.Contains(_storyId))
        {
            _target.SetActive(true);
            return;
        }

        _target.SetActive(false);
    }


    private void OnChangeSceneEvent()
    {
        UIMainDialogue.OnFinishStoryHandler -= Check;
        DatabaseManager.Instance.UserInfo.OnSetOutroHandler -= Check;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }
}
