using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;

public class Event1_0 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic, onComplate);
        });

    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }

}
