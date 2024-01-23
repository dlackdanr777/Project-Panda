using Muks.Tween;
using System;
using UnityEngine;

public class Event5_3 : StoryEvent
{
    [SerializeField] private GameObject _starter;

    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, onComplate);
        _starter?.SetActive(true);

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 3, TweenMode.Smootherstep, () => 
        {
            //Tween.SpriteRendererAlpha(_starter, 1, 2, TweenMode.Quadratic, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        _starter?.SetActive(false);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
