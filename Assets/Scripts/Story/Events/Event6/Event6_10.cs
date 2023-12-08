using Muks.Tween;
using System;
using UnityEngine;

/// <summary>
/// 파파라치 나타남 </summary>
public class Event6_10 : StoryEvent
{
    [SerializeField] private GameObject _paparazzi;
    public override void EventStart(Action onComplate)
    {
        Vector3 targetPos = new Vector3(_paparazzi.transform.position.x, _paparazzi.transform.position.y, Camera.main.transform.position.z);
        Tween.TransformMove(Camera.main.gameObject, targetPos, 1, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_paparazzi.gameObject, 1, 2, TweenMode.Quadratic, onComplate);
        });
    }

    public override void EventCancel(Action onComplate = null)
    {
        SpriteRenderer renderer = _paparazzi.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
