using Muks.Tween;
using System;
using UnityEngine;

public class Event6_0 : StoryEvent
{
    [SerializeField] private GameObject _exclamationMark;
    [SerializeField] private GameObject _starterPanda;
    [SerializeField] private GameObject _Jiji;
    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_exclamationMark, 0, 1, TweenMode.Quadratic);
        Vector3 targetPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 1, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_starterPanda.gameObject, 1, 1.5f, TweenMode.Quadratic, onComplate);
            Tween.SpriteRendererAlpha(_Jiji.gameObject, 1, 1.5f, TweenMode.Quadratic, onComplate);
        });
        onComplate?.Invoke();
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer startPandaRenderer = _starterPanda.GetComponent<SpriteRenderer>();
        startPandaRenderer.color = new Color(startPandaRenderer.color.r, startPandaRenderer.color.g, startPandaRenderer.color.b, 0);
        SpriteRenderer Jijirenderer = _Jiji.GetComponent<SpriteRenderer>();
        Jijirenderer.color = new Color(Jijirenderer.color.r, Jijirenderer.color.g, Jijirenderer.color.b, 0);
    }
}
