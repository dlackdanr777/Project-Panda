using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event2_2 : InteractionStoryEvent
{
    [SerializeField] private GameObject[] _changedObj;

    private int _clickCount;

    public override void EventStart(Action onComplate)
    {
        _clickCount = 0;
        ShowFollowButton();
        _nextActionHandler = onComplate;
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();
        _clickCount = 0;
    }

    protected override void OnFollowButtonClicked()
    {
        _clickCount++;
        if (_clickCount < _changedObj.Length)
        {
            _changedObj[_clickCount - 1].SetActive(false);
            _changedObj[_clickCount].SetActive(true);
        }

        else
        {
            HideFollowButton();
            _nextActionHandler?.Invoke();
        }

    }
}
