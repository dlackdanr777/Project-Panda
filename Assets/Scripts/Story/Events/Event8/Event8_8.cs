using BT;
using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event8_8 : InteractionStoryEvent
{

    [SerializeField] private GameObject _fish;
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
        // 버튼 클릭하면 강 정화

        // 물고기 폴짝..
        Tween.SpriteRendererAlpha(_fish, 1, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(_fish.transform.position.x, _fish.transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, (Camera.main.gameObject.transform.position + targetPos) / 2, 3, TweenMode.Smootherstep, () =>
        {
            Tween.TransformMove(_fish.gameObject, _fish.transform.position + transform.up, 0.5f, TweenMode.Smootherstep, () =>
            {
                Tween.SpriteRendererAlpha(_fish, 0, 0.5f, TweenMode.Quadratic);
                Tween.TransformMove(_fish.gameObject, _fish.transform.position - transform.up, 0.5f, TweenMode.Smootherstep, _nextActionHandler);
            });
        });
    }
}
