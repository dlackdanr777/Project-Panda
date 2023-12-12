using Muks.Tween;
using System;
using UnityEngine;

public class Event5_1 : StoryEvent
{
    [SerializeField] private GameObject _dragonfly;
    [SerializeField] private GameObject _jiji;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_jiji.transform.position.x, _jiji.transform.position.y, 0);
        Tween.TransformMove(_jiji.gameObject, targetPos + transform.up, 1, TweenMode.Smootherstep, () =>
        {
            Tween.TransformMove(_jiji.gameObject, targetPos - transform.up, 1, TweenMode.Smootherstep);
            Tween.SpriteRendererAlpha(_dragonfly.gameObject, 0, 2, TweenMode.Quadratic);
            Tween.TransformMove(_dragonfly.gameObject, _dragonfly.transform.position + transform.up, 2, TweenMode.Smootherstep, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _dragonfly.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
