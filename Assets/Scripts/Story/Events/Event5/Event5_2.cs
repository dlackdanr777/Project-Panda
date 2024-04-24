using Muks.Tween;
using System;
using UnityEngine;

public class Event5_2 : StoryEvent
{
    [SerializeField] private GameObject _jiji;

    public override void EventStart(Action onComplate)
    {
        Tween.SpriteRendererAlpha(_jiji, 0, 2, TweenMode.Quadratic, () =>
        {
            _jiji.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Tween.SpriteRendererAlpha(_jiji, 1, 1, TweenMode.Quadratic);
            onComplate?.Invoke();
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _jiji.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
