using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StoryOutroType
{
    Story1,
    Syory2
}



/// <summary>���� �����Ϳ��� bool���� Ȯ���� Ȱ��, ��Ȱ��ȭ �ϴ� ��ũ��Ʈ </summary>
public class ConfirmEnabledAfterCompletedOutro : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;

    [Header("Options")]
    [Tooltip("�ش� ���丮ID�� �Ϸ� ������ ��� Ȱ��(������ ��� ������ Ȱ��)")]
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


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
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


    /// <summary>�ν�����â�� ���� ���丮id�� �Ϸ�� ���¸� �������� Ȱ��ȭ, �ƴҰ�� ��Ȱ��ȭ �ϴ� �Լ�</summary>
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
