using System;
using UnityEngine;

public class Event6_3 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //�� �ε帮�� �Ҹ� �ֱ�
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
