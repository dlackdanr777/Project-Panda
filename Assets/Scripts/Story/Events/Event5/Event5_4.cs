using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event5_4 : InteractionStoryEvent
{
    public override void EventStart(Action onComplate)
    {
        _nextActionHandler = onComplate;
        // ���� ��� �����Ǹ� �������� ���ڸ�ä ��� ������ ����
        //���ڸ�ä ���� �� �̺�Ʈ ����
        ShowFollowButton();
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();

    }

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();

        // ���ڸ�ä �κ��丮�� �̵�

        _nextActionHandler?.Invoke();

    }
}
