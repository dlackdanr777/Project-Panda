using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event6_15 : InteractionStoryEvent
{
    [SerializeField] private GameObject _flash;

    public override void EventStart(Action onComplate)
    {
        // ī�޶� ��ư Ȱ��ȭ
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

        // �÷��� ȿ��
        Tween.IamgeAlpha(_flash, 1, 0.1f, TweenMode.Quadratic, () =>
        {
            Tween.IamgeAlpha(_flash, 0, 0.2f, TweenMode.Quadratic, _nextActionHandler);
        });
    }
}
