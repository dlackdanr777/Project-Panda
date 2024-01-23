using Muks.Tween;
using System;
using UnityEngine;

public class MS02B_0 : StoryEvent
{
    [SerializeField] private GameObject _exclamationMark;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_exclamationMark, 0, 1, TweenMode.Quadratic); 
    }
    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _exclamationMark.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
