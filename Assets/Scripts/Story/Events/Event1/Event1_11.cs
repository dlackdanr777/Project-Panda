using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;
using System.Collections;

public class Event1_11 : InteractionStoryEvent
{
    [SerializeField] private UINavigation _uiNav;


    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        _nextActionHandler = onComplate;
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, () =>
        {
            ShowFollowButton();
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();
        _uiNav.Pop("InsideWood");
    }
  

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();
        _uiNav.Push("InsideWood");

        Tween.TransformMove(gameObject, transform.position, 4, TweenMode.Smootherstep, () =>
        {
            _uiNav.Push("Dialogue");
            _nextActionHandler?.Invoke();
        });
    }
}
