using Muks.Tween;
using System;
using UnityEngine;

public class Event6_21 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        // 카메라 기능 활성화
        // 일기장 업데이트
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
