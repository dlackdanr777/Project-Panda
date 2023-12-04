using Muks.Tween;
using System;
using UnityEngine;

public class Event5_8 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //곤충 채집 활성화
        //맵 버튼 활성화
        //일기장에 첫 채집 기록
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
