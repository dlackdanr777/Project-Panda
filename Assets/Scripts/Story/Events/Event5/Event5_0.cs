using Muks.Tween;
using System;
using UnityEngine;

public class Event5_0 : StoryEvent
{
    [SerializeField] private GameObject _exclamationMark;
    [SerializeField] private GameObject _starterPanda;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_exclamationMark, 0, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(_starterPanda.transform.position.x, _starterPanda.transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, () =>
        {
            Tween.TransformMove(_starterPanda.gameObject, _starterPanda.transform.position + transform.up, 1, TweenMode.Smootherstep, () =>
            {
                Tween.TransformMove(_starterPanda.gameObject, _starterPanda.transform.position - transform.up, 1, TweenMode.Smootherstep, onComplate);
            });
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _exclamationMark.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
