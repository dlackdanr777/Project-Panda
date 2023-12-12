using Muks.Tween;
using System;
using UnityEngine;

public class Event6_8 : StoryEvent
{
    [SerializeField] private GameObject _darkPanda;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_darkPanda, 0, 2, TweenMode.Quadratic, onComplate);
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _darkPanda.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
