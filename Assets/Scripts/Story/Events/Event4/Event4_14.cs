using Muks.Tween;
using System;
using UnityEngine;


public class Event4_14 : StoryEvent
{
    [SerializeField] private GameObject _shopBrothers;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_shopBrothers, 0, 2, TweenMode.Quadratic);
        Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(_shopBrothers);
        Tween.Stop(gameObject);

        SpriteRenderer pandaBrotherRenderer = _shopBrothers.GetComponent<SpriteRenderer>();
        pandaBrotherRenderer.color = new Color(pandaBrotherRenderer.color.r, pandaBrotherRenderer.color.g, pandaBrotherRenderer.color.b, 0);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
