using Muks.Tween;
using System;
using UnityEngine;

public class Event5_8 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //���� ä�� Ȱ��ȭ
        //�� ��ư Ȱ��ȭ
        //�ϱ��忡 ù ä�� ���
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
