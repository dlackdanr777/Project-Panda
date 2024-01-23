using Muks.Tween;
using System;
using UnityEngine;

public class Event7_9 : StoryEvent
{
    [SerializeField] private GameObject _fishingGroundButton;
    public override void EventStart(Action onComplate)
    {
        _fishingGroundButton.SetActive(true);
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {
        _fishingGroundButton.SetActive(false);
    }
}
