using Muks.Tween;
using System;
using UnityEngine;

public class Event6_4 : StoryEvent
{
    [SerializeField] private GameObject _darkPanda;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_darkPanda.transform.position.x, _darkPanda.transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 1, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_darkPanda, 1, 2, TweenMode.Quadratic, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _darkPanda.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
