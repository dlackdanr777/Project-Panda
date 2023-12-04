using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;

public class Event1_0 : StoryEvent
{
    public override void EventStart(Action onComplate)
    {
        Debug.Log("½ÃÀÛ");
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + Camera.main.orthographicSize * 0.5f, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(gameObject, 1, 2, TweenMode.Quadratic, onComplate);
            Tween.TransformMove(gameObject,  new Vector3(-2.10f, -14, 0), 2, TweenMode.Spike);
        });

    }

    public override void EventCancel(Action onComplate = null)
    {
        
        Tween.Stop(gameObject);
        Tween.Stop(Camera.main.gameObject);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }

}
