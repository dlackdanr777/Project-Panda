using Muks.Tween;
using System;
using UnityEngine;

public class Event4_3 : StoryEvent
{
    [SerializeField] private GameObject _shopElderBrother;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_shopElderBrother, 0, 2, TweenMode.Quadratic, onComplate);
        Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic);
        
    }

    public override void EventCancel(Action onComplate = null)
    {
        Tween.Stop(_shopElderBrother);
        Tween.Stop(gameObject);

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        SpriteRenderer pandaBrotherRenderer = _shopElderBrother.GetComponent<SpriteRenderer>();
        pandaBrotherRenderer.color = new Color(pandaBrotherRenderer.color.r, pandaBrotherRenderer.color.g, pandaBrotherRenderer.color.b, 1);
    }
}
