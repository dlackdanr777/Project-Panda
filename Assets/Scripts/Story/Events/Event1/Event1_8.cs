using System;
using UnityEngine;
using Muks.Tween;

public class Event1_8 : InteractionStoryEvent
{
    private Vector3 _tempPos;


    public override void EventStart(Action onComplate)
    {
        _tempPos = gameObject.transform.position;
        _nextActionHandler = onComplate;

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 1, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, () =>
        {
            ShowFollowButton();
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(gameObject);
        transform.position = _tempPos;

        HideFollowButton();
    }


    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();
        Tween.TransformMove(gameObject, transform.position, 1, TweenMode.Smootherstep, () =>
        {
            _nextActionHandler?.Invoke();
        });
    }
}
