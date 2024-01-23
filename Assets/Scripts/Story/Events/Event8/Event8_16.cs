using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Event8_16 : InteractionStoryEvent
{
    [SerializeField] private GameObject _bait;
    [SerializeField] private GameObject _starterPanda;

    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_starterPanda.transform.position.x, _starterPanda.transform.position.y, _starterPanda.transform.position.z);
        Tween.SpriteRendererAlpha(_bait, 1, 0.5f, TweenMode.Quadratic);
        Tween.TransformMove(_bait, targetPos, 1.5f, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_bait, 0, 0.5f, TweenMode.Quadratic);
        });

        ShowFollowButton();
        _nextActionHandler = onComplate;
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();

        SpriteRenderer fishButtonRenderer = _bait.GetComponent<SpriteRenderer>();
        fishButtonRenderer.color = new Color(fishButtonRenderer.color.r, fishButtonRenderer.color.g, fishButtonRenderer.color.b, 0);

    }

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();
        _nextActionHandler?.Invoke();
    }
}