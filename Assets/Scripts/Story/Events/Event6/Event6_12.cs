using Muks.Tween;
using System;
using UnityEngine;

public class Event6_12 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //��Ĭ��Ĭ ǥ��
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
