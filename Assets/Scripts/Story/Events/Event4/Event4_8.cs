using Muks.Tween;
using System;
using UnityEngine;

public class Event4_8 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic);
        Tween.SpriteRendererAlpha(gameObject, 0, 1, TweenMode.Quadratic);
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic);
        Tween.SpriteRendererAlpha(gameObject, 0, 1, TweenMode.Quadratic, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(gameObject);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
