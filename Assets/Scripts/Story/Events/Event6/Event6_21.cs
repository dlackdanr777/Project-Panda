using Muks.Tween;
using System;
using UnityEngine;

public class Event6_21 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        // ī�޶� ��� Ȱ��ȭ
        // �ϱ��� ������Ʈ
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
