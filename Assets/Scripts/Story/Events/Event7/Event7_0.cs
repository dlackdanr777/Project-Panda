using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Event7_0 : StoryEvent
{
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_starterPanda, 1, 1, TweenMode.Quadratic);
        Tween.SpriteRendererAlpha(_Jiji, 1, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 1.5f, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer starterPandaRenderer = gameObject.GetComponent<SpriteRenderer>();
        starterPandaRenderer.color = new Color(starterPandaRenderer.color.r, starterPandaRenderer.color.g, starterPandaRenderer.color.b, 1);
        SpriteRenderer jijiRenderer = gameObject.GetComponent<SpriteRenderer>();
        jijiRenderer.color = new Color(jijiRenderer.color.r, jijiRenderer.color.g, jijiRenderer.color.b, 1);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
