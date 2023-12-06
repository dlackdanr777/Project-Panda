using Muks.Tween;
using System;
using UnityEngine;

public class Event6_12 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        //ÂûÄ¬ÂûÄ¬ Ç¥Çö
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {

    }
}
