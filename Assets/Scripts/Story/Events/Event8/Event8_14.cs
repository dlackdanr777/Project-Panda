using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_14 : InteractionStoryEvent
{

    public override void EventStart(Action onComplate)
    {
        ShowFollowButton();
        _nextActionHandler = onComplate;
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();

    }

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();
        _nextActionHandler?.Invoke();
    }
}
