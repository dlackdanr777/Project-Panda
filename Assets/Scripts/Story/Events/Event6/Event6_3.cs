using System;
using UnityEngine;

public class Event6_3 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //문 두드리는 소리 넣기
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
