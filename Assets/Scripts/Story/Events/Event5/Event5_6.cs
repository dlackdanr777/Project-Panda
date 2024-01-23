using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event5_6 : StoryEvent
{

    [SerializeField] private GameObject _forestButton;


    public override void EventStart(Action onComplate)
    {
        _forestButton.SetActive(true);

        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {
        _forestButton.SetActive(false);
    }

}
